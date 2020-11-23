using System.Threading.Tasks;
using AspNetWebpack.AssetHelpers.Testing;
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
            fixture.VerifyEmpty(result);
        }

        [Fact]
        public async Task GetLinkTag_NonExistingBundle_ShouldReturnEmptyHtmlString()
        {
            // Arrange
            var fixture = new GetLinkTagFixture("NonExistingBundle.css");

            // Act
            var result = await fixture.GetLinkTagAsync();

            // Assert
            fixture.VerifyNonExisting(result);
        }

        [Fact]
        public async Task GetLinkTag_ExistingBundle_ShouldReturnLinkTag()
        {
            // Arrange
            var fixture = new GetLinkTagFixture();

            // Act
            var result = await fixture.GetLinkTagAsync();

            // Assert
            fixture.VerifyExisting(result);
        }

        [Fact]
        public async Task GetLinkTag_FallbackBundle_ShouldReturnLinkTag()
        {
            // Arrange
            var fixture = new GetLinkTagFixture("NonExistingBundle.css");

            // Act
            var result = await fixture.GetLinkTagFallbackAsync();

            // Assert
            fixture.VerifyFallback(result);
        }
    }
}
