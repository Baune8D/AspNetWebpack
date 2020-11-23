using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Moq;
using Moq.Protected;

namespace AspNetWebpack.AssetHelpers.Testing
{
    public class GetScriptTagFixture : AssetServiceFixture
    {
        private const string ExistingBundle = "ExistingBundle.js";
        private const string FallbackBundle = "FallbackBundle.js";

        public GetScriptTagFixture(string bundle, ScriptLoad scriptLoad = ScriptLoad.Normal)
            : base(bundle, ExistingBundle, FallbackBundle)
        {
            ScriptLoad = scriptLoad;
            SetupGetFromManifest();
            SetupBuildScriptTag(ExistingResultBundle, ExistingResultBundlePath);
            SetupBuildScriptTag(FallbackResultBundle, FallbackResultBundlePath);
        }

        public GetScriptTagFixture(ScriptLoad scriptLoad = ScriptLoad.Normal)
            : this(ExistingBundle, scriptLoad)
        {
        }

        public ScriptLoad ScriptLoad { get; }

        public async Task<HtmlString> GetScriptTagAsync()
        {
            return await AssetService.GetScriptTagAsync(Bundle, ScriptLoad).ConfigureAwait(false);
        }

        public async Task<HtmlString> GetScriptTagFallbackAsync()
        {
            return await AssetService.GetScriptTagAsync(Bundle, FallbackBundle, ScriptLoad).ConfigureAwait(false);
        }

        public void VerifyEmpty(HtmlString result)
        {
            result.Should().Be(HtmlString.Empty);
            VerifyGetScriptTag(Bundle);
            VerifyNoOtherCalls();
        }

        public void VerifyNonExisting(HtmlString result)
        {
            result.Should().Be(HtmlString.Empty);
            VerifyGetScriptTag(Bundle);
            VerifyGetFromManifest(Bundle);
            VerifyNoOtherCalls();
        }

        public void VerifyExisting(HtmlString result)
        {
            result.ShouldBeScriptTag(ExistingResultBundlePath, ScriptLoad);
            VerifyGetScriptTag(Bundle);
            VerifyGetFromManifest(Bundle);
            VerifyBuildScriptTag(ExistingResultBundle);
            VerifyNoOtherCalls();
        }

        public void VerifyFallback(HtmlString result)
        {
            result.ShouldBeScriptTag(FallbackResultBundlePath, ScriptLoad);
            VerifyGetScriptTagFallback(Bundle, FallbackBundle);
            VerifyGetFromManifest(Bundle);
            VerifyGetFromManifest(FallbackBundle);
            VerifyBuildScriptTag(FallbackResultBundle);
            VerifyNoOtherCalls();
        }

        private void VerifyGetScriptTag(string bundle)
        {
            AssetServiceMock.Verify(x => x.GetScriptTagAsync(bundle, ScriptLoad));
            VerifyGetScriptTagFallback(bundle, null);
        }

        private void VerifyGetScriptTagFallback(string bundle, string? fallbackBundle)
        {
            AssetServiceMock.Verify(x => x.GetScriptTagAsync(bundle, fallbackBundle, ScriptLoad));
        }

        private void SetupBuildScriptTag(string resultBundle, string resultBundlePath)
        {
            string result = ScriptLoad switch
            {
                ScriptLoad.Normal => $"<script src=\"{resultBundlePath}\"></script>",
                ScriptLoad.Async => $"<script src=\"{resultBundlePath}\" async></script>",
                ScriptLoad.Defer => $"<script src=\"{resultBundlePath}\" defer></script>",
                ScriptLoad.AsyncDefer => $"<script src=\"{resultBundlePath}\" async defer></script>",
                _ => throw new Exception($"{nameof(ScriptLoad)} is invalid!"),
            };

            AssetServiceMock
                .Protected()
                .Setup<string>("BuildScriptTag", resultBundle, ScriptLoad)
                .Returns(result);
        }

        private void VerifyBuildScriptTag(string resultBundle)
        {
            AssetServiceMock
                .Protected()
                .Verify("BuildScriptTag", Times.Once(), resultBundle, ScriptLoad);
        }
    }
}
