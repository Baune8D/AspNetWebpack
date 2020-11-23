using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Moq;
using Moq.Protected;

namespace AspNetWebpack.AssetHelpers.Testing
{
    public class GetStyleTagFixture : AssetServiceFixture
    {
        private const string ExistingBundle = "ExistingBundle.css";
        private const string FallbackBundle = "FallbackBundle.css";

        public GetStyleTagFixture(string bundle)
            : base(bundle, ExistingBundle, FallbackBundle)
        {
            SetupGetFromManifest();
            SetupBuildStyleTag(ExistingResultBundle);
            SetupBuildStyleTag(FallbackResultBundle);
        }

        public GetStyleTagFixture()
            : this(ExistingBundle)
        {
        }

        public async Task<HtmlString> GetStyleTagAsync()
        {
            return await AssetService.GetStyleTagAsync(Bundle).ConfigureAwait(false);
        }

        public async Task<HtmlString> GetStyleTagFallbackAsync()
        {
            return await AssetService.GetStyleTagAsync(Bundle, FallbackBundle).ConfigureAwait(false);
        }

        public void VerifyEmpty(HtmlString result)
        {
            result.Should().Be(HtmlString.Empty);
            VerifyGetStyleTag(Bundle, null);
            VerifyNoOtherCalls();
        }

        public void VerifyNonExisting(HtmlString result)
        {
            result.Should().Be(HtmlString.Empty);
            VerifyGetStyleTag(Bundle, null);
            VerifyGetFromManifest(Bundle);
            VerifyNoOtherCalls();
        }

        public void VerifyExisting(HtmlString result)
        {
            result.ShouldBeStyleTag();
            VerifyGetStyleTag(Bundle, null);
            VerifyGetFromManifest(Bundle);
            VerifyBuildStyleTag(ExistingResultBundle);
            VerifyNoOtherCalls();
        }

        public void VerifyFallback(HtmlString result)
        {
            result.ShouldBeStyleTag();
            VerifyGetStyleTag(Bundle, FallbackBundle);
            VerifyGetFromManifest(Bundle);
            VerifyGetFromManifest(FallbackBundle);
            VerifyBuildStyleTag(FallbackResultBundle);
            VerifyNoOtherCalls();
        }

        private void VerifyGetStyleTag(string bundle, string? fallbackBundle)
        {
            AssetServiceMock.Verify(x => x.GetStyleTagAsync(bundle, fallbackBundle));
        }

        private void SetupBuildStyleTag(string resultBundle)
        {
            AssetServiceMock
                .Protected()
                .Setup<Task<string>>("BuildStyleTagAsync", resultBundle)
                .ReturnsAsync("<style>Test</style>");
        }

        private void VerifyBuildStyleTag(string resultBundle)
        {
            AssetServiceMock
                .Protected()
                .Verify("BuildStyleTagAsync", Times.Once(), resultBundle);
        }
    }
}
