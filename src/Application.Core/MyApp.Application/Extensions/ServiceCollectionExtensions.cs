using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Infrastructure.Data;
using Newtonsoft.Json;
using StackExchange.Profiling.Storage;

namespace MyApp.Application.Extensions
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

        public static void AddMyHttpContextAccesor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
        /// <summary>
        /// Register base object context
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddMyAppObjectContext(this IServiceCollection services)
        {
            services.AddDbContext<MyAppObjectContext>(optionsBuilder =>
            {
                var dataSettings = DataSettingsManager.Instance.LoadSettings();
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
            if (!DataSettingsManager.Instance.DatabaseIsInstalled)
                return;

            services.AddMiniProfiler(miniProfilerOptions =>
            {
                //use memory cache provider for storing each result
                ((MemoryCacheStorage)miniProfilerOptions.Storage).CacheDuration = TimeSpan.FromMinutes(60);
            }).AddEntityFramework();
        }
    }
}