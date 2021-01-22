// <copyright file="ViewStub.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace AspNetWebpack.AssetHelpers.Testing
{
    /// <summary>
    /// Stub class for testing view path.
    /// </summary>
    public class ViewStub : IView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewStub"/> class.
        /// </summary>
        /// <param name="view">A fixed view path.</param>
        public ViewStub(string view)
        {
            Path = view;
        }

        /// <inheritdoc />
        public string Path { get; }

        /// <inheritdoc />
        public Task RenderAsync(ViewContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
