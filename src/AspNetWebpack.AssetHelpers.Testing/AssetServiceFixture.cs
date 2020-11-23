using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace AspNetWebpack.AssetHelpers.Testing
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

        protected AssetServiceFixture(string bundle, string existingBundle, string fallbackBundle)
            : this(bundle, existingBundle)
        {
            FallbackBundle = fallbackBundle;
        }

        protected IAssetService AssetService => AssetServiceMock.Object;

        protected Mock<AssetService> AssetServiceMock { get; }

        protected string ExistingResultBundle => $"{ExistingBundle}?v=1234";

        protected string ExistingResultBundlePath => $"{AssetService.AssetPath}{ExistingResultBundle}";

        protected string FallbackResultBundle => $"{FallbackBundle}?v=1234";

        protected string FallbackResultBundlePath => $"{AssetService.AssetPath}{FallbackResultBundle}";

        protected string Bundle { get; }

        private string ExistingBundle { get; }

        private string? FallbackBundle { get; }

        protected void VerifyGetFromManifest(string bundle)
        {
            AssetServiceMock
                .Protected()
                .Verify("GetFromManifestAsync", Times.Once(), bundle);
        }

        protected void VerifyNoOtherCalls()
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
                .ReturnsAsync(ExistingResultBundle);

            if (FallbackBundle != null)
            {
                AssetServiceMock
                    .Protected()
                    .Setup<Task<string?>>("GetFromManifestAsync", FallbackBundle)
                    .ReturnsAsync(FallbackResultBundle);
            }
        }
    }
}
