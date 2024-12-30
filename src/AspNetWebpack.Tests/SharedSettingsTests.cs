// <copyright file="SharedSettingsTests.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System;
using AspNetWebpack.Tests.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace AspNetWebpack.Tests;

public class SharedSettingsTests
{
    private const string PublicDevServer = "https://public.dev";
    private const string InternalDevServer = "https://internal.dev";
    private const string PublicPath = "/public";
    private const string ManifestFile = "manifest.json";

    private readonly Mock<IOptions<WebpackOptions>> _optionsMock;

    public SharedSettingsTests()
    {
        _optionsMock = new Mock<IOptions<WebpackOptions>>();

        _optionsMock
            .SetupGet(x => x.Value)
            .Returns(new WebpackOptions
            {
                PublicDevServer = PublicDevServer,
                InternalDevServer = InternalDevServer,
                PublicPath = PublicPath,
                ManifestFile = ManifestFile,
            });
    }

    private static string DevAssetsDirectoryPathResult => $"{InternalDevServer}{PublicPath}";

    private static string DevAssetsWebPathResult => $"{PublicDevServer}{PublicPath}";

    private static string DevManifestPathResult => $"{DevAssetsDirectoryPathResult}{ManifestFile}";

    private static string ProdAssetsDirectoryPathResult => $"{TestValues.WebRootPath}{PublicPath}";

    private static string ProdAssetsWebPathResult => PublicPath;

    private static string ProdManifestPathResult => $"{ProdAssetsDirectoryPathResult}{ManifestFile}";

    [Fact]
    public void Constructor_OptionsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var webHostEnvironment = new Mock<IWebHostEnvironment>();

        // Act
        Action act = () => _ = new SharedSettings(null!, webHostEnvironment.Object);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>();
        webHostEnvironment.VerifyNoOtherCalls();
    }

    [Fact]
    public void Constructor_WebHostEnvironmentNull_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => _ = new SharedSettings(_optionsMock.Object, null!);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>();
        _optionsMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Constructor_Development_ShouldSetAllVariables()
    {
        // Arrange
        var webHostEnvironmentMock = DependencyMocker.GetWebHostEnvironment(TestValues.Development);

        // Act
        var sharedSettings = new SharedSettings(_optionsMock.Object, webHostEnvironmentMock.Object);

        // Assert
        sharedSettings.DevelopmentMode.Should().BeTrue();
        sharedSettings.AssetsDirectoryPath.Should().Be(DevAssetsDirectoryPathResult);
        sharedSettings.AssetsWebPath.Should().Be(DevAssetsWebPathResult);
        sharedSettings.ManifestPath.Should().Be(DevManifestPathResult);
        _optionsMock.VerifyGet(x => x.Value, Times.Exactly(5));
        _optionsMock.VerifyNoOtherCalls();
        webHostEnvironmentMock.VerifyGet(x => x.EnvironmentName, Times.Once);
        webHostEnvironmentMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void Constructor_Production_ShouldSetAllVariables()
    {
        // Arrange
        var webHostEnvironmentMock = DependencyMocker.GetWebHostEnvironment(TestValues.Production);

        // Act
        var sharedSettings = new SharedSettings(_optionsMock.Object, webHostEnvironmentMock.Object);

        // Assert
        sharedSettings.DevelopmentMode.Should().BeFalse();
        sharedSettings.AssetsDirectoryPath.Should().Be(ProdAssetsDirectoryPathResult);
        sharedSettings.AssetsWebPath.Should().Be(ProdAssetsWebPathResult);
        sharedSettings.ManifestPath.Should().Be(ProdManifestPathResult);
        _optionsMock.VerifyGet(x => x.Value, Times.Exactly(3));
        _optionsMock.VerifyNoOtherCalls();
        webHostEnvironmentMock.VerifyGet(x => x.EnvironmentName, Times.Once);
        webHostEnvironmentMock.VerifyGet(x => x.WebRootPath, Times.Once);
        webHostEnvironmentMock.VerifyNoOtherCalls();
    }
}
