using System.Threading.Tasks;
using AspNetWebpack.AssetHelpers.Testing;
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
            fixture.VerifyEmpty(result);
        }

        [Fact]
        public async Task GetStyleTag_NonExistingBundle_ShouldReturnEmptyHtmlString()
        {
            // Arrange
            var fixture = new GetStyleTagFixture("NonExistingBundle.css");

            // Act
            var result = await fixture.GetStyleTagAsync();

            // Assert
            fixture.VerifyNonExisting(result);
        }

        [Fact]
        public async Task GetStyleTag_ExistingBundle_ShouldReturnStyleTag()
        {
            // Arrange
            var fixture = new GetStyleTagFixture();

            // Act
            var result = await fixture.GetStyleTagAsync();

            // Assert
            fixture.VerifyExisting(result);
        }

        [Fact]
        public async Task GetStyleTag_FallbackBundle_ShouldReturnStyleTag()
        {
            // Arrange
            var fixture = new GetStyleTagFixture("NonExistingBundle.css");

            // Act
            var result = await fixture.GetStyleTagFallbackAsync();

            // Assert
            fixture.VerifyFallback(result);
        }
    }
}
