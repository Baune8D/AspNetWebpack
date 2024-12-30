// <copyright file="ViewDataExtensions.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace AspNetWebpack;

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
        ArgumentNullException.ThrowIfNull(viewData);

        if (!viewData.ContainsKey("Bundle") || viewData["Bundle"] is not string)
        {
            return null;
        }

        var bundle = (string)viewData["Bundle"]!;
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
