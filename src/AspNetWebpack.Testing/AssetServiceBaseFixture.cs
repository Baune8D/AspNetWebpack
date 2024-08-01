// <copyright file="AssetServiceBaseFixture.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System;
using Moq;

namespace AspNetWebpack.Testing
{
    /// <summary>
    /// Base fixture for testing AssetService.
    /// </summary>
    public abstract class AssetServiceBaseFixture
    {
        /// <summary>
        /// Valid bundle name without extension.
        /// </summary>
        public const string ValidBundleWithoutExtension = "Bundle";

        /// <summary>
        /// Valid fallback bundle name without extension.
        /// </summary>
        public const string ValidFallbackBundleWithoutExtension = "FallbackBundle";

        /// <summary>
        /// Invalid bundle name.
        /// </summary>
        public const string InvalidBundle = "InvalidBundle";

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetServiceBaseFixture"/> class.
        /// </summary>
        /// <param name="validTestBundle">A version of ValidBundleWithoutExtension with an extension appended.</param>
        /// <param name="validFallbackTestBundle">A version of ValidFallbackBundleWithoutExtension with an extension appended.</param>
        protected AssetServiceBaseFixture(string validTestBundle, string? validFallbackTestBundle = null)
        {
            ValidTestBundle = validTestBundle;
            ValidFallbackTestBundle = validFallbackTestBundle;

            SharedSettingsMock = DependencyMocker.GetSharedSettings(TestValues.Development);
            ManifestServiceMock = new Mock<IManifestService>();
            TagBuilderMock = new Mock<ITagBuilder>();

            AssetService = new AssetService(SharedSettingsMock.Object, ManifestServiceMock.Object, TagBuilderMock.Object);
        }

        /// <summary>
        /// Gets the true AssetService object.
        /// </summary>
        protected IAssetService AssetService { get; }

        /// <summary>
        /// Gets the ITagBuilder mock.
        /// </summary>
        protected Mock<ITagBuilder> TagBuilderMock { get; }

        /// <summary>
        /// Gets the full test bundle filename including cache buster.
        /// </summary>
        protected string ValidBundleResult => $"{ValidTestBundle}?v=1234";

        /// <summary>
        /// Gets the full path of <see cref="ValidBundleResult"/>.
        /// </summary>
        protected string ValidBundleResultPath => $"{AssetService.AssetsWebPath}{ValidBundleResult}";

        /// <summary>
        /// Gets the full fallback test bundle filename including cache buster.
        /// </summary>
        protected string ValidFallbackBundleResult => $"{ValidFallbackTestBundle}?v=1234";

        /// <summary>
        /// Gets the full path of <see cref="ValidFallbackBundleResult"/>.
        /// </summary>
        protected string ValidFallbackBundleResultPath => $"{AssetService.AssetsWebPath}{ValidFallbackBundleResult}";

        private Mock<ISharedSettings> SharedSettingsMock { get; }

        private Mock<IManifestService> ManifestServiceMock { get; }

        private string ValidTestBundle { get; }

        private string? ValidFallbackTestBundle { get; }

        /// <summary>
        /// Verify dependency functions triggered in constructor.
        /// </summary>
        protected void VerifyDependencies()
        {
            SharedSettingsMock.VerifyGet(x => x.AssetsDirectoryPath, Times.Once);
        }

        /// <summary>
        /// Verify GetFromManifestAsync.
        /// </summary>
        /// <param name="bundle">The bundle to verify against.</param>
        /// <param name="times">Number of times to verify GetFromManifestAsync.</param>
        protected void VerifyGetFromManifest(string bundle, Times? times = null)
        {
            ManifestServiceMock.Verify(x => x.GetFromManifestAsync(bundle), times ?? Times.Once());
        }

        /// <summary>
        /// Verify GetFromManifestAsync.
        /// </summary>
        /// <param name="bundle">The bundle to verify against.</param>
        /// <param name="fallbackBundle">The fallback bundle to verify against.</param>
        /// <param name="extension">The extension to use.</param>
        protected void VerifyGetFromManifest(string bundle, string? fallbackBundle, string extension)
        {
            ArgumentNullException.ThrowIfNull(bundle);

            var bundleIsValid =
                bundle == ValidBundleWithoutExtension ||
                bundle == $"{ValidBundleWithoutExtension}{extension}";

            if (bundle == fallbackBundle)
            {
                VerifyGetFromManifest(
                    bundle.EndsWith(extension, StringComparison.Ordinal) ? bundle : $"{bundle}{extension}",
                    bundleIsValid ? Times.Once() : Times.Exactly(2));
            }
            else
            {
                VerifyGetFromManifest(bundle.EndsWith(extension, StringComparison.Ordinal)
                    ? bundle
                    : $"{bundle}{extension}");

                if (!string.IsNullOrEmpty(fallbackBundle) && !bundleIsValid)
                {
                    VerifyGetFromManifest(fallbackBundle.EndsWith(extension, StringComparison.Ordinal)
                        ? fallbackBundle
                        : $"{fallbackBundle}{extension}");
                }
            }
        }

        /// <summary>
        /// Verify no other calls.
        /// </summary>
        protected void VerifyNoOtherCalls()
        {
            ManifestServiceMock.VerifyNoOtherCalls();
        }

        /// <summary>
        /// Setup GetFromManifestAsync.
        /// </summary>
        protected void SetupGetFromManifest()
        {
            ManifestServiceMock
                .Setup(x => x.GetFromManifestAsync(It.IsAny<string>()))
                .ReturnsAsync((string?)null);

            ManifestServiceMock
                .Setup(x => x.GetFromManifestAsync(ValidTestBundle))
                .ReturnsAsync(ValidBundleResult);

            if (ValidFallbackTestBundle != null)
            {
                ManifestServiceMock
                    .Setup(x => x.GetFromManifestAsync(ValidFallbackTestBundle))
                    .ReturnsAsync(ValidFallbackBundleResult);
            }
        }
    }
}
