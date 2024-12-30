// <copyright file="GetScriptTagTests.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System.Threading.Tasks;
using AspNetWebpack.Tests.Data;
using Xunit;

namespace AspNetWebpack.Tests.AssetServiceTests;

public sealed class GetScriptTagTests
{
    [Fact]
    public async Task GetScriptTag_EmptyString_ShouldReturnEmptyHtmlString()
    {
        // Arrange
        var fixture = new GetScriptTagFixture(string.Empty);

        // Act
        var result = await fixture.GetScriptTagAsync();

        // Assert
        fixture.VerifyEmpty(result);
    }

    [Fact]
    public async Task GetScriptTag_InvalidBundle_ShouldReturnEmptyHtmlString()
    {
        // Arrange
        var fixture = new GetScriptTagFixture(AssetServiceBaseFixture.InvalidBundle);

        // Act
        var result = await fixture.GetScriptTagAsync();

        // Assert
        fixture.VerifyNonExisting(result);
    }

    [Fact]
    public async Task GetScriptTag_ValidBundle_ShouldReturnScriptTag()
    {
        // Arrange
        var fixture = new GetScriptTagFixture(AssetServiceBaseFixture.ValidBundleWithoutExtension);

        // Act
        var result = await fixture.GetScriptTagAsync();

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
    public async Task GetScriptTag_ValidBundleAsync_ShouldReturnAsyncScriptTag()
    {
        // Arrange
        var fixture = new GetScriptTagFixture(AssetServiceBaseFixture.ValidBundleWithoutExtension, ScriptLoad.Async);

        // Act
        var result = await fixture.GetScriptTagAsync();

        // Assert
        fixture.VerifyExisting(result);
    }

    [Fact]
    public async Task GetScriptTag_ValidBundleDefer_ShouldReturnDeferScriptTag()
    {
        // Arrange
        var fixture = new GetScriptTagFixture(AssetServiceBaseFixture.ValidBundleWithoutExtension, ScriptLoad.Defer);

        // Act
        var result = await fixture.GetScriptTagAsync();

        // Assert
        fixture.VerifyExisting(result);
    }

    [Fact]
    public async Task GetScriptTag_ValidBundleAsyncDefer_ShouldReturnAsyncDeferScriptTag()
    {
        // Arrange
        var fixture = new GetScriptTagFixture(AssetServiceBaseFixture.ValidBundleWithoutExtension, ScriptLoad.AsyncDefer);

        // Act
        var result = await fixture.GetScriptTagAsync();

        // Assert
        fixture.VerifyExisting(result);
    }

    [Fact]
    public async Task GetLinkTag_FallbackEmptyString_ShouldReturnEmptyHtmlString()
    {
        // Arrange
        var fixture = new GetScriptTagFixture(AssetServiceBaseFixture.InvalidBundle, string.Empty);

        // Act
        var result = await fixture.GetScriptTagAsync();

        // Assert
        fixture.VerifyFallbackEmpty(result);
    }

    [Fact]
    public async Task GetLinkTag_InvalidFallbackBundle_ShouldReturnEmptyHtmlString()
    {
        // Arrange
        var fixture = new GetScriptTagFixture(AssetServiceBaseFixture.InvalidBundle, AssetServiceBaseFixture.InvalidBundle);

        // Act
        var result = await fixture.GetScriptTagAsync();

        // Assert
        fixture.VerifyFallbackNonExisting(result);
    }

    [Fact]
    public async Task GetLinkTag_ValidFallbackBundle_ShouldReturnStyleTag()
    {
        // Arrange
        var fixture = new GetScriptTagFixture(AssetServiceBaseFixture.InvalidBundle, AssetServiceBaseFixture.ValidFallbackBundleWithoutExtension);

        // Act
        var result = await fixture.GetScriptTagAsync();

        // Assert
        fixture.VerifyFallbackExisting(result);
    }

    [Fact]
    public async Task GetLinkTag_ValidFallbackBundleWithExtension_ShouldReturnStyleTag()
    {
        // Arrange
        var fixture = new GetScriptTagFixture(AssetServiceBaseFixture.InvalidBundle, GetScriptTagFixture.ValidFallbackBundleWithExtension);

        // Act
        var result = await fixture.GetScriptTagAsync();

        // Assert
        fixture.VerifyFallbackExisting(result);
    }

    [Fact]
    public async Task GetScriptTag_ValidFallbackBundleAsync_ShouldReturnAsyncScriptTag()
    {
        // Arrange
        var fixture = new GetScriptTagFixture(AssetServiceBaseFixture.InvalidBundle, AssetServiceBaseFixture.ValidFallbackBundleWithoutExtension, ScriptLoad.Async);

        // Act
        var result = await fixture.GetScriptTagAsync();

        // Assert
        fixture.VerifyFallbackExisting(result);
    }

    [Fact]
    public async Task GetScriptTag_ValidFallbackBundleDefer_ShouldReturnDeferScriptTag()
    {
        // Arrange
        var fixture = new GetScriptTagFixture(AssetServiceBaseFixture.ValidBundleWithoutExtension, AssetServiceBaseFixture.ValidFallbackBundleWithoutExtension, ScriptLoad.Defer);

        // Act
        var result = await fixture.GetScriptTagAsync();

        // Assert
        fixture.VerifyExisting(result);
    }

    [Fact]
    public async Task GetScriptTag_ValidFallbackBundleAsyncDefer_ShouldReturnAsyncDeferScriptTag()
    {
        // Arrange
        var fixture = new GetScriptTagFixture(AssetServiceBaseFixture.ValidBundleWithoutExtension, AssetServiceBaseFixture.ValidFallbackBundleWithoutExtension, ScriptLoad.AsyncDefer);

        // Act
        var result = await fixture.GetScriptTagAsync();

        // Assert
        fixture.VerifyExisting(result);
    }
}
