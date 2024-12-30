// <copyright file="ServiceCollectionExtensionsTests.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System;
using System.Net.Http;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace AspNetWebpack.Tests;

public class ServiceCollectionExtensionsTests
{
    private readonly IServiceCollection _serviceCollection;
    private readonly IConfiguration _configuration;

    public ServiceCollectionExtensionsTests()
    {
        _serviceCollection = new ServiceCollection();

        var configuration = new Mock<IConfiguration>();
        var configurationSection = new Mock<IConfigurationSection>();
        configuration
            .Setup(x => x.GetSection("Webpack"))
            .Returns(configurationSection.Object);

        _configuration = configuration.Object;
        _serviceCollection.AddSingleton(_configuration);
    }

    [Fact]
    public void AddAspNetWebpack_ConfigurationNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var webHostEnvironment = new Mock<IWebHostEnvironment>().Object;

        // Act
        Action act = () => new ServiceCollection().AddAspNetWebpack(null!, webHostEnvironment);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void AddAspNetWebpack_Development_ShouldResolveServices()
    {
        // Arrange
        var webHostEnvironment = CreateWebHostEnvironment("Development");
        _serviceCollection.AddSingleton(webHostEnvironment);

        // Act
        _serviceCollection.AddAspNetWebpack(_configuration, webHostEnvironment);
        var provider = _serviceCollection.BuildServiceProvider();
        var httpClientFactory = provider.GetService<IHttpClientFactory>();
        var webpackOptions = provider.GetService<IOptions<WebpackOptions>>();
        var manifestService = provider.GetService<IManifestService>();
        var tagBuilder = provider.GetService<ITagBuilder>();
        var assetService = provider.GetService<IAssetService>();

        // Assert
        httpClientFactory.Should().NotBeNull();
        VerifyDefaultServices(webpackOptions, manifestService, tagBuilder, assetService);
    }

    [Fact]
    public void AddAspNetWebpack_Production_ShouldResolveServices()
    {
        // Arrange
        var webHostEnvironment = CreateWebHostEnvironment("Production");
        _serviceCollection.AddSingleton(webHostEnvironment);

        // Act
        _serviceCollection.AddAspNetWebpack(_configuration, webHostEnvironment);
        var provider = _serviceCollection.BuildServiceProvider();
        var httpClientFactory = provider.GetService<IHttpClientFactory>();
        var webpackOptions = provider.GetService<IOptions<WebpackOptions>>();
        var manifestService = provider.GetService<IManifestService>();
        var tagBuilder = provider.GetService<ITagBuilder>();
        var assetService = provider.GetService<IAssetService>();

        // Assert
        httpClientFactory.Should().BeNull();
        VerifyDefaultServices(webpackOptions, manifestService, tagBuilder, assetService);
    }

    private static void VerifyDefaultServices(
        IOptions<WebpackOptions>? webpackOptions,
        IManifestService? manifestService,
        ITagBuilder? tagBuilder,
        IAssetService? assetService)
    {
        webpackOptions.Should().NotBeNull();
        manifestService.Should().NotBeNull();
        tagBuilder.Should().NotBeNull();
        assetService.Should().NotBeNull();
    }

    private static IWebHostEnvironment CreateWebHostEnvironment(string environment)
    {
        var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        webHostEnvironmentMock
            .SetupGet(x => x.EnvironmentName)
            .Returns(environment);

        return webHostEnvironmentMock.Object;
    }
}
