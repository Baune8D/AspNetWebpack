using System.Threading.Tasks;
using AspNetWebpack.AssetHelpers.Testing;
using Xunit;

namespace AspNetWebpack.AssetHelpers.Tests.AssetServiceTests
{
    public sealed class GetBundlePathTests
    {
        [Fact]
        public async Task GetBundlePath_EmptyString_ShouldReturnNull()
        {
            // Arrange
            var fixture = new GetBundlePathFixture(string.Empty);

            // Act
            var result = await fixture.GetBundlePathAsync();

            // Assert
            fixture.VerifyEmpty(result);
        }

        [Fact]
        public async Task GetBundlePath_NonExistingBundle_ShouldReturnNull()
        {
            // Arrange
            var fixture = new GetBundlePathFixture("NonExistingBundle.js");

            // Act
            var result = await fixture.GetBundlePathAsync();

            // Assert
            fixture.VerifyNonExisting(result);
        }

        [Fact]
        public async Task GetBundlePath_ExistingBundle_ShouldReturnBundlePath()
        {
            // Arrange
            var fixture = new GetBundlePathFixture();

            // Act
            var result = await fixture.GetBundlePathAsync();

            // Assert
            fixture.VerifyExisting(result);
        }
    }
}
