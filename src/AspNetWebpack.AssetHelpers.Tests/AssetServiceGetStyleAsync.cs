using System.Threading.Tasks;
using AspNetWebpack.AssetHelpers.Tests.Internal;
using Microsoft.AspNetCore.Html;
using Shouldly;
using Xunit;

namespace AspNetWebpack.AssetHelpers.Tests
{
    public class AssetServiceGetStyleAsync
    {
        private readonly AssetServiceFixture _fixture;

        public AssetServiceGetStyleAsync()
        {
            _fixture = new AssetServiceFixture();
        }

        [Fact]
        public async Task GetStyleAsync_EmptyString_ShouldThrowArgumentNullException()
        {
            // Arrange
            const string bundle = "";
            _fixture.SetupGetFromManifestAsync(bundle, null);

            // Act
            var result = await _fixture.AssetService.GetStyleAsync(bundle);

            // Assert
            result.ShouldBe(HtmlString.Empty);
            _fixture.VerifyGetStyleAsync(bundle);
            _fixture.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetStyleAsync_NonExistingBundle_ShouldReturnEmptyHtmlString()
        {
            // Arrange
            const string bundle = "NonExistingBundle.css";
            _fixture.SetupGetFromManifestAsync(bundle, null);

            // Act
            var result = await _fixture.AssetService.GetStyleAsync(bundle);

            // Assert
            result.ShouldBe(HtmlString.Empty);
            _fixture.VerifyGetStyleAsync(bundle);
            _fixture.VerifyGetFromManifestAsync(bundle);
            _fixture.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetStyleAsync_ExistingBundle_ShouldReturnLinkTag()
        {
            // Arrange
            const string bundle = "ExistingBundle.css";
            var resultBundle = $"{bundle}?v=1234";
            _fixture.SetupGetFromManifestAsync(bundle, resultBundle);
            _fixture.SetupGetStyleTagAsync(resultBundle);

            // Act
            var result = await _fixture.AssetService.GetStyleAsync(bundle);

            // Assert
            result.ShouldBeLinkTag(_fixture.GetBundlePath(resultBundle));
            _fixture.VerifyGetStyleAsync(bundle);
            _fixture.VerifyGetFromManifestAsync(bundle);
            _fixture.VerifyGetStyleTagAsync(resultBundle);
            _fixture.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetStyleAsync_ExistingBundleInline_ShouldReturnStyleTag()
        {
            // Arrange
            const StyleLoad styleLoad = StyleLoad.Inline;
            const string bundle = "ExistingBundle.css";
            var resultBundle = $"{bundle}?v=1234";
            _fixture.SetupGetFromManifestAsync(bundle, resultBundle);
            _fixture.SetupGetStyleTagAsync(resultBundle, styleLoad);

            // Act
            var result = await _fixture.AssetService.GetStyleAsync(bundle, styleLoad);

            // Assert
            result.ShouldBeStyleTag();
            _fixture.VerifyGetStyleAsync(bundle, styleLoad);
            _fixture.VerifyGetFromManifestAsync(bundle);
            _fixture.VerifyGetStyleTagAsync(resultBundle, styleLoad);
            _fixture.VerifyNoOtherCalls();
        }
    }
}
