// <copyright file="TagBuilderTests.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System;
using System.ComponentModel;
using System.IO.Abstractions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AspNetWebpack.Testing;
using FluentAssertions;
using Moq;
using Xunit;

namespace AspNetWebpack.Tests
{
    public sealed class TagBuilderTests : IDisposable
    {
        private const string Bundle = "Bundle.js";
        private const string HttpClientResponse = "CSS content";

        private readonly Mock<IFileSystem> _fileSystemMock;

        private TagBuilder? _tagBuilder;

        public TagBuilderTests()
        {
            _fileSystemMock = DependencyMocker.GetFileSystem(HttpClientResponse);
        }

        private static string ValidBundleResult => $"{TestValues.AssetsWebPath}{Bundle}";

        public void Dispose()
        {
            _tagBuilder?.Dispose();
        }

        [Fact]
        public void BuildScriptTag_InvalidScriptLoad_ShouldThrowInvalidEnumArgumentException()
        {
            // Arrange
            var sharedSettingsMock = DependencyMocker.GetSharedSettings(TestValues.Development);
            _tagBuilder = new TagBuilder(sharedSettingsMock.Object, _fileSystemMock.Object);

            // Act
            Action act = () => _tagBuilder.BuildScriptTag(Bundle, (ScriptLoad)6);

            // Assert
            act.Should().ThrowExactly<InvalidEnumArgumentException>();
            sharedSettingsMock.VerifyGet(x => x.DevelopmentMode, Times.Exactly(2));
            sharedSettingsMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void BuildScriptTag_Development_ShouldReturnScriptTag()
        {
            // Arrange
            var sharedSettingsMock = DependencyMocker.GetSharedSettings(TestValues.Development);
            _tagBuilder = new TagBuilder(sharedSettingsMock.Object, _fileSystemMock.Object);

            // Act
            var result = _tagBuilder.BuildScriptTag(Bundle, ScriptLoad.Normal);

            // Assert
            result.Should().Contain("crossorigin=\"anonymous\"")
                .And.NotContain("async")
                .And.NotContain("defer");
            VerifyScriptTag(result, sharedSettingsMock);
        }

        [Fact]
        public void BuildScriptTag_DevelopmentAsyncScriptLoad_ShouldReturnScriptTag()
        {
            // Arrange
            var sharedSettingsMock = DependencyMocker.GetSharedSettings(TestValues.Development);
            _tagBuilder = new TagBuilder(sharedSettingsMock.Object, _fileSystemMock.Object);

            // Act
            var result = _tagBuilder.BuildScriptTag(Bundle, ScriptLoad.Async);

            // Assert
            result.Should().Contain("crossorigin=\"anonymous\"")
                .And.Contain("async")
                .And.NotContain("defer");
            VerifyScriptTag(result, sharedSettingsMock);
        }

        [Fact]
        public void BuildScriptTag_DevelopmentDeferScriptLoad_ShouldReturnScriptTag()
        {
            // Arrange
            var sharedSettingsMock = DependencyMocker.GetSharedSettings(TestValues.Development);
            _tagBuilder = new TagBuilder(sharedSettingsMock.Object, _fileSystemMock.Object);

            // Act
            var result = _tagBuilder.BuildScriptTag(Bundle, ScriptLoad.Defer);

            // Assert
            result.Should().Contain("crossorigin=\"anonymous\"")
                .And.Contain("defer")
                .And.NotContain("async");
            VerifyScriptTag(result, sharedSettingsMock);
        }

        [Fact]
        public void BuildScriptTag_DevelopmentAsyncDeferScriptLoad_ShouldReturnScriptTag()
        {
            // Arrange
            var sharedSettingsMock = DependencyMocker.GetSharedSettings(TestValues.Development);
            _tagBuilder = new TagBuilder(sharedSettingsMock.Object, _fileSystemMock.Object);

            // Act
            var result = _tagBuilder.BuildScriptTag(Bundle, ScriptLoad.AsyncDefer);

            // Assert
            result.Should().Contain("crossorigin=\"anonymous\"")
                .And.Contain("async")
                .And.Contain("defer");
            VerifyScriptTag(result, sharedSettingsMock);
        }

        [Fact]
        public void BuildScriptTag_Production_ShouldReturnScriptTag()
        {
            // Arrange
            var sharedSettingsMock = DependencyMocker.GetSharedSettings(TestValues.Production);
            _tagBuilder = new TagBuilder(sharedSettingsMock.Object, _fileSystemMock.Object);

            // Act
            var result = _tagBuilder.BuildScriptTag(Bundle, ScriptLoad.Normal);

            // Assert
            result.Should().NotContain("crossorigin=\"anonymous\"")
                .And.NotContain("async")
                .And.NotContain("defer");
            VerifyScriptTag(result, sharedSettingsMock);
        }

        [Fact]
        public void BuildLinkTag_Default_ShouldReturnLinkTag()
        {
            // Arrange
            var sharedSettingsMock = DependencyMocker.GetSharedSettings(TestValues.Development);
            _tagBuilder = new TagBuilder(sharedSettingsMock.Object, _fileSystemMock.Object);

            // Act
            var result = _tagBuilder.BuildLinkTag(Bundle);

            // Assert
            result.Should().StartWith("<link ")
                .And.EndWith(" />")
                .And.Contain($"href=\"{ValidBundleResult}\"")
                .And.Contain("rel=\"stylesheet\"");
            sharedSettingsMock.VerifyGet(x => x.AssetsWebPath, Times.Once);
            sharedSettingsMock.VerifyNoOtherCalls();
            _fileSystemMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task BuildStyleTag_Null_ShouldThrowArgumentNullException()
        {
            // Arrange
            var sharedSettingsMock = DependencyMocker.GetSharedSettings(TestValues.Development);
            _tagBuilder = new TagBuilder(sharedSettingsMock.Object, _fileSystemMock.Object);

            // Act
            Func<Task> act = () => _tagBuilder.BuildStyleTagAsync(null!);

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
            sharedSettingsMock.VerifyNoOtherCalls();
            _fileSystemMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task BuildStyleTag_DevelopmentNoHttpClient_ShouldThrowArgumentNullException()
        {
            // Arrange
            var sharedSettingsMock = DependencyMocker.GetSharedSettings(TestValues.Development);
            _tagBuilder = new TagBuilder(sharedSettingsMock.Object, _fileSystemMock.Object);

            // Act
            Func<Task> act = () => _tagBuilder.BuildStyleTagAsync("InvalidBundle");

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
            sharedSettingsMock.VerifyGet(x => x.DevelopmentMode, Times.AtLeastOnce);
            sharedSettingsMock.VerifyGet(x => x.AssetsDirectoryPath, Times.Once);
            sharedSettingsMock.VerifyNoOtherCalls();
            _fileSystemMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task BuildStyleTag_Development_ShouldReturnStyleTag()
        {
            // Arrange
            var sharedSettingsMock = DependencyMocker.GetSharedSettings(TestValues.Development);
            var httpClientFactoryMock = DependencyMocker.GetHttpClientFactory(HttpStatusCode.OK, HttpClientResponse);
            _tagBuilder = new TagBuilder(sharedSettingsMock.Object, _fileSystemMock.Object, httpClientFactoryMock.Object);

            // Act
            var result = await _tagBuilder.BuildStyleTagAsync(Bundle);
            var result2 = await _tagBuilder.BuildStyleTagAsync(Bundle);

            // Assert
            VerifyStyleTag(result);
            VerifyStyleTag(result2);
            sharedSettingsMock.VerifyGet(x => x.DevelopmentMode, Times.AtLeastOnce);
            sharedSettingsMock.VerifyGet(x => x.AssetsDirectoryPath, Times.Exactly(2));
            sharedSettingsMock.VerifyNoOtherCalls();
            _fileSystemMock.VerifyNoOtherCalls();
            httpClientFactoryMock.Verify(x => x.CreateClient(It.IsAny<string>()), Times.Once);
            httpClientFactoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task BuildStyleTag_DevelopmentQueryString_ShouldReturnStyleTag()
        {
            // Arrange
            var sharedSettingsMock = DependencyMocker.GetSharedSettings(TestValues.Development);
            var httpClientFactoryMock = DependencyMocker.GetHttpClientFactory(HttpStatusCode.OK, HttpClientResponse);
            _tagBuilder = new TagBuilder(sharedSettingsMock.Object, _fileSystemMock.Object, httpClientFactoryMock.Object);
            var bundle = $"{Bundle}?v=123";

            // Act
            var result = await _tagBuilder.BuildStyleTagAsync(bundle);
            var result2 = await _tagBuilder.BuildStyleTagAsync(bundle);

            // Assert
            VerifyStyleTag(result);
            VerifyStyleTag(result2);
            sharedSettingsMock.VerifyGet(x => x.DevelopmentMode, Times.AtLeastOnce);
            sharedSettingsMock.VerifyGet(x => x.AssetsDirectoryPath, Times.Exactly(2));
            sharedSettingsMock.VerifyNoOtherCalls();
            _fileSystemMock.VerifyNoOtherCalls();
            httpClientFactoryMock.Verify(x => x.CreateClient(It.IsAny<string>()), Times.Once);
            httpClientFactoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task BuildStyleTag_Production_ShouldReturnStyleTag()
        {
            // Arrange
            var sharedSettingsMock = DependencyMocker.GetSharedSettings(TestValues.Production);
            _tagBuilder = new TagBuilder(sharedSettingsMock.Object, _fileSystemMock.Object);

            // Act
            var result = await _tagBuilder.BuildStyleTagAsync(Bundle);
            var result2 = await _tagBuilder.BuildStyleTagAsync(Bundle);

            // Assert
            VerifyStyleTag(result);
            VerifyStyleTag(result2);
            sharedSettingsMock.VerifyGet(x => x.DevelopmentMode, Times.AtLeastOnce);
            sharedSettingsMock.VerifyGet(x => x.AssetsDirectoryPath, Times.Once);
            sharedSettingsMock.VerifyNoOtherCalls();
            _fileSystemMock.Verify(x => x.File.ReadAllTextAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _fileSystemMock.VerifyNoOtherCalls();
        }

        private static void VerifyStyleTag(string result)
        {
            result.Should().Contain(HttpClientResponse)
                .And.StartWith("<style>")
                .And.EndWith("</style>");
        }

        private void VerifyScriptTag(string result, Mock<ISharedSettings> sharedSettingsMock)
        {
            result.Should().StartWith("<script ")
                .And.EndWith("</script>")
                .And.Contain($"src=\"{ValidBundleResult}\"");
            sharedSettingsMock.VerifyGet(x => x.DevelopmentMode, Times.Exactly(2));
            sharedSettingsMock.VerifyGet(x => x.AssetsWebPath, Times.Once);
            sharedSettingsMock.VerifyNoOtherCalls();
            _fileSystemMock.VerifyNoOtherCalls();
        }
    }
}
