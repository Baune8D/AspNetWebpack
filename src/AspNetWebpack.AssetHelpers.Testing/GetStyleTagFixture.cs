// <copyright file="GetStyleTagFixture.cs" company="Morten Larsen">
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
    /// Fixture for testing GetStyleTagAsync in AssetService.
    /// </summary>
    public class GetStyleTagFixture : AssetServiceFixture
    {
        private const string ExistingBundle = "ExistingBundle.css";
        private const string FallbackBundle = "FallbackBundle.css";

        /// <summary>
        /// Initializes a new instance of the <see cref="GetStyleTagFixture"/> class.
        /// </summary>
        /// <param name="bundle">The bundle name to test against.</param>
        public GetStyleTagFixture(string bundle)
            : base(bundle, ExistingBundle, FallbackBundle)
        {
            SetupGetFromManifest();
            SetupBuildStyleTag(ExistingResultBundle);
            SetupBuildStyleTag(FallbackResultBundle);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetStyleTagFixture"/> class.
        /// </summary>
        public GetStyleTagFixture()
            : this(ExistingBundle)
        {
        }

        /// <summary>
        /// Calls GetStyleTagAsync.
        /// </summary>
        /// <returns>The result of the called function.</returns>
        public async Task<HtmlString> GetStyleTagAsync()
        {
            return await AssetService.GetStyleTagAsync(Bundle).ConfigureAwait(false);
        }

        /// <summary>
        /// Calls GetStyleTagAsync with a fallback bundle.
        /// </summary>
        /// <returns>The result of the called function.</returns>
        public async Task<HtmlString> GetStyleTagFallbackAsync()
        {
            return await AssetService.GetStyleTagAsync(Bundle, FallbackBundle).ConfigureAwait(false);
        }

        /// <summary>
        /// Verify that GetStyleTagAsync is called with an empty string.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyEmpty(HtmlString result)
        {
            result.Should().Be(HtmlString.Empty);
            VerifyGetStyleTag(Bundle, null);
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetStyleTagAsync is called with a non existing bundle.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyNonExisting(HtmlString result)
        {
            result.Should().Be(HtmlString.Empty);
            VerifyGetStyleTag(Bundle, null);
            VerifyGetFromManifest(Bundle);
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetStyleTagAsync is called with an existing bundle.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyExisting(HtmlString result)
        {
            result.ShouldBeStyleTag();
            VerifyGetStyleTag(Bundle, null);
            VerifyGetFromManifest(Bundle);
            VerifyBuildStyleTag(ExistingResultBundle);
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetStyleTagAsync is called with a non existing bundle and uses the fallback bundle.
        /// </summary>
        /// <param name="result">The result to assert.</param>
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
