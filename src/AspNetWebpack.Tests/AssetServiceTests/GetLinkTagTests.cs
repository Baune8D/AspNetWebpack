// <copyright file="GetLinkTagTests.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System.Threading.Tasks;
using AspNetWebpack.Testing;
using Xunit;

namespace AspNetWebpack.Tests.AssetServiceTests
{
    public sealed class GetLinkTagTests
    {
        [Fact]
        public async Task GetLinkTag_EmptyString_ShouldReturnEmptyHtmlString()
        {
            // Arrange
            var fixture = new GetLinkTagFixture(string.Empty);

            // Act
            var result = await fixture.GetLinkTagAsync();

            // Assert
            fixture.VerifyEmpty(result);
        }

        [Fact]
        public async Task GetLinkTag_InvalidBundle_ShouldReturnEmptyHtmlString()
        {
            // Arrange
            var fixture = new GetLinkTagFixture(AssetServiceBaseFixture.InvalidBundle);

            // Act
            var result = await fixture.GetLinkTagAsync();

            // Assert
            fixture.VerifyNonExisting(result);
        }

        [Fact]
        public async Task GetLinkTag_ValidBundle_ShouldReturnLinkTag()
        {
            // Arrange
            var fixture = new GetLinkTagFixture(AssetServiceBaseFixture.ValidBundleWithoutExtension);

            // Act
            var result = await fixture.GetLinkTagAsync();

            // Assert
            fixture.VerifyExisting(result);
        }

        [Fact]
        public async Task GetLinkTag_ValidBundleWithExtension_ShouldReturnStyleTag()
        {
            // Arrange
            var fixture = new GetLinkTagFixture(GetLinkTagFixture.ValidBundleWithExtension);

            // Act
            var result = await fixture.GetLinkTagAsync();

            // Assert
            fixture.VerifyExisting(result);
        }

        [Fact]
        public async Task GetLinkTag_FallbackEmptyString_ShouldReturnEmptyHtmlString()
        {
            // Arrange
            var fixture = new GetLinkTagFixture(AssetServiceBaseFixture.InvalidBundle, string.Empty);

            // Act
            var result = await fixture.GetLinkTagAsync();

            // Assert
            fixture.VerifyFallbackEmpty(result);
        }

        [Fact]
        public async Task GetLinkTag_InvalidFallbackBundle_ShouldReturnEmptyHtmlString()
        {
            // Arrange
            var fixture = new GetLinkTagFixture(AssetServiceBaseFixture.InvalidBundle, AssetServiceBaseFixture.InvalidBundle);

            // Act
            var result = await fixture.GetLinkTagAsync();

            // Assert
            fixture.VerifyFallbackNonExisting(result);
        }

        [Fact]
        public async Task GetLinkTag_ValidFallbackBundle_ShouldReturnStyleTag()
        {
            // Arrange
            var fixture = new GetLinkTagFixture(AssetServiceBaseFixture.InvalidBundle, AssetServiceBaseFixture.ValidFallbackBundleWithoutExtension);

            // Act
            var result = await fixture.GetLinkTagAsync();

            // Assert
            fixture.VerifyFallbackExisting(result);
        }

        [Fact]
        public async Task GetLinkTag_ValidFallbackBundleWithExtension_ShouldReturnStyleTag()
        {
            // Arrange
            var fixture = new GetLinkTagFixture(AssetServiceBaseFixture.InvalidBundle, GetLinkTagFixture.ValidFallbackBundleWithExtension);

            // Act
            var result = await fixture.GetLinkTagAsync();

            // Assert
            fixture.VerifyFallbackExisting(result);
        }
    }
}
