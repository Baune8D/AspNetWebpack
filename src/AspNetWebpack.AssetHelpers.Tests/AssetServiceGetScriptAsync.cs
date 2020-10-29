using System.Threading.Tasks;
using AspNetWebpack.AssetHelpers.Tests.Internal;
using Microsoft.AspNetCore.Html;
using Shouldly;
using Xunit;

namespace AspNetWebpack.AssetHelpers.Tests
{
    public class AssetServiceGetScriptAsync
    {
        private readonly AssetServiceFixture _fixture;

        public AssetServiceGetScriptAsync()
        {
            _fixture = new AssetServiceFixture();
        }

        [Fact]
        public async Task GetScriptAsync_EmptyString_ShouldReturnEmptyHtmlString()
        {
            // Arrange
            const string bundle = "";
            _fixture.SetupGetFromManifestAsync(bundle, null);

            // Act
            var result = await _fixture.AssetService.GetScriptAsync(bundle);

            // Assert
            result.ShouldBe(HtmlString.Empty);
            _fixture.VerifyGetScriptAsync(bundle);
            _fixture.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetScriptAsync_NonExistingBundle_ShouldReturnEmptyHtmlString()
        {
            // Arrange
            const string bundle = "NonExistingBundle.js";
            _fixture.SetupGetFromManifestAsync(bundle, null);

            // Act
            var result = await _fixture.AssetService.GetScriptAsync(bundle);

            // Assert
            result.ShouldBe(HtmlString.Empty);
            _fixture.VerifyGetScriptAsync(bundle);
            _fixture.VerifyGetFromManifestAsync(bundle);
            _fixture.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetScriptAsync_ExistingBundle_ShouldReturnScriptTag()
        {
            // Arrange
            const string bundle = "ExistingBundle.js";
            var resultBundle = $"{bundle}?v=1234";
            _fixture.SetupGetFromManifestAsync(bundle, resultBundle);
            _fixture.SetupGetScriptTag(resultBundle);

            // Act
            var result = await _fixture.AssetService.GetScriptAsync(bundle);

            // Assert
            result.ShouldBeScriptTag(_fixture.GetBundlePath(resultBundle));
            _fixture.VerifyGetScriptAsync(bundle);
            _fixture.VerifyGetFromManifestAsync(bundle);
            _fixture.VerifyGetScriptTag(resultBundle);
            _fixture.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetScriptAsync_ExistingBundleAsync_ShouldReturnScriptTag()
        {
            // Arrange
            const ScriptLoad scriptLoad = ScriptLoad.Async;
            const string bundle = "ExistingBundle.js";
            var resultBundle = $"{bundle}?v=1234";
            _fixture.SetupGetFromManifestAsync(bundle, resultBundle);
            _fixture.SetupGetScriptTag(resultBundle);

            // Act
            var result = await _fixture.AssetService.GetScriptAsync(bundle, scriptLoad);

            // Assert
            result.ShouldBeScriptTag(_fixture.GetBundlePath(resultBundle), scriptLoad);
            _fixture.VerifyGetScriptAsync(bundle, scriptLoad);
            _fixture.VerifyGetFromManifestAsync(bundle);
            _fixture.VerifyGetScriptTag(resultBundle, scriptLoad);
            _fixture.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetScriptAsync_ExistingBundleDefer_ShouldReturnScriptTag()
        {
            // Arrange
            const ScriptLoad scriptLoad = ScriptLoad.Defer;
            const string bundle = "ExistingBundle.js";
            var resultBundle = $"{bundle}?v=1234";
            _fixture.SetupGetFromManifestAsync(bundle, resultBundle);
            _fixture.SetupGetScriptTag(resultBundle);

            // Act
            var result = await _fixture.AssetService.GetScriptAsync(bundle, scriptLoad);

            // Assert
            result.ShouldBeScriptTag(_fixture.GetBundlePath(resultBundle), scriptLoad);
            _fixture.VerifyGetScriptAsync(bundle, scriptLoad);
            _fixture.VerifyGetFromManifestAsync(bundle);
            _fixture.VerifyGetScriptTag(resultBundle, scriptLoad);
            _fixture.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetScriptAsync_ExistingBundleAsyncDefer_ShouldReturnScriptTag()
        {
            // Arrange
            const ScriptLoad scriptLoad = ScriptLoad.AsyncDefer;
            const string bundle = "ExistingBundle.js";
            var resultBundle = $"{bundle}?v=1234";
            _fixture.SetupGetFromManifestAsync(bundle, resultBundle);
            _fixture.SetupGetScriptTag(resultBundle);

            // Act
            var result = await _fixture.AssetService.GetScriptAsync(bundle, scriptLoad);

            // Assert
            result.ShouldBeScriptTag(_fixture.GetBundlePath(resultBundle), scriptLoad);
            _fixture.VerifyGetScriptAsync(bundle, scriptLoad);
            _fixture.VerifyGetFromManifestAsync(bundle);
            _fixture.VerifyGetScriptTag(resultBundle, scriptLoad);
            _fixture.VerifyNoOtherCalls();
        }
    }
}
