using System.Threading.Tasks;
using AspNetWebpack.AssetHelpers.Tests.Internal;
using Shouldly;
using Xunit;

namespace AspNetWebpack.AssetHelpers.Tests
{
    public class AssetServiceGetBundleAsync
    {
        private readonly AssetServiceFixture _fixture;

        public AssetServiceGetBundleAsync()
        {
            _fixture = new AssetServiceFixture();
        }

        [Fact]
        public async Task GetBundleAsync_EmptyString_ShouldReturnEmptyHtmlString()
        {
            // Arrange
            const string bundle = "";

            // Act
            var result = await _fixture.AssetService.GetBundleAsync(bundle);

            // Assert
            result.ShouldBeNull();
            _fixture.VerifyGetBundleAsync(bundle);
            _fixture.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetBundleAsync_NonExistingBundle_ShouldReturnEmptyHtmlString()
        {
            // Arrange
            const string bundle = "NonExistingBundle.js";
            _fixture.SetupGetFromManifestAsync(bundle, null);

            // Act
            var result = await _fixture.AssetService.GetBundleAsync(bundle);

            // Assert
            result.ShouldBeNull();
            _fixture.VerifyGetBundleAsync(bundle);
            _fixture.VerifyGetFromManifestAsync(bundle);
            _fixture.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetBundleAsync_ExistingBundle_ShouldReturnScriptTag()
        {
            // Arrange
            const string bundle = "ExistingBundle.js";
            var resultBundle = $"{bundle}?v=1234";
            _fixture.SetupGetFromManifestAsync(bundle, resultBundle);

            // Act
            var result = await _fixture.AssetService.GetBundleAsync(bundle);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBe(_fixture.GetBundlePath(resultBundle));
            _fixture.VerifyGetBundleAsync(bundle);
            _fixture.VerifyGetFromManifestAsync(bundle);
            _fixture.VerifyNoOtherCalls();
        }
    }
}
