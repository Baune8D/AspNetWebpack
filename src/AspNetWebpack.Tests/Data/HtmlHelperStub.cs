// <copyright file="HtmlHelperStub.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace AspNetWebpack.Tests.Data
{
    /// <summary>
    /// Stub class for testing view context.
    /// </summary>
    public class HtmlHelperStub : IHtmlHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlHelperStub"/> class.
        /// </summary>
        /// <param name="view">The stubbed view.</param>
        public HtmlHelperStub(string view)
        {
            ViewContext = new ViewContext
            {
                View = new ViewStub(view),
            };
        }

        /// <inheritdoc />
        public Html5DateRenderingMode Html5DateRenderingMode { get; set; }

        /// <inheritdoc />
        public string IdAttributeDotReplacement { get; } = null!;

        /// <inheritdoc />
        public IModelMetadataProvider MetadataProvider { get; } = null!;

        /// <inheritdoc />
        public ITempDataDictionary TempData { get; } = null!;

        /// <inheritdoc />
        public UrlEncoder UrlEncoder { get; } = null!;

        /// <inheritdoc />
        public dynamic ViewBag { get; } = null!;

        /// <inheritdoc />
        public ViewContext ViewContext { get; }

        /// <inheritdoc />
        public ViewDataDictionary ViewData { get; } = null!;

        /// <inheritdoc />
        public IHtmlContent ActionLink(string linkText, string actionName, string controllerName, string protocol, string hostname, string fragment, object routeValues, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IHtmlContent AntiForgeryToken()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public MvcForm BeginForm(string actionName, string controllerName, object routeValues, FormMethod method, bool? antiforgery, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public MvcForm BeginRouteForm(string routeName, object routeValues, FormMethod method, bool? antiforgery, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IHtmlContent CheckBox(string expression, bool? isChecked, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IHtmlContent Display(string expression, string templateName, string htmlFieldName, object additionalViewData)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string DisplayName(string expression)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string DisplayText(string expression)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IHtmlContent DropDownList(string expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IHtmlContent Editor(string expression, string templateName, string htmlFieldName, object additionalViewData)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string Encode(object value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string Encode(string value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void EndForm()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string FormatValue(object value, string format)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string GenerateIdFromName(string fullName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IEnumerable<SelectListItem> GetEnumSelectList(Type enumType)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IEnumerable<SelectListItem> GetEnumSelectList<TEnum>()
            where TEnum : struct
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IHtmlContent Hidden(string expression, object value, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string Id(string expression)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IHtmlContent Label(string expression, string labelText, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IHtmlContent ListBox(string expression, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string Name(string expression)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IHtmlContent> PartialAsync(string partialViewName, object model, ViewDataDictionary viewData)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IHtmlContent Password(string expression, object value, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IHtmlContent RadioButton(string expression, object value, bool? isChecked, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IHtmlContent Raw(object value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IHtmlContent Raw(string value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task RenderPartialAsync(string partialViewName, object model, ViewDataDictionary viewData)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IHtmlContent RouteLink(string linkText, string routeName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IHtmlContent TextArea(string expression, string value, int rows, int columns, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IHtmlContent TextBox(string expression, object value, string format, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IHtmlContent ValidationMessage(string expression, string message, object htmlAttributes, string tag)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IHtmlContent ValidationSummary(bool excludePropertyErrors, string message, object htmlAttributes, string tag)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string Value(string expression, string format)
        {
            throw new NotImplementedException();
        }
    }
}
