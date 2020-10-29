using System.Threading.Tasks;
using AspNetWebpack.AssetHelpers.Tests.AssetServiceTests.Internal;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace AspNetWebpack.AssetHelpers.Tests.AssetServiceTests
{
    public sealed class GetLinkTagTests
    {
        [Fact]
        public async Task GetLinkTag_EmptyString_ShouldReturnEmptyHtmlString()
        {
            // Arrange
            var fixture = new GetLinkTagFixture(string.Empty);

            // Act
            var result = await fixture.GetLinkTagAsync();

            // Assert
            result.Should().Be(HtmlString.Empty);
            fixture.VerifyGetLinkTag();
            fixture.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetLinkTag_NonExistingBundle_ShouldReturnEmptyHtmlString()
        {
            // Arrange
            var fixture = new GetLinkTagFixture("NonExistingBundle.css");

            // Act
            var result = await fixture.GetLinkTagAsync();

            // Assert
            result.Should().Be(HtmlString.Empty);
            fixture.VerifyGetLinkTag();
            fixture.VerifyGetFromManifest();
            fixture.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetLinkTag_ExistingBundle_ShouldReturnLinkTag()
        {
            // Arrange
            var fixture = new GetLinkTagFixture();

            // Act
            var result = await fixture.GetLinkTagAsync();

            // Assert
            fixture.Verify(result);
        }
    }
}
