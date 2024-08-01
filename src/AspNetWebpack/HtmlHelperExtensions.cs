// <copyright file="HtmlHelperExtensions.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetWebpack
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
        public static string GetBundleName(this IHtmlHelper html)
        {
            ArgumentNullException.ThrowIfNull(html);

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
