// <copyright file="GetScriptTagFixture.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Moq;
using Moq.Protected;

namespace AspNetWebpack.AssetHelpers.Testing
{
    /// <summary>
    /// Fixture for testing GetScriptTagAsync in AssetService.
    /// </summary>
    public class GetScriptTagFixture : AssetServiceFixture
    {
        private const string ExistingBundle = "ExistingBundle.js";
        private const string FallbackBundle = "FallbackBundle.js";

        /// <summary>
        /// Initializes a new instance of the <see cref="GetScriptTagFixture"/> class.
        /// </summary>
        /// <param name="bundle">The bundle name to test against.</param>
        /// <param name="scriptLoad">The script load mode.</param>
        public GetScriptTagFixture(string bundle, ScriptLoad scriptLoad = ScriptLoad.Normal)
            : base(bundle, ExistingBundle, FallbackBundle)
        {
            ScriptLoad = scriptLoad;
            SetupGetFromManifest();
            SetupBuildScriptTag(ExistingResultBundle, ExistingResultBundlePath);
            SetupBuildScriptTag(FallbackResultBundle, FallbackResultBundlePath);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetScriptTagFixture"/> class.
        /// </summary>
        /// <param name="scriptLoad">The script load mode.</param>
        public GetScriptTagFixture(ScriptLoad scriptLoad = ScriptLoad.Normal)
            : this(ExistingBundle, scriptLoad)
        {
        }

        /// <summary>
        /// Gets the script load mode.
        /// </summary>
        public ScriptLoad ScriptLoad { get; }

        /// <summary>
        /// Calls GetScriptTagAsync.
        /// </summary>
        /// <returns>The result of the called function.</returns>
        public async Task<HtmlString> GetScriptTagAsync()
        {
            return await AssetService.GetScriptTagAsync(Bundle, ScriptLoad).ConfigureAwait(false);
        }

        /// <summary>
        /// Calls GetScriptTagAsync with a fallback bundle.
        /// </summary>
        /// <returns>The result of the called function.</returns>
        public async Task<HtmlString> GetScriptTagFallbackAsync()
        {
            return await AssetService.GetScriptTagAsync(Bundle, FallbackBundle, ScriptLoad).ConfigureAwait(false);
        }

        /// <summary>
        /// Verify that GetScriptTagAsync is called with an empty string.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyEmpty(HtmlString result)
        {
            result.Should().Be(HtmlString.Empty);
            VerifyGetScriptTag(Bundle);
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetScriptTagAsync is called with a non existing bundle.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyNonExisting(HtmlString result)
        {
            result.Should().Be(HtmlString.Empty);
            VerifyGetScriptTag(Bundle);
            VerifyGetFromManifest(Bundle);
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetScriptTagAsync is called with an existing bundle.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyExisting(HtmlString result)
        {
            result.ShouldBeScriptTag(ExistingResultBundlePath, ScriptLoad);
            VerifyGetScriptTag(Bundle);
            VerifyGetFromManifest(Bundle);
            VerifyBuildScriptTag(ExistingResultBundle);
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetScriptTagAsync is called with a non existing bundle and uses the fallback bundle.
        /// </summary>
        /// <param name="result">The result to assert.</param>
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
                _ => throw new ArgumentException($"{nameof(ScriptLoad)} is invalid!"),
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
