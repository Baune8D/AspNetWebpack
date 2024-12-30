// <copyright file="ManifestService.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspNetWebpack;

/// <summary>
/// Service for including Webpack assets in UI projects.
/// </summary>
public sealed class ManifestService : IManifestService, IDisposable
{
    private readonly ISharedSettings _sharedSettings;
    private readonly IFileSystem _fileSystem;

    private JsonDocument? _manifest;

    /// <summary>
    /// Initializes a new instance of the <see cref="ManifestService"/> class.
    /// </summary>
    /// <param name="sharedSettings">Shared settings.</param>
    /// <param name="fileSystem">File system.</param>
    public ManifestService(ISharedSettings sharedSettings, IFileSystem fileSystem)
    {
        _sharedSettings = sharedSettings;
        _fileSystem = fileSystem;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ManifestService"/> class.
    /// </summary>
    /// <param name="sharedSettings">Shared settings.</param>
    /// <param name="fileSystem">File system.</param>
    /// <param name="httpClientFactory">HttpClient factory.</param>
    public ManifestService(ISharedSettings sharedSettings, IFileSystem fileSystem, IHttpClientFactory httpClientFactory)
        : this(sharedSettings, fileSystem)
    {
        if (_sharedSettings.DevelopmentMode)
        {
            HttpClient = httpClientFactory.CreateClient();
        }
    }

    private HttpClient? HttpClient { get; }

    /// <inheritdoc />
    public void Dispose()
    {
        _manifest?.Dispose();
        HttpClient?.Dispose();
    }

    /// <summary>
    /// Gets the asset filename from the Webpack manifest.
    /// </summary>
    /// <param name="bundle">The name of the Webpack bundle.</param>
    /// <returns>The asset filename.</returns>
    public async Task<string?> GetFromManifestAsync(string bundle)
    {
        JsonDocument manifest;

        if (_manifest == null)
        {
            var json = _sharedSettings.DevelopmentMode
                ? await FetchDevelopmentManifestAsync(HttpClient, _sharedSettings.ManifestPath).ConfigureAwait(false)
                : await _fileSystem.File.ReadAllTextAsync(_sharedSettings.ManifestPath).ConfigureAwait(false);

            manifest = JsonDocument.Parse(json);
            if (!_sharedSettings.DevelopmentMode)
            {
                _manifest = manifest;
            }
        }
        else
        {
            manifest = _manifest;
        }

        try
        {
            return manifest.RootElement.GetProperty(bundle).GetString();
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }

    private static async Task<string> FetchDevelopmentManifestAsync(HttpClient? httpClient, string manifestPath)
    {
        if (httpClient == null)
        {
            throw new ArgumentNullException(nameof(httpClient), "HttpClient only available in development mode.");
        }

        try
        {
            return await httpClient.GetStringAsync(new Uri(manifestPath)).ConfigureAwait(false);
        }
        catch (HttpRequestException)
        {
            throw new InvalidOperationException("Webpack Dev Server not started!");
        }
    }
}
