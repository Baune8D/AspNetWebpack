// <copyright file="ServiceCollectionExtensions.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System;
using System.IO.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace AspNetWebpack;

/// <summary>
/// Extensions methods for IServiceCollection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds AssetService and necessary dependencies.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="webHostEnvironment">The hosting environment.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddAspNetWebpack(this IServiceCollection serviceCollection, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        if (webHostEnvironment.IsDevelopment())
        {
            serviceCollection.AddHttpClient();
        }

        serviceCollection.Configure<WebpackOptions>(configuration.GetSection("Webpack"));
        serviceCollection.TryAddTransient<IFileSystem, FileSystem>();
        serviceCollection.AddSingleton<ISharedSettings, SharedSettings>();
        serviceCollection.AddSingleton<ITagBuilder, TagBuilder>();
        serviceCollection.AddSingleton<IManifestService, ManifestService>();
        serviceCollection.AddSingleton<IAssetService, AssetService>();

        return serviceCollection;
    }
}
