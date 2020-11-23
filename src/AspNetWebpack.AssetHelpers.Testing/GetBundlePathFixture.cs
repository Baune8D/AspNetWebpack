using System.Threading.Tasks;
using FluentAssertions;

namespace AspNetWebpack.AssetHelpers.Testing
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
            return await AssetService.GetBundlePathAsync(Bundle).ConfigureAwait(false);
        }

        public void VerifyEmpty(string? result)
        {
            result.Should().BeNull();
            VerifyGetBundlePath();
            VerifyNoOtherCalls();
        }

        public void VerifyNonExisting(string? result)
        {
            result.Should().BeNull();
            VerifyGetBundlePath();
            VerifyGetFromManifest(Bundle);
            VerifyNoOtherCalls();
        }

        public void VerifyExisting(string? result)
        {
            result.Should().NotBeNull();
            result.Should().Be(ExistingResultBundlePath);
            VerifyGetBundlePath();
            VerifyGetFromManifest(Bundle);
            VerifyNoOtherCalls();
        }

        private void VerifyGetBundlePath()
        {
            AssetServiceMock.Verify(x => x.GetBundlePathAsync(Bundle));
        }
    }
}
