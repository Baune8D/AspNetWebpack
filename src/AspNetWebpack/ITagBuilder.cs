// <copyright file="ITagBuilder.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System.Threading.Tasks;

namespace AspNetWebpack
{
    /// <summary>
    /// Service for including Webpack assets in UI projects.
    /// </summary>
    public interface ITagBuilder
    {
        /// <summary>
        /// Builds the script tag.
        /// </summary>
        /// <param name="file">The JS file to use in the tag.</param>
        /// <param name="load">Enum for modifying script load behavior.</param>
        /// <returns>A string containing the script tag.</returns>
        string BuildScriptTag(string file, ScriptLoad load);

        /// <summary>
        /// Builds the link/style tag.
        /// </summary>
        /// <param name="file">The CSS file to use in the tag.</param>
        /// <returns>A string containing the link/style tag.</returns>
        string BuildLinkTag(string file);

        /// <summary>
        /// Builds the link/style tag.
        /// </summary>
        /// <param name="file">The CSS file to use in the tag.</param>
        /// <returns>A string containing the link/style tag.</returns>
        Task<string> BuildStyleTagAsync(string file);
    }
}
