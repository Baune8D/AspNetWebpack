using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Moq;
using Moq.Protected;

namespace AspNetWebpack.AssetHelpers.Testing
{
    public class GetLinkTagFixture : AssetServiceFixture
    {
        private const string ExistingBundle = "ExistingBundle.css";
        private const string FallbackBundle = "FallbackBundle.css";

        public GetLinkTagFixture(string bundle)
            : base(bundle, ExistingBundle, FallbackBundle)
        {
            SetupGetFromManifest();
            SetupBuildLinkTag(ExistingResultBundle, ExistingResultBundlePath);
            SetupBuildLinkTag(FallbackResultBundle, FallbackResultBundlePath);
        }

        public GetLinkTagFixture()
            : this(ExistingBundle)
        {
        }

        public async Task<HtmlString> GetLinkTagAsync()
        {
            return await AssetService.GetLinkTagAsync(Bundle).ConfigureAwait(false);
        }

        public async Task<HtmlString> GetLinkTagFallbackAsync()
        {
            return await AssetService.GetLinkTagAsync(Bundle, FallbackBundle).ConfigureAwait(false);
        }

        public void VerifyEmpty(HtmlString result)
        {
            result.Should().Be(HtmlString.Empty);
            VerifyGetLinkTag(Bundle, null);
            VerifyNoOtherCalls();
        }

        public void VerifyNonExisting(HtmlString result)
        {
            result.Should().Be(HtmlString.Empty);
            VerifyGetLinkTag(Bundle, null);
            VerifyGetFromManifest(Bundle);
            VerifyNoOtherCalls();
        }

        public void VerifyExisting(HtmlString result)
        {
            result.ShouldBeLinkTag(ExistingResultBundlePath);
            VerifyGetLinkTag(Bundle, null);
            VerifyGetFromManifest(Bundle);
            VerifyBuildLinkTag(ExistingResultBundle);
            VerifyNoOtherCalls();
        }

        public void VerifyFallback(HtmlString result)
        {
            result.ShouldBeLinkTag(FallbackResultBundlePath);
            VerifyGetLinkTag(Bundle, FallbackBundle);
            VerifyGetFromManifest(Bundle);
            VerifyGetFromManifest(FallbackBundle);
            VerifyBuildLinkTag(FallbackResultBundle);
            VerifyNoOtherCalls();
        }

        private void VerifyGetLinkTag(string bundle, string? fallbackBundle)
        {
            AssetServiceMock.Verify(x => x.GetLinkTagAsync(bundle, fallbackBundle));
        }

        private void SetupBuildLinkTag(string resultBundle, string resultBundlePath)
        {
            AssetServiceMock
                .Protected()
                .Setup<string>("BuildLinkTag", resultBundle)
                .Returns($"<link href=\"{resultBundlePath}\" />");
        }

        private void VerifyBuildLinkTag(string resultBundle)
        {
            AssetServiceMock
                .Protected()
                .Verify("BuildLinkTag", Times.Once(), resultBundle);
        }
    }
}
