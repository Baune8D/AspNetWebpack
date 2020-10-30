using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Moq;

namespace AspNetWebpack.AssetHelpers.Tests.AssetServiceTests.Internal
{
    public sealed class ConstructorFixture : IDisposable
    {
        private readonly IOptions<WebpackOptions> _options;
        private readonly Mock<IWebHostEnvironment> _webHostEnvironment;
        private readonly Mock<IHttpClientFactory> _httpClientFactory;
        private readonly HttpClient _httpClient;

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

        public IWebHostEnvironment WebHostEnvironment => _webHostEnvironment.Object;

        public WebpackOptions WebpackOptions => _options.Value;

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        public TestableConstructor Construct()
        {
            return new TestableConstructor(_webHostEnvironment.Object, _options, _httpClientFactory.Object);
        }
    }
}
