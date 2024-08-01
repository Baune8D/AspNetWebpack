// <copyright file="GetBundlePathTests.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;
using AspNetWebpack.Testing;
using FluentAssertions;
using Xunit;

namespace AspNetWebpack.Tests.AssetServiceTests
{
    public sealed class GetBundlePathTests
    {
        [Fact]
        public async Task GetBundlePath_EmptyString_ShouldReturnNull()
        {
            // Arrange
            var fixture = new GetBundlePathFixture(string.Empty);

            // Act
            var result = await fixture.GetBundlePathAsync();

            // Assert
            fixture.VerifyEmpty(result);
        }

        [Fact]
        public async Task GetBundlePath_ValidBundleWithoutExtension_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var fixture = new GetBundlePathFixture(AssetServiceBaseFixture.ValidBundleWithoutExtension);

            // Act
            Func<Task> act = () => fixture.GetBundlePathAsync();

            // Assert
            await act.Should()
                .ThrowExactlyAsync<InvalidOperationException>()
                .WithMessage("A file extension is needed either in bundle name or as file type parameter.");
        }

        [Fact]
        public async Task GetBundlePath_ValidBundleWithExtensionAndFileTypeParam_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var fixture = new GetBundlePathFixture(GetBundlePathFixture.ValidBundleWithExtension, FileType.CSS);

            // Act
            Func<Task> act = () => fixture.GetBundlePathAsync();

            // Assert
            await act.Should()
                .ThrowExactlyAsync<InvalidOperationException>()
                .WithMessage("If bundle name already has an extension then do not specify it again as file type parameter.");
        }

        [Fact]
        public async Task GetBundlePath_InvalidBundleWithExtension_ShouldReturnNull()
        {
            // Arrange
            var fixture = new GetBundlePathFixture(GetBundlePathFixture.InvalidBundleWithExtension);

            // Act
            var result = await fixture.GetBundlePathAsync();

            // Assert
            fixture.VerifyNonExisting(result);
        }

        [Fact]
        public async Task GetBundlePath_ValidBundleWithExtension_ShouldReturnBundlePath()
        {
            // Arrange
            var fixture = new GetBundlePathFixture(GetBundlePathFixture.ValidBundleWithExtension);

            // Act
            var result = await fixture.GetBundlePathAsync();

            // Assert
            fixture.VerifyExisting(result);
        }

        [Fact]
        public async Task GetBundlePath_InvalidBundleWithExtensionAsParam_ShouldReturnNull()
        {
            // Arrange
            var fixture = new GetBundlePathFixture(AssetServiceBaseFixture.ValidBundleWithoutExtension, FileType.CSS);

            // Act
            var result = await fixture.GetBundlePathAsync();

            // Assert
            fixture.VerifyNonExisting(result);
        }

        [Fact]
        public async Task GetBundlePath_ValidBundleWithExtensionAsParam_ShouldReturnBundlePath()
        {
            // Arrange
            var fixture = new GetBundlePathFixture(AssetServiceBaseFixture.ValidBundleWithoutExtension, FileType.JS);

            // Act
            var result = await fixture.GetBundlePathAsync();

            // Assert
            fixture.VerifyExisting(result);
        }
    }
}
