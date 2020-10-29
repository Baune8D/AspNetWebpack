using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace AspNetWebpack.AssetHelpers
{
    /// <summary>
    /// Service for including Webpack assets in UI projects.
    /// </summary>
    public class AssetService : IAssetService
    {
        private readonly Dictionary<string, string> _inlineStyles = new Dictionary<string, string>();

        private readonly HttpClient? _httpClient;
        private readonly bool _developmentMode;
        private readonly string _assetBaseFilePath;
        private readonly string _manifestPath;

        private JsonDocument? _manifest;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetService"/> class.
        /// </summary>
        /// <param name="env">Web host environment.</param>
        /// <param name="options">Webpack options.</param>
        /// <param name="httpClientFactory">HttpClient factory.</param>
        /// <exception cref="ArgumentNullException">If Webpack options is null.</exception>
        public AssetService(IWebHostEnvironment env, IOptions<WebpackOptions> options, IHttpClientFactory httpClientFactory)
        {
            if (env == null)
            {
                throw new ArgumentNullException(nameof(env));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _developmentMode = env.IsDevelopment();

            if (_developmentMode)
            {
                _httpClient = httpClientFactory.CreateClient();
            }

            _manifestPath = _assetBaseFilePath + options.Value.ManifestFile;

            switch (env.EnvironmentName)
            {
                case "Development":
                    _assetBaseFilePath = options.Value.InternalDevServer + options.Value.AssetsPublicPath;
                    break;
                default:
                    _assetBaseFilePath = env.WebRootPath + options.Value.AssetsPublicPath;
                    break;
            }

            AssetPath = _developmentMode
                ? options.Value.PublicDevServer + options.Value.AssetsPublicPath
                : options.Value.AssetsPublicPath;
        }

        /// <summary>
        /// Gets web path for UI assets.
        /// </summary>
        public string AssetPath { get; }

        /// <summary>
        /// Gets the full file path.
        /// </summary>
        /// <param name="resource">The bundle filename.</param>
        /// <returns>The full file path.</returns>
        public virtual async Task<string?> GetBundleAsync(string resource)
        {
            if (string.IsNullOrEmpty(resource))
            {
                return null;
            }

            var file = await GetFromManifestAsync(resource).ConfigureAwait(false);

            return file != null
                ? $"{AssetPath}{file}"
                : null;
        }

        /// <summary>
        /// Gets a html script tag for the specified asset.
        /// </summary>
        /// <param name="resource">The name of the Webpack bundle.</param>
        /// <param name="load">Enum for modifying script load behavior.</param>
        /// <returns>An HtmlString containing the html script tag.</returns>
        public virtual async Task<HtmlString> GetScriptAsync(string resource, ScriptLoad load = ScriptLoad.Normal)
        {
            if (string.IsNullOrEmpty(resource))
            {
                return HtmlString.Empty;
            }

            if (!Path.HasExtension(resource))
            {
                resource += ".js";
            }

            var file = await GetFromManifestAsync(resource).ConfigureAwait(false);

            return file != null
                ? new HtmlString(GetScriptTag(file, load))
                : HtmlString.Empty;
        }

        /// <summary>
        /// Gets a html style tag for the specified asset.
        /// </summary>
        /// <param name="resource">The name of the Webpack bundle.</param>
        /// <param name="load">Enum for modifying style load behavior.</param>
        /// <returns>An HtmlString containing the html style tag.</returns>
        public virtual async Task<HtmlString> GetStyleAsync(string resource, StyleLoad load = StyleLoad.Normal)
        {
            if (string.IsNullOrEmpty(resource))
            {
                return HtmlString.Empty;
            }

            if (!Path.HasExtension(resource))
            {
                resource += ".css";
            }

            var file = await GetFromManifestAsync(resource).ConfigureAwait(false);

            return file != null
                ? new HtmlString(await GetStyleTagAsync(file, load).ConfigureAwait(false))
                : HtmlString.Empty;
        }

        /// <summary>
        /// Gets the asset filename from the Webpack manifest.
        /// </summary>
        /// <param name="resource">The name of the Webpack bundle.</param>
        /// <returns>The asset filename.</returns>
        protected virtual async Task<string?> GetFromManifestAsync(string resource)
        {
            JsonDocument manifest;

            if (_manifest == null)
            {
                var json = _developmentMode
                    ? await GetDevelopmentManifestAsync(_httpClient, _manifestPath).ConfigureAwait(false)
                    : await File.ReadAllTextAsync(_manifestPath).ConfigureAwait(false);

                manifest = JsonDocument.Parse(json);
                if (!_developmentMode)
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
                return manifest.RootElement.GetProperty(resource).GetString();
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Builds the script tag.
        /// </summary>
        /// <param name="file">The JS file to use in the tag.</param>
        /// <param name="load">Enum for modifying script load behavior.</param>
        /// <returns>A string containing the script tag.</returns>
        protected virtual string GetScriptTag(string file, ScriptLoad load)
        {
            var crossOrigin = string.Empty;
            if (_developmentMode)
            {
                crossOrigin = "crossorigin=\"anonymous\"";
            }

            var loadType = _developmentMode ? " " : string.Empty;
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
                    throw new ArgumentOutOfRangeException(nameof(load), load, null);
            }

            return $"<script src=\"{AssetPath}{file}\" {crossOrigin}{loadType}></script>";
        }

        /// <summary>
        /// Builds the link/style tag.
        /// </summary>
        /// <param name="file">The CSS file to use in the tag.</param>
        /// <param name="load">Enum for modifying style load behavior.</param>
        /// <returns>A string containing the link/style tag.</returns>
        protected virtual async Task<string> GetStyleTagAsync(string file, StyleLoad load)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (load != StyleLoad.Inline)
            {
                return $"<link href=\"{AssetPath}{file}\" rel=\"stylesheet\" />";
            }

            if (!_developmentMode && _inlineStyles.ContainsKey(file))
            {
                return _inlineStyles[file];
            }

            var filename = file;
            var queryIndex = filename.IndexOf('?', StringComparison.Ordinal);
            if (queryIndex != -1)
            {
                filename = filename.Substring(0, queryIndex);
            }

            var fullPath = $"{_assetBaseFilePath}{filename}";

            var style = _developmentMode
                ? await GetDevelopmentStyleAsync(_httpClient, fullPath).ConfigureAwait(false)
                : await File.ReadAllTextAsync(fullPath).ConfigureAwait(false);

            var result = $"<style>{style}</style>";

            if (!_developmentMode)
            {
                _inlineStyles.Add(file, result);
            }

            return result;
        }

        /// <summary>
        /// Fetch the manifest from dev server.
        /// </summary>
        /// <param name="httpClient">The HttpClient.</param>
        /// <param name="manifestPath">Path for the manifest file.</param>
        /// <returns>The manifest content.</returns>
        /// <exception cref="ArgumentNullException">If HttpClient is null.</exception>
        /// <exception cref="Exception">If dev server cannot be reached.</exception>
        // ReSharper disable once MemberCanBeMadeStatic.Global
        protected virtual async Task<string> GetDevelopmentManifestAsync(HttpClient? httpClient, string manifestPath)
        {
            if (httpClient == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            try
            {
                return await httpClient.GetStringAsync(new Uri(manifestPath)).ConfigureAwait(false);
            }
            catch (HttpRequestException)
            {
                throw new Exception("Webpack Dev Server not started!");
            }
        }

        /// <summary>
        /// Fetch the CSS file content from dev server.
        /// </summary>
        /// <param name="httpClient">The HttpClient.</param>
        /// <param name="fullPath">Path to CSS file.</param>
        /// <returns>The CSS file content.</returns>
        /// <exception cref="ArgumentNullException">If HttpClient is null.</exception>
        // ReSharper disable once MemberCanBeMadeStatic.Global
        protected virtual async Task<string> GetDevelopmentStyleAsync(HttpClient? httpClient, string fullPath)
        {
            if (httpClient == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            return await httpClient.GetStringAsync(new Uri(fullPath)).ConfigureAwait(false);
        }
    }
}
