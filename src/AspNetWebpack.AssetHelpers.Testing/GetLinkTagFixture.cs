using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Moq;
using Moq.Protected;

namespace AspNetWebpack.AssetHelpers.Testing
{
    public class GetLinkTagFixture : AssetServiceFixture
    {
        private const string ExistingBundle = "ExistingBundle.css";

        public GetLinkTagFixture(string bundle)
            : base(bundle, ExistingBundle)
        {
            SetupGetFromManifest();
            SetupBuildLinkTag();
        }

        public GetLinkTagFixture()
            : this(ExistingBundle)
        {
        }

        public async Task<HtmlString> GetLinkTagAsync()
        {
            return await AssetService.GetLinkTagAsync(Bundle).ConfigureAwait(false);
        }

        public void Verify(HtmlString result)
        {
            result.ShouldBeLinkTag(ResultBundlePath);
            VerifyGetLinkTag();
            VerifyGetFromManifest();
            VerifyBuildLinkTag();
            VerifyNoOtherCalls();
        }

        public void VerifyGetLinkTag()
        {
            AssetServiceMock.Verify(x => x.GetLinkTagAsync(Bundle));
        }

        private void SetupBuildLinkTag()
        {
            AssetServiceMock
                .Protected()
                .Setup<string>("BuildLinkTag", ResultBundle)
                .Returns($"<link href=\"{ResultBundlePath}\" />");
        }

        private void VerifyBuildLinkTag()
        {
            AssetServiceMock
                .Protected()
                .Verify("BuildLinkTag", Times.Once(), ResultBundle);
        }
    }
}
