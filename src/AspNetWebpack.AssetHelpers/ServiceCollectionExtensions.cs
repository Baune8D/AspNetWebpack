// <copyright file="ServiceCollectionExtensions.cs" company="Morten Larsen">
// Copyright (c) Morten Larsen. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetWebpack.AssetHelpers
{
    /// <summary>
    /// Extensions methods for IServiceCollection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds AssetService and necessary dependencies.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="env">The hosting environment.</param>
        /// <returns>The modified service collection.</returns>
        /// <exception cref="ArgumentNullException">If services or configuration is null.</exception>
        public static IServiceCollection AddAssetHelpers(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (env.IsDevelopment())
            {
                services.AddHttpClient();
            }

            services.Configure<WebpackOptions>(configuration.GetSection("Webpack"));
            services.AddSingleton<IAssetService, AssetService>();

            return services;
        }
    }
}
