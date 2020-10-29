using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace AspNetWebpack.AssetHelpers
{
    /// <summary>
    /// Extensions for getting the bundle name from view data.
    /// </summary>
    public static class ViewDataExtensions
    {
        /// <summary>
        /// Check the ViewData for a Webpack bundle name.
        /// </summary>
        /// <param name="viewData">The view data.</param>
        /// <returns>The name of the Webpack bundle.</returns>
        public static string? GetBundleName(this ViewDataDictionary viewData)
        {
            if (viewData == null)
            {
                throw new ArgumentNullException(nameof(viewData));
            }

            if (!viewData.ContainsKey("Bundle") || !(viewData["Bundle"] is string))
            {
                return null;
            }

            var bundle = (string)viewData["Bundle"];
            if (!bundle.StartsWith('/'))
            {
                return bundle;
            }

            // Use Razor Page logic to resolve bundle. E.g. /Some/Bundle = Some_Bundle
            var viewPaths = bundle
                .Split('/')
                .ToList();
            viewPaths.Remove(string.Empty);
            return string.Join("_", viewPaths);
        }
    }
}
