// <copyright file="ConstructorTests.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace AspNetWebpack.Tests.AssetServiceTests
{
    public sealed class ConstructorTests
    {
        [Fact]
        public void Constructor_SharedSettingsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var manifestServiceMock = new Mock<IManifestService>();
            var tagBuilderMock = new Mock<ITagBuilder>();

            // Act
            Action act = () => _ = new AssetService(null!, manifestServiceMock.Object, tagBuilderMock.Object);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
            manifestServiceMock.VerifyNoOtherCalls();
            tagBuilderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void Constructor_Default_ShouldSetAllVariables()
        {
            // Arrange
            const string assetsDirectoryPath = "SomeDirectoryPath";
            const string assetsWebPath = "SomeWebPath";

            var sharedSettingsMock = new Mock<ISharedSettings>();

            sharedSettingsMock
                .SetupGet(x => x.AssetsDirectoryPath)
                .Returns(assetsDirectoryPath);

            sharedSettingsMock
                .SetupGet(x => x.AssetsWebPath)
                .Returns(assetsWebPath);

            var manifestServiceMock = new Mock<IManifestService>();
            var tagBuilderMock = new Mock<ITagBuilder>();

            // Act
            var result = new AssetService(sharedSettingsMock.Object, manifestServiceMock.Object, tagBuilderMock.Object);

            // Assert
            result.AssetsDirectoryPath.Should().Be(assetsDirectoryPath);
            result.AssetsWebPath.Should().Be(assetsWebPath);
            manifestServiceMock.VerifyNoOtherCalls();
            tagBuilderMock.VerifyNoOtherCalls();
        }
    }
}
