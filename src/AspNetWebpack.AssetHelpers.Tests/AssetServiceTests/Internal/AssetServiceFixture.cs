using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace AspNetWebpack.AssetHelpers.Tests.AssetServiceTests.Internal
{
    public abstract class AssetServiceFixture
    {
        protected AssetServiceFixture(string bundle, string existingBundle)
        {
            Bundle = bundle;
            ExistingBundle = existingBundle;

            var options = Options.Create(new WebpackOptions
            {
                PublicDevServer = "https://public.dev",
                InternalDevServer = "https://internal.dev",
            });

            var env = new Mock<IWebHostEnvironment>();
            var httpClientFactory = new Mock<IHttpClientFactory>();

            AssetServiceMock = new Mock<AssetService>(env.Object, options, httpClientFactory.Object)
            {
                CallBase = true,
            };
        }

        protected IAssetService AssetService => AssetServiceMock.Object;

        protected Mock<AssetService> AssetServiceMock { get; }

        protected string ResultBundle => $"{ExistingBundle}?v=1234";

        protected string ResultBundlePath => $"{AssetService.AssetPath}{ResultBundle}";

        protected string Bundle { get; }

        private string ExistingBundle { get; }

        public void VerifyGetFromManifest()
        {
            AssetServiceMock
                .Protected()
                .Verify("GetFromManifestAsync", Times.Once(), Bundle);
        }

        public void VerifyNoOtherCalls()
        {
            AssetServiceMock.VerifyNoOtherCalls();
        }

        protected void SetupGetFromManifest()
        {
            AssetServiceMock
                .Protected()
                .Setup<Task<string?>>("GetFromManifestAsync", ItExpr.IsAny<string>())
                .ReturnsAsync((string?)null);

            AssetServiceMock
                .Protected()
                .Setup<Task<string?>>("GetFromManifestAsync", ExistingBundle)
                .ReturnsAsync(ResultBundle);
        }
    }
}
