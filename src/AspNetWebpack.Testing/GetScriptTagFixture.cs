// <copyright file="GetScriptTagFixture.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Moq;

namespace AspNetWebpack.Testing
{
    /// <summary>
    /// Fixture for testing GetScriptTagAsync function in AssetService.
    /// </summary>
    public class GetScriptTagFixture : AssetServiceBaseFixture
    {
        /// <summary>
        /// Valid bundle name with extension.
        /// </summary>
        public static readonly string ValidBundleWithExtension = $"{ValidBundleWithoutExtension}.js";

        /// <summary>
        /// Valid fallback bundle name with extension.
        /// </summary>
        public static readonly string ValidFallbackBundleWithExtension = $"{ValidFallbackBundleWithoutExtension}.js";

        private const string ScriptTag = "<script src=\"Bundle.js\"></script>";
        private const string FallbackScriptTag = "<script src=\"FallbackBundle.js\"></script>";

        /// <summary>
        /// Initializes a new instance of the <see cref="GetScriptTagFixture"/> class.
        /// </summary>
        /// <param name="bundle">The bundle name to test against.</param>
        /// <param name="scriptLoad">The script load mode.</param>
        public GetScriptTagFixture(string bundle, ScriptLoad scriptLoad = ScriptLoad.Normal)
            : base(ValidBundleWithExtension, ValidFallbackBundleWithExtension)
        {
            Bundle = bundle;
            ScriptLoad = scriptLoad;
            SetupGetFromManifest();
            SetupBuildScriptTag(ValidBundleResult, ScriptTag);
            SetupBuildScriptTag(ValidFallbackBundleResult, FallbackScriptTag);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetScriptTagFixture"/> class.
        /// </summary>
        /// <param name="bundle">The bundle name to test against.</param>
        /// <param name="fallbackBundle">The fallback bundle name to test against.</param>
        /// <param name="scriptLoad">The script load mode.</param>
        public GetScriptTagFixture(string bundle, string fallbackBundle, ScriptLoad scriptLoad = ScriptLoad.Normal)
            : this(bundle, scriptLoad)
        {
            FallbackBundle = fallbackBundle;
        }

        /// <summary>
        /// Gets the script load mode.
        /// </summary>
        public ScriptLoad ScriptLoad { get; }

        private string Bundle { get; }

        private string? FallbackBundle { get; }

        /// <summary>
        /// Calls GetScriptTagAsync with provided parameters.
        /// </summary>
        /// <returns>The result of the called function.</returns>
        public async Task<HtmlString> GetScriptTagAsync()
        {
            if (FallbackBundle == null)
            {
                return await AssetService
                    .GetScriptTagAsync(Bundle, ScriptLoad)
                    .ConfigureAwait(false);
            }

            return await AssetService
                .GetScriptTagAsync(Bundle, FallbackBundle, ScriptLoad)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Verify that GetScriptTagAsync was called with an empty string.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyEmpty(HtmlString result)
        {
            result.Should().Be(HtmlString.Empty);
            VerifyDependencies();
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetScriptTagAsync was called with an invalid bundle.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyNonExisting(HtmlString result)
        {
            result.Should().Be(HtmlString.Empty);
            VerifyDependencies();
            VerifyGetFromManifest(Bundle, FallbackBundle, ".js");
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetScriptTagAsync is called with a valid bundle.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyExisting(HtmlString result)
        {
            result.Should().BeEquivalentTo(new HtmlString(ScriptTag));
            VerifyDependencies();
            VerifyGetFromManifest(Bundle, FallbackBundle, ".js");
            VerifyBuildScriptTag(ValidBundleResult);
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetScriptTagAsync is called with an empty string as fallback.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyFallbackEmpty(HtmlString result)
        {
            result.Should().Be(HtmlString.Empty);
            VerifyDependencies();
            VerifyGetFromManifest(Bundle, FallbackBundle, ".js");
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetScriptTagAsync is called with an invalid bundle and an invalid fallback bundle.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyFallbackNonExisting(HtmlString result)
        {
            result.Should().Be(HtmlString.Empty);
            VerifyDependencies();
            VerifyGetFromManifest(Bundle, FallbackBundle, ".js");
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetScriptTagAsync is called with an invalid bundle and a valid fallback bundle.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyFallbackExisting(HtmlString result)
        {
            result.Should().BeEquivalentTo(new HtmlString(FallbackScriptTag));
            VerifyDependencies();
            VerifyGetFromManifest(Bundle, FallbackBundle, ".js");
            VerifyBuildScriptTag(ValidFallbackBundleResult);
            VerifyNoOtherCalls();
        }

        private void SetupBuildScriptTag(string resultBundle, string returnValue)
        {
            TagBuilderMock
                .Setup(x => x.BuildScriptTag(resultBundle, ScriptLoad))
                .Returns(returnValue);
        }

        private void VerifyBuildScriptTag(string resultBundle)
        {
            TagBuilderMock.Verify(x => x.BuildScriptTag(resultBundle, ScriptLoad), Times.Once());
        }
    }
}
