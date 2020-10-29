using System.Threading.Tasks;
using AspNetWebpack.AssetHelpers.Tests.AssetServiceTests.Internal;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace AspNetWebpack.AssetHelpers.Tests.AssetServiceTests
{
    public sealed class GetStyleTagTests
    {
        [Fact]
        public async Task GetStyleTag_EmptyString_ShouldReturnEmptyHtmlString()
        {
            // Arrange
            var fixture = new GetStyleTagFixture(string.Empty);

            // Act
            var result = await fixture.GetStyleTagAsync();

            // Assert
            result.Should().Be(HtmlString.Empty);
            fixture.VerifyGetStyleTag();
            fixture.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetStyleTag_NonExistingBundle_ShouldReturnEmptyHtmlString()
        {
            // Arrange
            var fixture = new GetStyleTagFixture("NonExistingBundle.css");

            // Act
            var result = await fixture.GetStyleTagAsync();

            // Assert
            result.Should().Be(HtmlString.Empty);
            fixture.VerifyGetStyleTag();
            fixture.VerifyGetFromManifest();
            fixture.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetStyleTag_ExistingBundle_ShouldReturnStyleTag()
        {
            // Arrange
            var fixture = new GetStyleTagFixture();

            // Act
            var result = await fixture.GetStyleTagAsync();

            // Assert
            fixture.Verify(result);
        }
    }
}
