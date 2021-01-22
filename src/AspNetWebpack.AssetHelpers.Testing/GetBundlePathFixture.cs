// <copyright file="GetBundlePathFixture.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System.Threading.Tasks;
using FluentAssertions;

namespace AspNetWebpack.AssetHelpers.Testing
{
    /// <summary>
    /// Fixture for testing GetBundlePathAsync in AssetService.
    /// </summary>
    public class GetBundlePathFixture : AssetServiceFixture
    {
        private const string ExistingBundle = "ExistingBundle.js";

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBundlePathFixture"/> class.
        /// </summary>
        /// <param name="bundle">The bundle name to test against.</param>
        public GetBundlePathFixture(string bundle)
            : base(bundle, ExistingBundle)
        {
            SetupGetFromManifest();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBundlePathFixture"/> class.
        /// </summary>
        public GetBundlePathFixture()
            : this(ExistingBundle)
        {
        }

        /// <summary>
        /// Calls GetBundlePathAsync.
        /// </summary>
        /// <returns>The result of the called function.</returns>
        public async Task<string?> GetBundlePathAsync()
        {
            return await AssetService.GetBundlePathAsync(Bundle).ConfigureAwait(false);
        }

        /// <summary>
        /// Verify that GetBundlePathAsync is called with an empty string.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyEmpty(string? result)
        {
            result.Should().BeNull();
            VerifyGetBundlePath();
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetBundlePathAsync is called with a non existing bundle.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyNonExisting(string? result)
        {
            result.Should().BeNull();
            VerifyGetBundlePath();
            VerifyGetFromManifest(Bundle);
            VerifyNoOtherCalls();
        }

        /// <summary>
        /// Verify that GetBundlePathAsync is called with an existing bundle.
        /// </summary>
        /// <param name="result">The result to assert.</param>
        public void VerifyExisting(string? result)
        {
            result.Should().NotBeNull();
            result.Should().Be(ExistingResultBundlePath);
            VerifyGetBundlePath();
            VerifyGetFromManifest(Bundle);
            VerifyNoOtherCalls();
        }

        private void VerifyGetBundlePath()
        {
            AssetServiceMock.Verify(x => x.GetBundlePathAsync(Bundle));
        }
    }
}
