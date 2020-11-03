using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace AspNetWebpack.AssetHelpers.Testing
{
    public sealed class TestableConstructor : AssetService
    {
        public TestableConstructor(
            IWebHostEnvironment env,
            IOptions<WebpackOptions> options,
            IHttpClientFactory httpClientFactory)
            : base(env, options, httpClientFactory)
        {
        }

        public new HttpClient? HttpClient => base.HttpClient;

        public new bool DevelopmentMode => base.DevelopmentMode;

        public new string AssetBaseFilePath => base.AssetBaseFilePath;

        public new string ManifestPath => base.ManifestPath;
    }
}
