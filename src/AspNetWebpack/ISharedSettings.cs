// <copyright file="ISharedSettings.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace AspNetWebpack
{
    /// <summary>
    /// A collection of shared settings for other services.
    /// </summary>
    public interface ISharedSettings
    {
        /// <summary>
        /// Gets a value indicating whether development mode is active.
        /// </summary>
        bool DevelopmentMode { get; }

        /// <summary>
        /// Gets the full directory path for assets.
        /// </summary>
        string AssetsDirectoryPath { get; }

        /// <summary>
        /// Gets the web path for UI assets.
        /// </summary>
        string AssetsWebPath { get; }

        /// <summary>
        /// Gets the manifest file path.
        /// </summary>
        string ManifestPath { get; }
    }
}
