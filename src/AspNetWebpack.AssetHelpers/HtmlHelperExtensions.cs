using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetWebpack.AssetHelpers
{
    /// <summary>
    /// Helper class for getting the bundle name from the view context.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Gets the bundle name from the view context.
        /// </summary>
        /// <param name="html">The html helper.</param>
        /// <returns>The bundle name.</returns>
        /// <exception cref="ArgumentNullException">If html is null.</exception>
        public static string GetBundleName(this IHtmlHelper html)
        {
            if (html == null)
            {
                throw new ArgumentNullException(nameof(html));
            }

            var path = html.ViewContext.View.Path;
            var viewPaths = path
                .Replace(".cshtml", string.Empty, StringComparison.Ordinal)
                .Split('/')
                .ToList();
            viewPaths.Remove(string.Empty);
            viewPaths.Remove("Areas");
            viewPaths.Remove("Views");
            viewPaths.Remove("Pages");
            return string.Join("_", viewPaths);
        }
    }
}
