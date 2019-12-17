using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Infrastructure;
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

        public static void AddMyHttpContextAccesor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
        /// <summary>
        /// Register base object context
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddDbContext(this IServiceCollection services)
        {
            var dataSettings = DataSettingsManager.Instance.LoadSettings();
            if (!dataSettings?.IsValid ?? true)
                return;

            var optionsBuilder = new DbContextOptionsBuilder<MyAppObjectContext>();
            optionsBuilder.UseSqlServer(dataSettings.DataConnectionString);

            services.AddScoped<IDbContext, MyAppObjectContext>(x =>
                new MyAppObjectContext(optionsBuilder.Options));

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