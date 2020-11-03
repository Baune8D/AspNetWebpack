using System.Threading.Tasks;
using AspNetWebpack.AssetHelpers.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
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
            result.Should().Be(HtmlString.Empty);
            fixture.VerifyGetScriptTag();
            fixture.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetScriptTag_NonExistingBundle_ShouldReturnEmptyHtmlString()
        {
            // Arrange
            var fixture = new GetScriptTagFixture("NonExistingBundle.js");

            // Act
            var result = await fixture.GetScriptTagAsync();

            // Assert
            result.Should().Be(HtmlString.Empty);
            fixture.VerifyGetScriptTag();
            fixture.VerifyGetFromManifest();
            fixture.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetScriptTag_ExistingBundle_ShouldReturnScriptTag()
        {
            // Arrange
            var fixture = new GetScriptTagFixture();

            // Act
            var result = await fixture.GetScriptTagAsync();

            // Assert
            fixture.Verify(result);
        }

        [Fact]
        public async Task GetScriptTag_ExistingBundleAsync_ShouldReturnAsyncScriptTag()
        {
            // Arrange
            var fixture = new GetScriptTagFixture(ScriptLoad.Async);

            // Act
            var result = await fixture.GetScriptTagAsync();

            // Assert
            fixture.Verify(result);
        }

        [Fact]
        public async Task GetScriptTag_ExistingBundleDefer_ShouldReturnDeferScriptTag()
        {
            // Arrange
            var fixture = new GetScriptTagFixture(ScriptLoad.Defer);

            // Act
            var result = await fixture.GetScriptTagAsync();

            // Assert
            fixture.Verify(result);
        }

        [Fact]
        public async Task GetScriptTag_ExistingBundleAsyncDefer_ShouldReturnAsyncDeferScriptTag()
        {
            // Arrange
            var fixture = new GetScriptTagFixture(ScriptLoad.AsyncDefer);

            // Act
            var result = await fixture.GetScriptTagAsync();

            // Assert
            fixture.Verify(result);
        }
    }
}
