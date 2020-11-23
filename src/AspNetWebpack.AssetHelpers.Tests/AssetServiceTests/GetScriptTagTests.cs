using System.Threading.Tasks;
using AspNetWebpack.AssetHelpers.Testing;
using Xunit;

namespace AspNetWebpack.AssetHelpers.Tests.AssetServiceTests
{
    public sealed class GetScriptTagTests
    {
        [Fact]
        public async Task GetScriptTag_EmptyString_ShouldReturnEmptyHtmlString()
        {
            // Arrange
            var fixture = new GetScriptTagFixture(string.Empty);

            // Act
            var result = await fixture.GetScriptTagAsync();

            // Assert
            fixture.VerifyEmpty(result);
        }

        [Fact]
        public async Task GetScriptTag_NonExistingBundle_ShouldReturnEmptyHtmlString()
        {
            // Arrange
            var fixture = new GetScriptTagFixture("NonExistingBundle.js");

            // Act
            var result = await fixture.GetScriptTagAsync();

            // Assert
            fixture.VerifyNonExisting(result);
        }

        [Fact]
        public async Task GetScriptTag_ExistingBundle_ShouldReturnScriptTag()
        {
            // Arrange
            var fixture = new GetScriptTagFixture();

            // Act
            var result = await fixture.GetScriptTagAsync();

            // Assert
            fixture.VerifyExisting(result);
        }

        [Fact]
        public async Task GetScriptTag_ExistingBundleAsync_ShouldReturnAsyncScriptTag()
        {
            // Arrange
            var fixture = new GetScriptTagFixture(ScriptLoad.Async);

            // Act
            var result = await fixture.GetScriptTagAsync();

            // Assert
            fixture.VerifyExisting(result);
        }

        [Fact]
        public async Task GetScriptTag_ExistingBundleDefer_ShouldReturnDeferScriptTag()
        {
            // Arrange
            var fixture = new GetScriptTagFixture(ScriptLoad.Defer);

            // Act
            var result = await fixture.GetScriptTagAsync();

            // Assert
            fixture.VerifyExisting(result);
        }

        [Fact]
        public async Task GetScriptTag_ExistingBundleAsyncDefer_ShouldReturnAsyncDeferScriptTag()
        {
            // Arrange
            var fixture = new GetScriptTagFixture(ScriptLoad.AsyncDefer);

            // Act
            var result = await fixture.GetScriptTagAsync();

            // Assert
            fixture.VerifyExisting(result);
        }

        [Fact]
        public async Task GetScriptTag_FallbackBundle_ShouldReturnScriptTag()
        {
            // Arrange
            var fixture = new GetScriptTagFixture("NonExistingBundle.css");

            // Act
            var result = await fixture.GetScriptTagFallbackAsync();

            // Assert
            fixture.VerifyFallback(result);
        }
    }
}
