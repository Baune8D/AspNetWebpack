// <copyright file="IManifestService.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System.Threading.Tasks;

namespace AspNetWebpack
{
    /// <summary>
    /// Service for including Webpack assets in UI projects.
    /// </summary>
    public interface IManifestService
    {
        /// <summary>
        /// Gets the asset filename from the Webpack manifest.
        /// </summary>
        /// <param name="bundle">The name of the Webpack bundle.</param>
        /// <returns>The asset filename.</returns>
        Task<string?> GetFromManifestAsync(string bundle);
    }
}
