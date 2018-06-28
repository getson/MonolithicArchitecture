using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Configuration;
using MyApp.Core.Domain.Services.Logging;
using MyApp.Core.Infrastructure;
using MyApp.Infrastructure.Common;
using MyApp.Infrastructure.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Profiling.Storage;

namespace MyApp.Web.Framework.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Add and configure MVC for the application
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <returns>A builder for configuring MVC services</returns>
        public static IMvcBuilder AddMyAppMvc(this IServiceCollection services)
        {
            //add basic MVC feature
            var mvcBuilder = services.AddMvc();

            mvcBuilder.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //TODO tocheck getson this option
          
            mvcBuilder.AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //MVC now serializes JSON with camel case names by default, use this code to avoid it
                //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            return mvcBuilder;
        }
        /// <summary>
        /// Register base object context
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddMyAppObjectContext(this IServiceCollection services)
        {
            services.AddDbContext<MyAppObjectContext>(optionsBuilder =>
            {
                var dataSettings = DataSettingsManager.LoadSettings();
                if (!dataSettings?.IsValid ?? true)
                    return;

                optionsBuilder.UseLazyLoadingProxies()
                              .UseSqlServer(dataSettings.DataConnectionString);
            });
        }

        /// <summary>
        /// Add and configure MiniProfiler service
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddMyAppMiniProfiler(this IServiceCollection services)
        {
            //whether database is already installed
            if (!DataSettingsManager.DatabaseIsInstalled)
                return;

            services.AddMiniProfiler(miniProfilerOptions =>
            {
                //use memory cache provider for storing each result
                ((MemoryCacheStorage)miniProfilerOptions.Storage).CacheDuration = TimeSpan.FromMinutes(60);
            }).AddEntityFramework();
        }
    }
}