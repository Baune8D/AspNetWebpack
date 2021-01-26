// <copyright file="IAssetService.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;

namespace AspNetWebpack.AssetHelpers
{
    /// <summary>
    /// Service for including Webpack assets in UI projects.
    /// </summary>
    public interface IAssetService
    {
        /// <summary>
        /// Gets full directory path for assets.
        /// </summary>
        string AssetsDirectoryPath { get; }

        /// <summary>
        /// Gets web path for UI assets.
        /// </summary>
        string AssetsWebPath { get; }

        /// <summary>
        /// Gets the full file path.
        /// </summary>
        /// <param name="bundle">The bundle filename.</param>
        /// <param name="fileType">The bundle file type, will append extension to bundle if specified.</param>
        /// <returns>The full file path.</returns>
        Task<string?> GetBundlePathAsync(string bundle, FileType? fileType = null);

        /// <summary>
        /// Gets a html script tag for the specified asset.
        /// </summary>
        /// <param name="bundle">The name of the Webpack bundle.</param>
        /// <param name="load">Enum for modifying script load behavior.</param>
        /// <returns>An HtmlString containing the html script tag.</returns>
        Task<HtmlString> GetScriptTagAsync(string bundle, ScriptLoad load = ScriptLoad.Normal);

        /// <summary>
        /// Gets a html script tag for the specified asset.
        /// </summary>
        /// <param name="bundle">The name of the Webpack bundle.</param>
        /// <param name="fallbackBundle">The name of the bundle to fallback to if main bundle does not exist.</param>
        /// <param name="load">Enum for modifying script load behavior.</param>
        /// <returns>An HtmlString containing the html script tag.</returns>
        Task<HtmlString> GetScriptTagAsync(string bundle, string? fallbackBundle, ScriptLoad load = ScriptLoad.Normal);

        /// <summary>
        /// Gets a html link tag for the specified asset.
        /// </summary>
        /// <param name="bundle">The name of the Webpack bundle.</param>
        /// <param name="fallbackBundle">The name of the bundle to fallback to if main bundle does not exist.</param>
        /// <returns>An HtmlString containing the html link tag.</returns>
        Task<HtmlString> GetLinkTagAsync(string bundle, string? fallbackBundle = null);

        /// <summary>
        /// Gets a html style tag for the specified asset.
        /// </summary>
        /// <param name="bundle">The name of the Webpack bundle.</param>
        /// <param name="fallbackBundle">The name of the bundle to fallback to if main bundle does not exist.</param>
        /// <returns>An HtmlString containing the html style tag.</returns>
        Task<HtmlString> GetStyleTagAsync(string bundle, string? fallbackBundle = null);
    }
}
