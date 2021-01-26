// <copyright file="GetLinkTagFixture.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Moq;

namespace AspNetWebpack.AssetHelpers.Testing
{
    /// <summary>
    /// Fixture for testing GetLinkTagAsync function in AssetService.
    /// </summary>
    public class GetLinkTagFixture : AssetServiceBaseFixture
    {
        /// <summary>
        /// Valid bundle name with extension.
        /// </summary>
        public static readonly string ValidBundleWithExtension = $"{ValidBundleWithoutExtension}.css";

        /// <summary>
        /// Valid fallback bundle name with extension.
        /// </summary>
        public static readonly string ValidFallbackBundleWithExtension = $"{ValidFallbackBundleWithoutExtension}.css";

        private const string LinkTag = "<link href=\"Bundle.css\" />";
        private const string FallbackLinkTag = "<link href=\"FallbackBundle.css\" />";

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLinkTagFixture"/> class.
        /// </summary>
        /// <param name="bundle">The bundle name param to be used in GetLinkTagAsync.</param>
        /// <param name="fallbackBundle">The fallback bundle name param to be used in GetLinkTagAsync.</param>
        public GetLinkTagFixture(string bundle, string? fallbackBundle = null)
            : base(ValidBundleWithExtension, ValidFallbackBundleWithExtension)
        {
            Bundle = bundle;
            FallbackBundle = fallbackBundle;
            SetupGetFromManifest();
            SetupBuildLinkTag(ValidBundleResult, LinkTag);
            SetupBuildLinkTag(ValidFallbackBundleResult, FallbackLinkTag);
        }

        private string Bundle { get; }

        private string? FallbackBundle { get; }

        /// <summary>
        /// Calls GetLinkTagAsync with provided parameters.
        /// </summary>
        /// <returns>The result of the called function.</returns>
        public async Task<HtmlString> GetLinkTagAsync()
        {
            return await AssetService
                .GetLinkTagAsync(Bundle, FallbackBundle)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Verify that GetLinkTagAsync was called with an empty string.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyEmpty(HtmlString result)
        {
            result.Should().Be(HtmlString.Empty);
            VerifyDependencies();
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetLinkTagAsync was called with an invalid bundle.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyNonExisting(HtmlString result)
        {
            result.Should().Be(HtmlString.Empty);
            VerifyDependencies();
            VerifyGetFromManifest(Bundle, FallbackBundle, ".css");
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetLinkTagAsync was called with a valid bundle.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyExisting(HtmlString result)
        {
            result.Should().BeEquivalentTo(new HtmlString(LinkTag));
            VerifyDependencies();
            VerifyGetFromManifest(Bundle, FallbackBundle, ".css");
            VerifyBuildLinkTag(ValidBundleResult);
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetLinkTagAsync was called with an empty string as fallback.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyFallbackEmpty(HtmlString result)
        {
            result.Should().Be(HtmlString.Empty);
            VerifyDependencies();
            VerifyGetFromManifest(Bundle, FallbackBundle, ".css");
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetLinkTagAsync was called with an invalid bundle and an invalid fallback bundle.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyFallbackNonExisting(HtmlString result)
        {
            result.Should().Be(HtmlString.Empty);
            VerifyDependencies();
            VerifyGetFromManifest(Bundle, FallbackBundle, ".css");
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetLinkTagAsync was called with an invalid bundle and a valid fallback bundle.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyFallbackExisting(HtmlString result)
        {
            result.Should().BeEquivalentTo(new HtmlString(FallbackLinkTag));
            VerifyDependencies();
            VerifyGetFromManifest(Bundle, FallbackBundle, ".css");
            VerifyBuildLinkTag(ValidFallbackBundleResult);
            VerifyNoOtherCalls();
        }

        private void SetupBuildLinkTag(string resultBundle, string returnValue)
        {
            TagBuilderMock
                .Setup(x => x.BuildLinkTag(resultBundle))
                .Returns(returnValue);
        }

        private void VerifyBuildLinkTag(string resultBundle)
        {
            TagBuilderMock.Verify(x => x.BuildLinkTag(resultBundle), Times.Once());
        }
    }
}
