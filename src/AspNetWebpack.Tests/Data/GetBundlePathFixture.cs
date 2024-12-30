// <copyright file="GetBundlePathFixture.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System.ComponentModel;
using System.Threading.Tasks;
using FluentAssertions;

namespace AspNetWebpack.Tests.Data;

/// <summary>
/// Fixture for testing GetBundlePathAsync function in AssetService.
/// </summary>
public class GetBundlePathFixture : AssetServiceBaseFixture
{
    /// <summary>
    /// Valid bundle name with extension.
    /// </summary>
    public const string ValidBundleWithExtension = $"{ValidBundleWithoutExtension}.js";

    /// <summary>
    /// Invalid bundle name with extension.
    /// </summary>
    public const string InvalidBundleWithExtension = $"{InvalidBundle}.js";

    /// <summary>
    /// Initializes a new instance of the <see cref="GetBundlePathFixture"/> class.
    /// </summary>
    /// <param name="bundle">The bundle name param to be used in GetBundlePathAsync.</param>
    /// <param name="fileType">The file type param to be used in GetBundlePathAsync.</param>
    public GetBundlePathFixture(string bundle, FileType? fileType = null)
        : base(ValidBundleWithExtension)
    {
        Bundle = bundle;
        FileType = fileType;
        SetupGetFromManifest();
    }

    private string Bundle { get; }

    private FileType? FileType { get; }

    private string BundleWithCssExtension => $"{Bundle}.css";

    private string BundleWithJsExtension => $"{Bundle}.js";

    /// <summary>
    /// Calls GetBundlePathAsync with provided parameters.
    /// </summary>
    /// <returns>The result of GetBundlePathAsync.</returns>
    public async Task<string?> GetBundlePathAsync()
    {
        return await AssetService
            .GetBundlePathAsync(Bundle, FileType)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Verify that GetBundlePathAsync was called with an empty string.
    /// </summary>
    /// <param name="result">The result to assert.</param>
    public void VerifyEmpty(string? result)
    {
        result.Should().BeNull();
        VerifyDependencies();
        VerifyNoOtherCalls();
    }

    /// <summary>
    /// Verify that GetBundlePathAsync was called with an invalid bundle.
    /// </summary>
    /// <param name="result">The result to assert.</param>
    public void VerifyNonExisting(string? result)
    {
        result.Should().BeNull();
        VerifyDependencies();
        VerifyGetFromManifest();
        VerifyNoOtherCalls();
    }

    /// <summary>
    /// Verify that GetBundlePathAsync was called with a valid bundle.
    /// </summary>
    /// <param name="result">The result to assert.</param>
    public void VerifyExisting(string? result)
    {
        result.Should().NotBeNull()
            .And.Be(ValidBundleResultPath);
        VerifyDependencies();
        VerifyGetFromManifest();
        VerifyNoOtherCalls();
    }

    private void VerifyGetFromManifest()
    {
        switch (FileType)
        {
            case AspNetWebpack.FileType.CSS:
                VerifyGetFromManifest(BundleWithCssExtension);
                break;
            case AspNetWebpack.FileType.JS:
                VerifyGetFromManifest(BundleWithJsExtension);
                break;
            case null:
                VerifyGetFromManifest(Bundle);
                break;
            default:
                throw new InvalidEnumArgumentException(nameof(FileType), (int)FileType, typeof(FileType));
        }
    }
}
