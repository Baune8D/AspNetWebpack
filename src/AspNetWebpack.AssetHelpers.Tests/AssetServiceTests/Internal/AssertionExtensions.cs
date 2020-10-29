using System;
using FluentAssertions;
using Microsoft.AspNetCore.Html;

namespace AspNetWebpack.AssetHelpers.Tests.AssetServiceTests.Internal
{
    public static class AssertionExtensions
    {
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

        public static void ShouldBeLinkTag(this HtmlString result, string bundle)
        {
            result.Should().NotBeNull();
            result.Value.Should().StartWith("<link");
            result.Value.Should().EndWith(" />");
            result.Value.Should().Contain(bundle);
        }

        public static void ShouldBeStyleTag(this HtmlString result)
        {
            result.Should().NotBeNull();
            result.Value.Should().StartWith("<style>");
            result.Value.Should().EndWith("</style>");
            result.Value.Length.Should().BeGreaterThan(15);
        }
    }
}
