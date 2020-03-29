// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.Data;
using IdentityServer4.Serivces;
using IdentityServer4.Test;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// DI extension methods for adding IdentityServer
    /// </summary>
    public static class IdentityServerServiceCollectionExtensions
    {
        /// <summary>
        /// Creates a builder.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddIdentityServerBuilder(this IServiceCollection services)
        {
            return new IdentityServerBuilder(services);
        }

        /// <summary>
        /// Adds IdentityServer.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services)
        {
            var builder = services.AddIdentityServerBuilder();
           
            services.AddDbContext<UMDbContext>
                 (options => options.UseMySql("server=api-blog-cgo-mysqldbserver.mysql.database.azure.com;port=3306;database=cgo_um;uid=AdminCgo@api-blog-cgo-mysqldbserver;password='Standar123.'"));

            services.AddScoped<IDbContext>(c => c.GetService<UMDbContext>());
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IUMStore, UMStore>();

            builder
                .AddRequiredPlatformServices()
                .AddCookieAuthentication()
                .AddCoreServices()
                .AddDefaultEndpoints()
                .AddPluggableServices()
                .AddValidators()
                .AddResponseGenerators()
                .AddDefaultSecretParsers()
                .AddDefaultSecretValidators();

            // provide default in-memory implementation, not suitable for most production scenarios

            builder.AddInMemoryPersistedGrants();

            return builder;
        }

        /// <summary>
        /// Adds IdentityServer.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="setupAction">The setup action.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services, Action<IdentityServerOptions> setupAction)
        {
            services.Configure(setupAction);
            return services.AddIdentityServer();
        }

        /// <summary>
        /// Adds the IdentityServer.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<IdentityServerOptions>(configuration);
            return services.AddIdentityServer();
        }

        /// <summary>
        /// Configures the OpenIdConnect handlers to persist the state parameter into the server-side IDistributedCache.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="schemes">The schemes to configure. If none provided, then all OpenIdConnect schemes will use the cache.</param>
        public static IServiceCollection AddOidcStateDataFormatterCache(this IServiceCollection services, params string[] schemes)
        {
            services.AddSingleton<IPostConfigureOptions<OpenIdConnectOptions>>(
                svcs => new ConfigureOpenIdConnectOptions(
                    schemes,
                    svcs.GetRequiredService<IHttpContextAccessor>())
            );

            return services;
        }
    }
}