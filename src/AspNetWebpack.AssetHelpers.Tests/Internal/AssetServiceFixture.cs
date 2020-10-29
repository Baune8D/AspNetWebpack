using System;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace AspNetWebpack.AssetHelpers.Tests.Internal
{
    [UsedImplicitly]
    public class AssetServiceFixture
    {
        private readonly Mock<AssetService> _assetServiceMock;

        public AssetServiceFixture()
        {
            var options = Options.Create(new WebpackOptions
            {
                PublicDevServer = "https://public.dev",
                InternalDevServer = "https://internal.dev",
            });

            var env = new Mock<IWebHostEnvironment>();
            var httpClientFactory = new Mock<IHttpClientFactory>();

            _assetServiceMock = new Mock<AssetService>(env.Object, options, httpClientFactory.Object)
            {
                CallBase = true,
            };
        }

        public IAssetService AssetService => _assetServiceMock.Object;

        public string GetBundlePath(string resultBundle)
        {
            return $"{AssetService.AssetPath}{resultBundle}";
        }

        public void SetupGetFromManifestAsync(string bundle, string? resultBundle)
        {
            _assetServiceMock
                .Protected()
                .Setup<Task<string?>>("GetFromManifestAsync", bundle)
                .ReturnsAsync(resultBundle);
        }

        public void SetupGetScriptTag(string resultBundle, ScriptLoad scriptLoad = ScriptLoad.Normal)
        {
            string result = scriptLoad switch
            {
                ScriptLoad.Normal => $"<script src=\"{GetBundlePath(resultBundle)}\"></script>",
                ScriptLoad.Async => $"<script src=\"{GetBundlePath(resultBundle)}\" async></script>",
                ScriptLoad.Defer => $"<script src=\"{GetBundlePath(resultBundle)}\" defer></script>",
                ScriptLoad.AsyncDefer => $"<script src=\"{GetBundlePath(resultBundle)}\" async defer></script>",
                _ => throw new ArgumentOutOfRangeException(nameof(scriptLoad), scriptLoad, null),
            };

            _assetServiceMock
                .Protected()
                .Setup<string>("GetScriptTag", resultBundle, scriptLoad)
                .Returns(result);
        }

        public void SetupGetStyleTagAsync(string resultBundle, StyleLoad styleLoad = StyleLoad.Normal)
        {
            string result = styleLoad switch
            {
                StyleLoad.Normal => $"<link href=\"{GetBundlePath(resultBundle)}\" />",
                StyleLoad.Inline => "<style>Test</style>",
                _ => throw new ArgumentOutOfRangeException(nameof(styleLoad), styleLoad, null),
            };

            _assetServiceMock
                .Protected()
                .Setup<Task<string>>("GetStyleTagAsync", resultBundle, styleLoad)
                .ReturnsAsync(result);
        }

        public void VerifyGetBundleAsync(string bundle)
        {
            _assetServiceMock.Verify(x => x.GetBundleAsync(bundle));
        }

        public void VerifyGetScriptAsync(string bundle, ScriptLoad scriptLoad = ScriptLoad.Normal)
        {
            _assetServiceMock.Verify(x => x.GetScriptAsync(bundle, scriptLoad));
        }

        public void VerifyGetStyleAsync(string bundle, StyleLoad styleLoad = StyleLoad.Normal)
        {
            _assetServiceMock.Verify(x => x.GetStyleAsync(bundle, styleLoad));
        }

        public void VerifyGetFromManifestAsync(string bundle)
        {
            _assetServiceMock
                .Protected()
                .Verify("GetFromManifestAsync", Times.Once(), bundle);
        }

        public void VerifyGetScriptTag(string resultBundle, ScriptLoad scriptLoad = ScriptLoad.Normal)
        {
            _assetServiceMock
                .Protected()
                .Verify("GetScriptTag", Times.Once(), resultBundle, scriptLoad);
        }

        public void VerifyGetStyleTagAsync(string resultBundle, StyleLoad styleLoad = StyleLoad.Normal)
        {
            _assetServiceMock
                .Protected()
                .Verify("GetStyleTagAsync", Times.Once(), resultBundle, styleLoad);
        }

        public void VerifyNoOtherCalls()
        {
            _assetServiceMock.VerifyNoOtherCalls();
        }
    }
}
