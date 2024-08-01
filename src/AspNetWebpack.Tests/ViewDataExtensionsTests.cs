// <copyright file="ViewDataExtensionsTests.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Xunit;

namespace AspNetWebpack.Tests
{
    public sealed class ViewDataExtensionsTests
    {
        [Fact]
        public void GetBundleName_Null_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => ((ViewDataDictionary)null!).GetBundleName();

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void GetBundleName_Null_ShouldReturnNull()
        {
            // Arrange
            var viewData = new ViewDataDictionary<dynamic>(new EmptyModelMetadataProvider(), new ModelStateDictionary());

            // Act
            var result = viewData.GetBundleName();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetBundleName_Int_ShouldReturnNull()
        {
            // Arrange
            var viewData = new ViewDataDictionary<dynamic>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                { "Bundle", 123 },
            };

            // Act
            var result = viewData.GetBundleName();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetBundleName_Bundle_ShouldReturnBundleName()
        {
            // Arrange
            const string bundle = "TestBundle";
            var viewData = new ViewDataDictionary<dynamic>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                { "Bundle", bundle },
            };

            // Act
            var result = viewData.GetBundleName();

            // Assert
            result.Should().Be(bundle);
        }

        [Fact]
        public void GetBundleName_RazorPageBundle_ShouldReturnBundleName()
        {
            // Arrange
            const string bundle = "/Test/Bundle";
            var viewData = new ViewDataDictionary<dynamic>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                { "Bundle", bundle },
            };

            // Act
            var result = viewData.GetBundleName();

            // Assert
            result.Should().Be("Test_Bundle");
        }
    }
}
