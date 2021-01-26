// <copyright file="HttpMessageHandlerManifestStub.cs" company="Morten Larsen">
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
    public class HttpMessageHandlerManifestStub : HttpMessageHandler
    {
        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = $"{{\"{TestValues.JsonBundle}\": \"{TestValues.JsonResultBundle}\"}}";

            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(response, Encoding.UTF8, "application/json"),
            });
        }
    }
}
