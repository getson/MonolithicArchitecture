﻿using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
using MyApp.Core.Configuration;
using MyApp.Core.Domain.Services.Logging;
using MyApp.Core.Infrastructure;
using MyApp.Core.Interfaces.Infrastructure;
using MyApp.Infrastructure.Common;
using MyApp.Infrastructure.Data;
using MyApp.Web.Framework.Infrastructure.Extensions;
using MyApp.Web.Framework.Routing;

namespace MyApp.Web.Framework.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring MVC on application startup
    /// </summary>
    public class MyAppMvcStartup : IMyAppStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {

            //add object context
            services.AddMyAppObjectContext();

            //add EF services
            services.AddEntityFrameworkSqlServer();
            services.AddEntityFrameworkProxies();

            //compression
            services.AddResponseCompression();

            //add options feature
            services.AddOptions();

            //add memory cache
            services.AddMemoryCache();

            //add distributed memory cache
            services.AddDistributedMemoryCache();

            //add localization
            services.AddLocalization();

            //add MiniProfiler services
            services.AddMyAppMiniProfiler();

            //add and configure MVC feature
            services.AddMyAppMvc();

        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {

            //compression
            if (EngineContext.Current.Configuration.UseResponseCompression)
            {
                //gzip by default
                application.UseResponseCompression();
            }


            //plugins
            var staticFileOptions = new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(EngineContext.Current.FileProvider.MapPath(@"Plugins")),
                RequestPath = new PathString("/Plugins"),
                OnPrepareResponse = ctx =>
                {
                    if (!string.IsNullOrEmpty(EngineContext.Current.Configuration.StaticFilesCacheControl))
                        ctx.Context.Response.Headers.Append(HeaderNames.CacheControl,
                            EngineContext.Current.Configuration.StaticFilesCacheControl);
                }
            };

            //whether database is installed
            if (DataSettingsManager.DatabaseIsInstalled)
            {
                var fileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();
                staticFileOptions.ContentTypeProvider = fileExtensionContentTypeProvider;

            }

            application.UseStaticFiles(staticFileOptions);

            //add MiniProfiler
            application.UseMiniProfiler();

            //MVC routing
            application.UseMvc(routeBuilder =>
            {
                //register all routes
                EngineContext.Current.Resolve<IRoutePublisher>().RegisterRoutes(routeBuilder);
            });

        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 100;

    }
}