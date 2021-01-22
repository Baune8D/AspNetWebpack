// <copyright file="ConstructorFixture.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Moq;

namespace AspNetWebpack.AssetHelpers.Testing
{
    /// <summary>
    /// Fixture for testing AssetService constructor.
    /// </summary>
    public sealed class ConstructorFixture : IDisposable
    {
        private readonly IOptions<WebpackOptions> _options;
        private readonly Mock<IWebHostEnvironment> _webHostEnvironment;
        private readonly Mock<IHttpClientFactory> _httpClientFactory;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorFixture"/> class.
        /// </summary>
        /// <param name="environment">Environment name.</param>
        public ConstructorFixture(string environment)
        {
            _options = Options.Create(new WebpackOptions
            {
                PublicDevServer = "https://public.dev",
                InternalDevServer = "https://internal.dev",
            });

            _webHostEnvironment = new Mock<IWebHostEnvironment>();
            _httpClientFactory = new Mock<IHttpClientFactory>();
            _httpClient = new HttpClient();

            _webHostEnvironment
                .SetupGet(x => x.EnvironmentName)
                .Returns(environment);

            _webHostEnvironment
                .SetupGet(x => x.WebRootPath)
                .Returns("wwwroot");

            _httpClientFactory
                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(_httpClient);
        }

        /// <summary>
        /// Gets the true WebHostEnvironment object.
        /// </summary>
        public IWebHostEnvironment WebHostEnvironment => _webHostEnvironment.Object;

        /// <summary>
        /// Gets the Webpack options.
        /// </summary>
        public WebpackOptions WebpackOptions => _options.Value;

        /// <inheritdoc />
        public void Dispose()
        {
            _httpClient.Dispose();
        }

        /// <summary>
        /// Creates a testable constructor.
        /// </summary>
        /// <returns>Testable constructor.</returns>
        public TestableConstructor Construct()
        {
            return new(_webHostEnvironment.Object, _options, _httpClientFactory.Object);
        }
    }
}
