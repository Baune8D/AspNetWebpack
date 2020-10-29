using AspNetWebpack.AssetHelpers.Tests.Internal;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Xunit;

namespace AspNetWebpack.AssetHelpers.Tests
{
    public class HtmlHelperExtensionsTests
    {
        [Fact]
        public void GetBundleName_MvcView_ShouldReturnBundleName()
        {
            // Arrange
            IHtmlHelper htmlHelper = new HtmlHelperStub("/Views/Some/Page.cshtml");

            // Act
            var result = htmlHelper.GetBundleName();

            // Assert
            result.Should().Be("Some_Page");
        }

        [Fact]
        public void GetBundleName_AreaMvcView_ShouldReturnBundleName()
        {
            // Arrange
            IHtmlHelper htmlHelper = new HtmlHelperStub("/Areas/Test/Views/Some/Page.cshtml");

            // Act
            var result = htmlHelper.GetBundleName();

            // Assert
            result.Should().Be("Test_Some_Page");
        }

        [Fact]
        public void GetBundleName_RazorPage_ShouldReturnBundleName()
        {
            // Arrange
            IHtmlHelper htmlHelper = new HtmlHelperStub("/Pages/Some/Page.cshtml");

            // Act
            var result = htmlHelper.GetBundleName();

            // Assert
            result.Should().Be("Some_Page");
        }

        [Fact]
        public void GetBundleName_AreaRazorPage_ShouldReturnBundleName()
        {
            // Arrange
            IHtmlHelper htmlHelper = new HtmlHelperStub("/Areas/Test/Pages/Some/Page.cshtml");

            // Act
            var result = htmlHelper.GetBundleName();

            // Assert
            result.Should().Be("Test_Some_Page");
        }
    }
}
