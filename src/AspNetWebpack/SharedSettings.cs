// <copyright file="SharedSettings.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace AspNetWebpack;

/// <summary>
/// A collection of shared settings for other services.
/// </summary>
public class SharedSettings : ISharedSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SharedSettings"/> class.
    /// </summary>
    /// <param name="options">Webpack options.</param>
    /// <param name="webHostEnvironment">Web host environment.</param>
    public SharedSettings(IOptions<WebpackOptions> options, IWebHostEnvironment webHostEnvironment)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(webHostEnvironment);

        DevelopmentMode = webHostEnvironment.IsDevelopment();

        AssetsDirectoryPath = DevelopmentMode
            ? options.Value.InternalDevServer + options.Value.PublicPath
            : webHostEnvironment.WebRootPath + options.Value.PublicPath;

        AssetsWebPath = DevelopmentMode
            ? options.Value.PublicDevServer + options.Value.PublicPath
            : options.Value.PublicPath;

        ManifestPath = AssetsDirectoryPath + options.Value.ManifestFile;
    }

    /// <summary>
    /// Gets a value indicating whether development mode is active.
    /// </summary>
    public bool DevelopmentMode { get; }

    /// <summary>
    /// Gets the full directory path for assets.
    /// </summary>
    public string AssetsDirectoryPath { get; }

    /// <summary>
    /// Gets the web path for UI assets.
    /// </summary>
    public string AssetsWebPath { get; }

    /// <summary>
    /// Gets the manifest file path.
    /// </summary>
    public string ManifestPath { get; }
}
