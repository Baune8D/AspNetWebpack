// <copyright file="AssertionExtensions.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.AspNetCore.Html;

namespace AspNetWebpack.AssetHelpers.Testing
{
    /// <summary>
    /// Helper functions for asserting asset service results.
    /// </summary>
    public static class AssertionExtensions
    {
        /// <summary>
        /// Validates that the result is a valid script tag and that it contains the correct bundle and script load mode.
        /// </summary>
        /// <param name="result">The result to validate.</param>
        /// <param name="bundle">The result bundle to be used in the element.</param>
        /// <param name="scriptLoad">The chosen script load mode.</param>
        /// <exception cref="ArgumentOutOfRangeException">If script load enum is invalid.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062", Justification = "Result is asserted as not null")]
        public static void ShouldBeScriptTag(this HtmlString result, string bundle, ScriptLoad scriptLoad = ScriptLoad.Normal)
        {
            result.Should().NotBeNull();
            result.Value.Should().StartWith("<script");
            result.Value.Should().EndWith("</script>");
            result.Value.Should().Contain(bundle);

            switch (scriptLoad)
            {
                case ScriptLoad.Normal:
                    break;
                case ScriptLoad.Async:
                    result.Value.Should().Contain(" async>");
                    break;
                case ScriptLoad.Defer:
                    result.Value.Should().Contain(" defer>");
                    break;
                case ScriptLoad.AsyncDefer:
                    result.Value.Should().Contain(" async defer>");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(scriptLoad), scriptLoad, null);
            }
        }

        /// <summary>
        /// Validates that the result is a valid link tag and that it contains the correct bundle.
        /// </summary>
        /// <param name="result">The result to validate.</param>
        /// <param name="bundle">The result bundle to be used in the element.</param>
        [SuppressMessage("Microsoft.Design", "CA1062", Justification = "Result is asserted as not null")]
        public static void ShouldBeLinkTag(this HtmlString result, string bundle)
        {
            result.Should().NotBeNull();
            result.Value.Should().StartWith("<link");
            result.Value.Should().EndWith(" />");
            result.Value.Should().Contain(bundle);
        }

        /// <summary>
        /// Validates that the result is a valid style tag.
        /// </summary>
        /// <param name="result">The result to validate.</param>
        [SuppressMessage("Microsoft.Design", "CA1062", Justification = "Result is asserted as not null")]
        public static void ShouldBeStyleTag(this HtmlString result)
        {
            result.Should().NotBeNull();
            result.Value.Should().StartWith("<style>");
            result.Value.Should().EndWith("</style>");
            result.Value!.Length.Should().BeGreaterThan(15);
        }
    }
}
