// <copyright file="HttpMessageHandlerStub.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetWebpack.AssetHelpers.Testing
{
    /// <inheritdoc />
    public class HttpMessageHandlerStub : HttpMessageHandler
    {
        private readonly HttpStatusCode _httpStatusCode;
        private readonly string _content;
        private readonly bool _json;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMessageHandlerStub"/> class.
        /// </summary>
        /// <param name="httpStatusCode">The status code to stub for this request.</param>
        /// <param name="content">The content to stub for this request.</param>
        /// <param name="json">If the content is json.</param>
        public HttpMessageHandlerStub(HttpStatusCode httpStatusCode, string content, bool json)
        {
            _httpStatusCode = httpStatusCode;
            _content = content;
            _json = json;
        }

        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = _httpStatusCode,
                Content = _json
                    ? new StringContent(_content, Encoding.UTF8, "application/json")
                    : new StringContent(_content),
            });
        }
    }
}
