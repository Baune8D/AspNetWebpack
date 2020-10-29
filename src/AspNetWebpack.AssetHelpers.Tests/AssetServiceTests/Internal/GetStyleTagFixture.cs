using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Moq;
using Moq.Protected;

namespace AspNetWebpack.AssetHelpers.Tests.AssetServiceTests.Internal
{
    public class GetStyleTagFixture : AssetServiceFixture
    {
        private const string ExistingBundle = "ExistingBundle.css";

        public GetStyleTagFixture(string bundle)
            : base(bundle, ExistingBundle)
        {
            SetupGetFromManifest();
            SetupBuildStyleTag();
        }

        public GetStyleTagFixture()
            : this(ExistingBundle)
        {
        }

        public async Task<HtmlString> GetStyleTagAsync()
        {
            return await AssetService.GetStyleTagAsync(Bundle);
        }

        public void Verify(HtmlString result)
        {
            result.ShouldBeStyleTag();
            VerifyGetStyleTag();
            VerifyGetFromManifest();
            VerifyBuildStyleTag();
            VerifyNoOtherCalls();
        }

        public void VerifyGetStyleTag()
        {
            AssetServiceMock.Verify(x => x.GetStyleTagAsync(Bundle));
        }

        private void SetupBuildStyleTag()
        {
            AssetServiceMock
                .Protected()
                .Setup<Task<string>>("BuildStyleTagAsync", ResultBundle)
                .ReturnsAsync("<style>Test</style>");
        }

        private void VerifyBuildStyleTag()
        {
            AssetServiceMock
                .Protected()
                .Verify("BuildStyleTagAsync", Times.Once(), ResultBundle);
        }
    }
}
