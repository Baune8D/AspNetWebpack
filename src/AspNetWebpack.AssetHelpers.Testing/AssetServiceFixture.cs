// <copyright file="AssetServiceFixture.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace AspNetWebpack.AssetHelpers.Testing
{
    /// <summary>
    /// Fixture for testing AssetService.
    /// </summary>
    public abstract class AssetServiceFixture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetServiceFixture"/> class.
        /// </summary>
        /// <param name="bundle">The bundle name to test against.</param>
        /// <param name="existingBundle">A bundle name that is sure to exist.</param>
        protected AssetServiceFixture(string bundle, string existingBundle)
        {
            Bundle = bundle;
            ExistingBundle = existingBundle;

            var options = Options.Create(new WebpackOptions
            {
                PublicDevServer = "https://public.dev",
                InternalDevServer = "https://internal.dev",
            });

            var env = new Mock<IWebHostEnvironment>();
            var httpClientFactory = new Mock<IHttpClientFactory>();

            AssetServiceMock = new Mock<AssetService>(env.Object, options, httpClientFactory.Object)
            {
                CallBase = true,
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetServiceFixture"/> class.
        /// </summary>
        /// <param name="bundle">The bundle name to test against.</param>
        /// <param name="existingBundle">A bundle name that is sure to exist.</param>
        /// <param name="fallbackBundle">A fallback bundle name if "bundle" does not exist.</param>
        protected AssetServiceFixture(string bundle, string existingBundle, string fallbackBundle)
            : this(bundle, existingBundle)
        {
            FallbackBundle = fallbackBundle;
        }

        /// <summary>
        /// Gets the true AssetService object.
        /// </summary>
        protected IAssetService AssetService => AssetServiceMock.Object;

        /// <summary>
        /// Gets the AssetService mock.
        /// </summary>
        protected Mock<AssetService> AssetServiceMock { get; }

        /// <summary>
        /// Gets the full existing bundle name including cache buster.
        /// </summary>
        protected string ExistingResultBundle => $"{ExistingBundle}?v=1234";

        /// <summary>
        /// Gets the full path of <see cref="ExistingResultBundle"/>.
        /// </summary>
        protected string ExistingResultBundlePath => $"{AssetService.AssetPath}{ExistingResultBundle}";

        /// <summary>
        /// Gets the full fallback bundle name including cache buster.
        /// </summary>
        protected string FallbackResultBundle => $"{FallbackBundle}?v=1234";

        /// <summary>
        /// Gets the full path of <see cref="FallbackResultBundle"/>.
        /// </summary>
        protected string FallbackResultBundlePath => $"{AssetService.AssetPath}{FallbackResultBundle}";

        /// <summary>
        /// Gets the bundle name that is being tested against.
        /// </summary>
        protected string Bundle { get; }

        private string ExistingBundle { get; }

        private string? FallbackBundle { get; }

        /// <summary>
        /// Verify GetFromManifestAsync.
        /// </summary>
        /// <param name="bundle">The bundle to verify against.</param>
        protected void VerifyGetFromManifest(string bundle)
        {
            AssetServiceMock
                .Protected()
                .Verify("GetFromManifestAsync", Times.Once(), bundle);
        }

        /// <summary>
        /// Verify no other calls.
        /// </summary>
        protected void VerifyNoOtherCalls()
        {
            AssetServiceMock.VerifyNoOtherCalls();
        }

        /// <summary>
        /// Setup GetFromManifestAsync.
        /// </summary>
        protected void SetupGetFromManifest()
        {
            AssetServiceMock
                .Protected()
                .Setup<Task<string?>>("GetFromManifestAsync", ItExpr.IsAny<string>())
                .ReturnsAsync((string?)null);

            AssetServiceMock
                .Protected()
                .Setup<Task<string?>>("GetFromManifestAsync", ExistingBundle)
                .ReturnsAsync(ExistingResultBundle);

            if (FallbackBundle != null)
            {
                AssetServiceMock
                    .Protected()
                    .Setup<Task<string?>>("GetFromManifestAsync", FallbackBundle)
                    .ReturnsAsync(FallbackResultBundle);
            }
        }
    }
}
