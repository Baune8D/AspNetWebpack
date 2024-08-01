// <copyright file="GetStyleTagFixture.cs" company="Morten Larsen">
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
    /// Fixture for testing GetStyleTagAsync function in AssetService.
    /// </summary>
    public class GetStyleTagFixture : AssetServiceBaseFixture
    {
        /// <summary>
        /// Valid bundle name with extension.
        /// </summary>
        public static readonly string ValidBundleWithExtension = $"{ValidBundleWithoutExtension}.css";

        /// <summary>
        /// Valid fallback bundle name with extension.
        /// </summary>
        public static readonly string ValidFallbackBundleWithExtension = $"{ValidFallbackBundleWithoutExtension}.css";

        private const string StyleTag = "<style>Some Content</script>";
        private const string FallbackStyleTag = "<style>Some Fallback Content</style>";

        /// <summary>
        /// Initializes a new instance of the <see cref="GetStyleTagFixture"/> class.
        /// </summary>
        /// <param name="bundle">The bundle name param to be used in GetStyleTagAsync.</param>
        /// <param name="fallbackBundle">The fallback bundle name param to be used in GetStyleTagAsync.</param>
        public GetStyleTagFixture(string bundle, string? fallbackBundle = null)
            : base(ValidBundleWithExtension, ValidFallbackBundleWithExtension)
        {
            Bundle = bundle;
            FallbackBundle = fallbackBundle;
            SetupGetFromManifest();
            SetupBuildStyleTag(ValidBundleResult, StyleTag);
            SetupBuildStyleTag(ValidFallbackBundleResult, FallbackStyleTag);
        }

        private string Bundle { get; }

        private string? FallbackBundle { get; }

        /// <summary>
        /// Calls GetStyleTagAsync with provided parameters.
        /// </summary>
        /// <returns>The result of the called function.</returns>
        public async Task<HtmlString> GetStyleTagAsync()
        {
            return await AssetService
                .GetStyleTagAsync(Bundle, FallbackBundle)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Verify that GetStyleTagAsync was called with an empty string.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyEmpty(HtmlString result)
        {
            result.Should().Be(HtmlString.Empty);
            VerifyDependencies();
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetStyleTagAsync was called with an invalid bundle.
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
        /// Verify that GetStyleTagAsync was called with a valid bundle.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyExisting(HtmlString result)
        {
            result.Should().BeEquivalentTo(new HtmlString(StyleTag));
            VerifyDependencies();
            VerifyGetFromManifest(Bundle, FallbackBundle, ".css");
            VerifyBuildStyleTag(ValidBundleResult);
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetStyleTagAsync was called with an empty string as fallback.
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
        /// Verify that GetStyleTagAsync was called with an invalid bundle and an invalid fallback bundle.
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
        /// Verify that GetStyleTagAsync was called with an invalid bundle and a valid fallback bundle.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyFallbackExisting(HtmlString result)
        {
            result.Should().BeEquivalentTo(new HtmlString(FallbackStyleTag));
            VerifyDependencies();
            VerifyGetFromManifest(Bundle, FallbackBundle, ".css");
            VerifyBuildStyleTag(ValidFallbackBundleResult);
            VerifyNoOtherCalls();
        }

        private void SetupBuildStyleTag(string resultBundle, string returnValue)
        {
            TagBuilderMock
                .Setup(x => x.BuildStyleTagAsync(resultBundle))
                .ReturnsAsync(returnValue);
        }

        private void VerifyBuildStyleTag(string resultBundle)
        {
            TagBuilderMock.Verify(x => x.BuildStyleTagAsync(resultBundle), Times.Once());
        }
    }
}
