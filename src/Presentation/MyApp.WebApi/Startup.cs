using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Common;
using MyApp.Core.Configuration;
using MyApp.Core.Infrastructure;
using MyApp.Infrastructure.Data;
using MyApp.Infrastructure.FileSystem;
using System;
using System.IO;

namespace MyApp.WebApi
{/// <summary>
/// startup class
/// </summary>
    public class Startup
    {

        public IConfigurationRoot Configuration { get; }
        /// <inheritdoc />
        public Startup(IWebHostEnvironment environment)
        {

            Configuration = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            //create, initialize and configure the engine
            var (rootPath, contentPath) = GetPaths(services);

            var myAppConfig = new MyAppConfig();
            Configuration.GetSection("MyApp").Bind(myAppConfig);
            services.AddSingleton(myAppConfig);

            DefaultFileProvider.Instance = new MyAppFileProvider(rootPath, contentPath);

            //create, initialize and configure the engine
            var engine = EngineContext.Create();
            engine.Initialize(services, DefaultFileProvider.Instance, myAppConfig);

            return engine.ConfigureServices(services, myAppConfig);
        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName =="dev")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            EngineContext.Current.ConfigureRequestPipeline(app);

        }

        #region Helper
        private static (string rootPath, string contentPath) GetPaths(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var hostingEnvironment = provider.GetRequiredService<IWebHostEnvironment>();
            var webRootPath = File.Exists(hostingEnvironment.WebRootPath) ? Path.GetDirectoryName(hostingEnvironment.WebRootPath) : hostingEnvironment.WebRootPath;

            return (webRootPath, hostingEnvironment.ContentRootPath);
        }
        #endregion
    }
}
