// <copyright file="TestableConstructor.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace AspNetWebpack.AssetHelpers.Testing
{
    /// <summary>
    /// Version of AssetService with a testable constructor.
    /// </summary>
    public sealed class TestableConstructor : AssetService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestableConstructor"/> class.
        /// </summary>
        /// <param name="env">Web host environment.</param>
        /// <param name="options">Webpack options.</param>
        /// <param name="httpClientFactory">HttpClient factory.</param>
        public TestableConstructor(
            IWebHostEnvironment env,
            IOptions<WebpackOptions> options,
            IHttpClientFactory httpClientFactory)
            : base(env, options, httpClientFactory)
        {
        }

        /// <summary>
        /// Gets internal HttpClient.
        /// </summary>
        public new HttpClient? HttpClient => base.HttpClient;

        /// <summary>
        /// Gets a value indicating whether development mode is used.
        /// </summary>
        public new bool DevelopmentMode => base.DevelopmentMode;

        /// <summary>
        /// Gets internal AssetBaseFilePath.
        /// </summary>
        public new string AssetBaseFilePath => base.AssetBaseFilePath;

        /// <summary>
        /// Gets internal ManifestPath.
        /// </summary>
        public new string ManifestPath => base.ManifestPath;
    }
}
