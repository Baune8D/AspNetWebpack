// <copyright file="TagBuilder.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Abstractions;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspNetWebpack
{
    /// <summary>
    /// Service for including Webpack assets in UI projects.
    /// </summary>
    public sealed class TagBuilder : ITagBuilder, IDisposable
    {
        private readonly Dictionary<string, string> _inlineStyles = new();
        private readonly ISharedSettings _sharedSettings;
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagBuilder"/> class.
        /// </summary>
        /// <param name="sharedSettings">Shared settings.</param>
        /// <param name="fileSystem">File system.</param>
        public TagBuilder(ISharedSettings sharedSettings, IFileSystem fileSystem)
        {
            _sharedSettings = sharedSettings;
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TagBuilder"/> class.
        /// </summary>
        /// <param name="sharedSettings">Shared settings.</param>
        /// <param name="fileSystem">File system.</param>
        /// <param name="httpClientFactory">HttpClient factory.</param>
        public TagBuilder(ISharedSettings sharedSettings, IFileSystem fileSystem, IHttpClientFactory httpClientFactory)
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
            HttpClient?.Dispose();
        }

        /// <summary>
        /// Builds the script tag.
        /// </summary>
        /// <param name="file">The JS file to use in the tag.</param>
        /// <param name="load">Enum for modifying script load behavior.</param>
        /// <returns>A string containing the script tag.</returns>
        public string BuildScriptTag(string file, ScriptLoad load)
        {
            var crossOrigin = string.Empty;
            if (_sharedSettings.DevelopmentMode)
            {
                crossOrigin = "crossorigin=\"anonymous\"";
            }

            var loadType = _sharedSettings.DevelopmentMode ? " " : string.Empty;
            switch (load)
            {
                case ScriptLoad.Normal:
                    break;
                case ScriptLoad.Async:
                    loadType += "async";
                    break;
                case ScriptLoad.Defer:
                    loadType += "defer";
                    break;
                case ScriptLoad.AsyncDefer:
                    loadType += "async defer";
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(load), (int)load, typeof(ScriptLoad));
            }

            return $"<script src=\"{_sharedSettings.AssetsWebPath}{file}\" {crossOrigin}{loadType}></script>";
        }

        /// <summary>
        /// Builds the link/style tag.
        /// </summary>
        /// <param name="file">The CSS file to use in the tag.</param>
        /// <returns>A string containing the link/style tag.</returns>
        public string BuildLinkTag(string file)
        {
            return $"<link href=\"{_sharedSettings.AssetsWebPath}{file}\" rel=\"stylesheet\" />";
        }

        /// <summary>
        /// Builds the link/style tag.
        /// </summary>
        /// <param name="file">The CSS file to use in the tag.</param>
        /// <returns>A string containing the link/style tag.</returns>
        public async Task<string> BuildStyleTagAsync(string file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (!_sharedSettings.DevelopmentMode && _inlineStyles.ContainsKey(file))
            {
                return _inlineStyles[file];
            }

            var filename = file;
            var queryIndex = filename.IndexOf('?', StringComparison.Ordinal);
            if (queryIndex != -1)
            {
                filename = filename.Substring(0, queryIndex);
            }

            var fullPath = $"{_sharedSettings.AssetsDirectoryPath}{filename}";

            var style = _sharedSettings.DevelopmentMode
                ? await FetchDevelopmentStyleAsync(HttpClient, fullPath).ConfigureAwait(false)
                : await _fileSystem.File.ReadAllTextAsync(fullPath).ConfigureAwait(false);

            var result = $"<style>{style}</style>";

            if (!_sharedSettings.DevelopmentMode)
            {
                _inlineStyles.Add(file, result);
            }

            return result;
        }

        private static async Task<string> FetchDevelopmentStyleAsync(HttpClient? httpClient, string fullPath)
        {
            if (httpClient == null)
            {
                throw new ArgumentNullException(nameof(httpClient), "HttpClient only available in development mode.");
            }

            return await httpClient.GetStringAsync(new Uri(fullPath)).ConfigureAwait(false);
        }
    }
}
