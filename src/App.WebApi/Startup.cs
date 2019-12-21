using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Core.Common;
using BinaryOrigin.SeedWork.Core.Configuration;
using BinaryOrigin.SeedWork.Infrastructure;
using BinaryOrigin.SeedWork.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace App.WebApi
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        public IConfigurationRoot Configuration { get; }

        public Startup(IWebHostEnvironment environment)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            _environment = environment;
        }

        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //create instance of config
            var appConfig = new AppConfiguration();

            //bind it to the appropriate section of configuration
            Configuration.GetSection("App").Bind(appConfig);
            appConfig.Environment = _environment.EnvironmentName;
            //and register it as a service
            services.AddSingleton(appConfig);

            var engine = (AppWebApiEngine)EngineContext.Create<AppWebApiEngine>();

            DefaultFileProvider.Instance = new AppFileProvider(AppContext.BaseDirectory);

            engine.Initialize(services, DefaultFileProvider.Instance, appConfig);
            var serviceProvider = engine.ConfigureServices(services, appConfig);

            return serviceProvider;
        }

        /// <summary>
        /// Configure the application HTTP request pipeline
        /// </summary>
        /// <param name="application"></param>
        public void Configure(IApplicationBuilder application)
        {
            ((AppWebApiEngine)EngineContext.Current).ConfigureRequestPipeline(application);
        }
    }
}