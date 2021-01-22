// <copyright file="GetLinkTagFixture.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Moq;
using Moq.Protected;

namespace AspNetWebpack.AssetHelpers.Testing
{
    /// <summary>
    /// Fixture for testing GetLinkTagAsync in AssetService.
    /// </summary>
    public class GetLinkTagFixture : AssetServiceFixture
    {
        private const string ExistingBundle = "ExistingBundle.css";
        private const string FallbackBundle = "FallbackBundle.css";

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLinkTagFixture"/> class.
        /// </summary>
        /// <param name="bundle">The bundle name to test against.</param>
        public GetLinkTagFixture(string bundle)
            : base(bundle, ExistingBundle, FallbackBundle)
        {
            SetupGetFromManifest();
            SetupBuildLinkTag(ExistingResultBundle, ExistingResultBundlePath);
            SetupBuildLinkTag(FallbackResultBundle, FallbackResultBundlePath);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLinkTagFixture"/> class.
        /// </summary>
        public GetLinkTagFixture()
            : this(ExistingBundle)
        {
        }

        /// <summary>
        /// Calls GetLinkTagAsync.
        /// </summary>
        /// <returns>The result of the called function.</returns>
        public async Task<HtmlString> GetLinkTagAsync()
        {
            return await AssetService.GetLinkTagAsync(Bundle).ConfigureAwait(false);
        }

        /// <summary>
        /// Calls GetLinkTagAsync with a fallback bundle.
        /// </summary>
        /// <returns>The result of the called function.</returns>
        public async Task<HtmlString> GetLinkTagFallbackAsync()
        {
            return await AssetService.GetLinkTagAsync(Bundle, FallbackBundle).ConfigureAwait(false);
        }

        /// <summary>
        /// Verify that GetLinkTagAsync is called with an empty string.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyEmpty(HtmlString result)
        {
            result.Should().Be(HtmlString.Empty);
            VerifyGetLinkTag(Bundle, null);
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetLinkTagAsync is called with a non existing bundle.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyNonExisting(HtmlString result)
        {
            result.Should().Be(HtmlString.Empty);
            VerifyGetLinkTag(Bundle, null);
            VerifyGetFromManifest(Bundle);
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetLinkTagAsync is called with an existing bundle.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyExisting(HtmlString result)
        {
            result.ShouldBeLinkTag(ExistingResultBundlePath);
            VerifyGetLinkTag(Bundle, null);
            VerifyGetFromManifest(Bundle);
            VerifyBuildLinkTag(ExistingResultBundle);
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetLinkTagAsync is called with a non existing bundle and uses the fallback bundle.
        /// </summary>
        /// <param name="result">The result to assert.</param>
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
