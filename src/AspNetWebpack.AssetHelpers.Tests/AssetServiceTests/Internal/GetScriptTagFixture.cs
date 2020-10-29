using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Moq;
using Moq.Protected;

namespace AspNetWebpack.AssetHelpers.Tests.AssetServiceTests.Internal
{
    public class GetScriptTagFixture : AssetServiceFixture
    {
        private const string ExistingBundle = "ExistingBundle.js";

        public GetScriptTagFixture(string bundle, ScriptLoad scriptLoad = ScriptLoad.Normal)
            : base(bundle, ExistingBundle)
        {
            ScriptLoad = scriptLoad;
            SetupGetFromManifest();
            SetupBuildScriptTag();
        }

        public GetScriptTagFixture(ScriptLoad scriptLoad = ScriptLoad.Normal)
            : this(ExistingBundle, scriptLoad)
        {
        }

        public ScriptLoad ScriptLoad { get; }

        public async Task<HtmlString> GetScriptTagAsync()
        {
            return await AssetService.GetScriptTagAsync(Bundle, ScriptLoad);
        }

        public void Verify(HtmlString result)
        {
            result.ShouldBeScriptTag(ResultBundlePath, ScriptLoad);
            VerifyGetScriptTag();
            VerifyGetFromManifest();
            VerifyBuildScriptTag();
            VerifyNoOtherCalls();
        }

        public void VerifyGetScriptTag()
        {
            AssetServiceMock.Verify(x => x.GetScriptTagAsync(Bundle, ScriptLoad));
        }

        private void SetupBuildScriptTag()
        {
            string result = ScriptLoad switch
            {
                ScriptLoad.Normal => $"<script src=\"{ResultBundlePath}\"></script>",
                ScriptLoad.Async => $"<script src=\"{ResultBundlePath}\" async></script>",
                ScriptLoad.Defer => $"<script src=\"{ResultBundlePath}\" defer></script>",
                ScriptLoad.AsyncDefer => $"<script src=\"{ResultBundlePath}\" async defer></script>",
                _ => throw new ArgumentOutOfRangeException(nameof(ScriptLoad), ScriptLoad, null),
            };

            AssetServiceMock
                .Protected()
                .Setup<string>("BuildScriptTag", ResultBundle, ScriptLoad)
                .Returns(result);
        }

        private void VerifyBuildScriptTag()
        {
            AssetServiceMock
                .Protected()
                .Verify("BuildScriptTag", Times.Once(), ResultBundle, ScriptLoad);
        }
    }
}
