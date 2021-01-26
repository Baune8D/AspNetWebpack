// <copyright file="HttpMessageHandlerStyleStub.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetWebpack.AssetHelpers.Testing
{
    /// <inheritdoc />
    public class HttpMessageHandlerStyleStub : HttpMessageHandler
    {
        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("CSS Content"),
            });
        }
    }
}
