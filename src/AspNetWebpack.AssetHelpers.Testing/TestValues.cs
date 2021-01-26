// <copyright file="TestValues.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace AspNetWebpack.AssetHelpers.Testing
{
    /// <summary>
    /// Fixed values for use in testing.
    /// </summary>
    public static class TestValues
    {
        /// <summary>
        /// Fixed value for development environment.
        /// </summary>
        public const string Development = "Development";

        /// <summary>
        /// Fixed value for production environment.
        /// </summary>
        public const string Production = "Production";

        /// <summary>
        /// Web root path used in mocked WebHostEnvironment.
        /// </summary>
        public const string WebRootPath = "/Path/To/wwwroot";

        /// <summary>
        /// Assets web path used in mocked SharedSettings.
        /// </summary>
        public const string AssetsWebPath = "/Path/To/Assets/";

        /// <summary>
        /// Bundle filename used in json result from HttpMessageHandlerStub.
        /// </summary>
        public const string JsonBundle = "Bundle.js";

        /// <summary>
        /// Bundle result filename used in json result from HttpMessageHandlerStub.
        /// </summary>
        public const string JsonResultBundle = "Bundle.min.js?v=123";
    }
}
