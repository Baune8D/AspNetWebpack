using System;
using Microsoft.AspNetCore.Html;
using Shouldly;

namespace AspNetWebpack.AssetHelpers.Tests.Internal
{
    public static class AssertionExtensions
    {
        public static void ShouldBeScriptTag(this HtmlString result, string bundle, ScriptLoad scriptLoad = ScriptLoad.Normal)
        {
            result.ShouldNotBeNull();
            result.Value.ShouldStartWith("<script");
            result.Value.ShouldEndWith("</script>");
            result.Value.ShouldContain(bundle);

            switch (scriptLoad)
            {
                case ScriptLoad.Normal:
                    break;
                case ScriptLoad.Async:
                    result.Value.ShouldContain(" async>");
                    break;
                case ScriptLoad.Defer:
                    result.Value.ShouldContain(" defer>");
                    break;
                case ScriptLoad.AsyncDefer:
                    result.Value.ShouldContain(" async defer>");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(scriptLoad), scriptLoad, null);
            }
        }

        public static void ShouldBeLinkTag(this HtmlString result, string bundle)
        {
            result.ShouldNotBeNull();
            result.Value.ShouldStartWith("<link");
            result.Value.ShouldEndWith(" />");
            result.Value.ShouldContain(bundle);
        }

        public static void ShouldBeStyleTag(this HtmlString result)
        {
            result.ShouldNotBeNull();
            result.Value.ShouldStartWith("<style>");
            result.Value.ShouldEndWith("</style>");
            result.Value.Length.ShouldBeGreaterThan(15);
        }
    }
}
