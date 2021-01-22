// <copyright file="ConstructorTests.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System;
using AspNetWebpack.AssetHelpers.Testing;
using FluentAssertions;
using Xunit;

namespace AspNetWebpack.AssetHelpers.Tests.AssetServiceTests
{
    public sealed class ConstructorTests : IDisposable
    {
        private ConstructorFixture? _fixture;

        public void Dispose()
        {
            _fixture?.Dispose();
        }

        [Fact]
        public void Constructor_Development_ShouldSetAllVariables()
        {
            // Arrange
            _fixture = new ConstructorFixture("Development");

            // Act
            var result = _fixture.Construct();

            // Assert
            result.DevelopmentMode.Should().BeTrue();
            result.HttpClient.Should().NotBeNull();
            result.AssetBaseFilePath.Should().Be($"{_fixture.WebpackOptions.InternalDevServer}{_fixture.WebpackOptions.AssetsPublicPath}");
            result.ManifestPath.Should().Be($"{result.AssetBaseFilePath}{_fixture.WebpackOptions.ManifestFile}");
            result.AssetPath.Should().Be($"{_fixture.WebpackOptions.PublicDevServer}{_fixture.WebpackOptions.AssetsPublicPath}");
        }

        [Fact]
        public void Constructor_Production_ShouldSetAllVariables()
        {
            // Arrange
            _fixture = new ConstructorFixture("Production");

            // Act
            var result = _fixture.Construct();

            // Assert
            result.DevelopmentMode.Should().BeFalse();
            result.HttpClient.Should().BeNull();
            result.AssetBaseFilePath.Should().Be($"{_fixture.WebHostEnvironment.WebRootPath}{_fixture.WebpackOptions.AssetsPublicPath}");
            result.ManifestPath.Should().Be($"{result.AssetBaseFilePath}{_fixture.WebpackOptions.ManifestFile}");
            result.AssetPath.Should().Be(_fixture.WebpackOptions.AssetsPublicPath);
        }
    }
}
