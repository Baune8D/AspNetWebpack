using System.Threading.Tasks;
using FluentAssertions;

namespace AspNetWebpack.AssetHelpers.Tests.AssetServiceTests.Internal
{
    public class GetBundlePathFixture : AssetServiceFixture
    {
        private const string ExistingBundle = "ExistingBundle.js";

        public GetBundlePathFixture(string bundle)
            : base(bundle, ExistingBundle)
        {
            SetupGetFromManifest();
        }

        public GetBundlePathFixture()
            : this(ExistingBundle)
        {
        }

        public async Task<string?> GetBundlePathAsync()
        {
            return await AssetService.GetBundlePathAsync(Bundle);
        }

        public void Verify(string? result)
        {
            result.Should().NotBeNull();
            result.Should().Be(ResultBundlePath);
            VerifyGetBundlePath();
            VerifyGetFromManifest();
            VerifyNoOtherCalls();
        }

        public void VerifyGetBundlePath()
        {
            AssetServiceMock.Verify(x => x.GetBundlePathAsync(Bundle));
        }
    }
}
