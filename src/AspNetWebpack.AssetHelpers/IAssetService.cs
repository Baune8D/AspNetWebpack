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
        /// Gets web path for UI assets.
        /// </summary>
        string AssetPath { get; }

        /// <summary>
        /// Gets the full file path.
        /// </summary>
        /// <param name="resource">The bundle filename.</param>
        /// <returns>The full file path.</returns>
        Task<string?> GetBundleAsync(string resource);

        /// <summary>
        /// Gets a html script tag for the specified asset.
        /// </summary>
        /// <param name="resource">The name of the Webpack bundle.</param>
        /// <param name="load">Enum for modifying script load behavior.</param>
        /// <returns>An HtmlString containing the html script tag.</returns>
        Task<HtmlString> GetScriptAsync(string resource, ScriptLoad load = ScriptLoad.Normal);

        /// <summary>
        /// Gets a html style tag for the specified asset.
        /// </summary>
        /// <param name="resource">The name of the Webpack bundle.</param>
        /// <param name="load">Enum for modifying style load behavior.</param>
        /// <returns>An HtmlString containing the html style tag.</returns>
        Task<HtmlString> GetStyleAsync(string resource, StyleLoad load = StyleLoad.Normal);
    }
}
