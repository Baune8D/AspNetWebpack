// <copyright file="GetStyleTagTests.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System.Threading.Tasks;
using AspNetWebpack.AssetHelpers.Testing;
using Xunit;

namespace AspNetWebpack.AssetHelpers.Tests.AssetServiceTests
{
    public sealed class GetStyleTagTests
    {
        [Fact]
        public async Task GetStyleTag_EmptyString_ShouldReturnEmptyHtmlString()
        {
            // Arrange
            var fixture = new GetStyleTagFixture(string.Empty);

            // Act
            var result = await fixture.GetStyleTagAsync();

            // Assert
            fixture.VerifyEmpty(result);
        }

        [Fact]
        public async Task GetStyleTag_InvalidBundle_ShouldReturnEmptyHtmlString()
        {
            // Arrange
            var fixture = new GetStyleTagFixture(AssetServiceBaseFixture.InvalidBundle);

            // Act
            var result = await fixture.GetStyleTagAsync();

            // Assert
            fixture.VerifyNonExisting(result);
        }

        [Fact]
        public async Task GetStyleTag_ValidBundle_ShouldReturnStyleTag()
        {
            // Arrange
            var fixture = new GetStyleTagFixture(AssetServiceBaseFixture.ValidBundleWithoutExtension);

            // Act
            var result = await fixture.GetStyleTagAsync();

            // Assert
            fixture.VerifyExisting(result);
        }

        [Fact]
        public async Task GetStyleTag_ValidBundleWithExtension_ShouldReturnStyleTag()
        {
            // Arrange
            var fixture = new GetStyleTagFixture(GetStyleTagFixture.ValidBundleWithExtension);

            // Act
            var result = await fixture.GetStyleTagAsync();

            // Assert
            fixture.VerifyExisting(result);
        }

        [Fact]
        public async Task GetStyleTag_FallbackEmptyString_ShouldReturnEmptyHtmlString()
        {
            // Arrange
            var fixture = new GetStyleTagFixture(AssetServiceBaseFixture.InvalidBundle, string.Empty);

            // Act
            var result = await fixture.GetStyleTagAsync();

            // Assert
            fixture.VerifyFallbackEmpty(result);
        }

        [Fact]
        public async Task GetStyleTag_InvalidFallbackBundle_ShouldReturnEmptyHtmlString()
        {
            // Arrange
            var fixture = new GetStyleTagFixture(AssetServiceBaseFixture.InvalidBundle, AssetServiceBaseFixture.InvalidBundle);

            // Act
            var result = await fixture.GetStyleTagAsync();

            // Assert
            fixture.VerifyFallbackNonExisting(result);
        }

        [Fact]
        public async Task GetStyleTag_ValidFallbackBundle_ShouldReturnStyleTag()
        {
            // Arrange
            var fixture = new GetStyleTagFixture(AssetServiceBaseFixture.InvalidBundle, AssetServiceBaseFixture.ValidFallbackBundleWithoutExtension);

            // Act
            var result = await fixture.GetStyleTagAsync();

            // Assert
            fixture.VerifyFallbackExisting(result);
        }

        [Fact]
        public async Task GetStyleTag_ValidFallbackBundleWithExtension_ShouldReturnStyleTag()
        {
            // Arrange
            var fixture = new GetStyleTagFixture(AssetServiceBaseFixture.InvalidBundle, GetStyleTagFixture.ValidFallbackBundleWithExtension);

            // Act
            var result = await fixture.GetStyleTagAsync();

            // Assert
            fixture.VerifyFallbackExisting(result);
        }
    }
}
