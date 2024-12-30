// <copyright file="AssetService.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;

namespace AspNetWebpack;

/// <summary>
/// Service for including Webpack assets in UI projects.
/// </summary>
public sealed class AssetService : IAssetService
{
    private readonly IManifestService _manifestService;
    private readonly ITagBuilder _tagBuilder;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssetService"/> class.
    /// </summary>
    /// <param name="sharedSettings">Shared settings.</param>
    /// <param name="manifestService">Asset manifest service.</param>
    /// <param name="tagBuilder">Asset builder service.</param>
    public AssetService(ISharedSettings sharedSettings, IManifestService manifestService, ITagBuilder tagBuilder)
    {
        ArgumentNullException.ThrowIfNull(sharedSettings);

        AssetsDirectoryPath = sharedSettings.AssetsDirectoryPath;
        AssetsWebPath = sharedSettings.AssetsWebPath;

        _manifestService = manifestService;
        _tagBuilder = tagBuilder;
    }

    /// <summary>
    /// Gets full directory path for assets.
    /// </summary>
    public string AssetsDirectoryPath { get; }

    /// <summary>
    /// Gets web path for UI assets.
    /// </summary>
    public string AssetsWebPath { get; }

    /// <summary>
    /// Gets the full file path.
    /// </summary>
    /// <param name="bundle">The bundle filename.</param>
    /// <param name="fileType">The bundle file type, will append extension to bundle if specified.</param>
    /// <returns>The full file path.</returns>
    public async Task<string?> GetBundlePathAsync(string bundle, FileType? fileType = null)
    {
        if (string.IsNullOrEmpty(bundle))
        {
            return null;
        }

        if (!Path.HasExtension(bundle) && !fileType.HasValue)
        {
            throw new InvalidOperationException("A file extension is needed either in bundle name or as file type parameter.");
        }

        if (fileType.HasValue)
        {
            if (Path.HasExtension(bundle))
            {
                throw new InvalidOperationException("If bundle name already has an extension then do not specify it again as file type parameter.");
            }

            bundle = fileType switch
            {
                FileType.CSS => $"{bundle}.css",
                FileType.JS => $"{bundle}.js",
                _ => throw new InvalidEnumArgumentException(nameof(fileType), (int)fileType, typeof(FileType)),
            };
        }

        var file = await _manifestService.GetFromManifestAsync(bundle).ConfigureAwait(false);

        return file != null
            ? $"{AssetsWebPath}{file}"
            : null;
    }

    /// <summary>
    /// Gets a html script tag for the specified asset.
    /// </summary>
    /// <param name="bundle">The name of the Webpack bundle.</param>
    /// <param name="load">Enum for modifying script load behavior.</param>
    /// <returns>An HtmlString containing the html script tag.</returns>
    public async Task<HtmlString> GetScriptTagAsync(string bundle, ScriptLoad load = ScriptLoad.Normal)
    {
        return await GetScriptTagAsync(bundle, null, load).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a html script tag for the specified asset.
    /// </summary>
    /// <param name="bundle">The name of the Webpack bundle.</param>
    /// <param name="fallbackBundle">The name of the bundle to fallback to if main bundle does not exist.</param>
    /// <param name="load">Enum for modifying script load behavior.</param>
    /// <returns>An HtmlString containing the html script tag.</returns>
    public async Task<HtmlString> GetScriptTagAsync(string bundle, string? fallbackBundle, ScriptLoad load = ScriptLoad.Normal)
    {
        if (string.IsNullOrEmpty(bundle))
        {
            return HtmlString.Empty;
        }

        bundle = TryFixJsBundleName(bundle);
        var file = await _manifestService.GetFromManifestAsync(bundle).ConfigureAwait(false);

        if (file == null)
        {
            if (string.IsNullOrEmpty(fallbackBundle))
            {
                return HtmlString.Empty;
            }

            fallbackBundle = TryFixJsBundleName(fallbackBundle);
            file = await _manifestService.GetFromManifestAsync(fallbackBundle).ConfigureAwait(false);
        }

        return file != null
            ? new HtmlString(_tagBuilder.BuildScriptTag(file, load))
            : HtmlString.Empty;
    }

    /// <summary>
    /// Gets a html link tag for the specified asset.
    /// </summary>
    /// <param name="bundle">The name of the Webpack bundle.</param>
    /// <param name="fallbackBundle">The name of the bundle to fallback to if main bundle does not exist.</param>
    /// <returns>An HtmlString containing the html link tag.</returns>
    public async Task<HtmlString> GetLinkTagAsync(string bundle, string? fallbackBundle = null)
    {
        if (string.IsNullOrEmpty(bundle))
        {
            return HtmlString.Empty;
        }

        bundle = TryFixCssBundleName(bundle);
        var file = await _manifestService.GetFromManifestAsync(bundle).ConfigureAwait(false);

        if (file == null)
        {
            if (string.IsNullOrEmpty(fallbackBundle))
            {
                return HtmlString.Empty;
            }

            fallbackBundle = TryFixCssBundleName(fallbackBundle);
            file = await _manifestService.GetFromManifestAsync(fallbackBundle).ConfigureAwait(false);
        }

        return file != null
            ? new HtmlString(_tagBuilder.BuildLinkTag(file))
            : HtmlString.Empty;
    }

    /// <summary>
    /// Gets a html style tag for the specified asset.
    /// </summary>
    /// <param name="bundle">The name of the Webpack bundle.</param>
    /// <param name="fallbackBundle">The name of the bundle to fallback to if main bundle does not exist.</param>
    /// <returns>An HtmlString containing the html style tag.</returns>
    public async Task<HtmlString> GetStyleTagAsync(string bundle, string? fallbackBundle = null)
    {
        if (string.IsNullOrEmpty(bundle))
        {
            return HtmlString.Empty;
        }

        bundle = TryFixCssBundleName(bundle);
        var file = await _manifestService.GetFromManifestAsync(bundle).ConfigureAwait(false);

        if (file == null)
        {
            if (string.IsNullOrEmpty(fallbackBundle))
            {
                return HtmlString.Empty;
            }

            fallbackBundle = TryFixCssBundleName(fallbackBundle);
            file = await _manifestService.GetFromManifestAsync(fallbackBundle).ConfigureAwait(false);
        }

        return file != null
            ? new HtmlString(await _tagBuilder.BuildStyleTagAsync(file).ConfigureAwait(false))
            : HtmlString.Empty;
    }

    private static string TryFixCssBundleName(string bundle)
    {
        return TryFixBundleName(bundle, "css");
    }

    private static string TryFixJsBundleName(string bundle)
    {
        return TryFixBundleName(bundle, "js");
    }

    private static string TryFixBundleName(string bundle, string extension)
    {
        return !Path.HasExtension(bundle)
            ? $"{bundle}.{extension}"
            : bundle;
    }
}
