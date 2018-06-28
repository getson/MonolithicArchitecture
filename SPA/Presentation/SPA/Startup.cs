using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Configuration;
using MyApp.Core.Domain.Services.Logging;
using MyApp.Core.Infrastructure;
using MyApp.Infrastructure.Common;
using MyApp.Infrastructure.Data;
using SPA.Infrastructure;

namespace SPA
{/// <summary>
/// startup class
/// </summary>
    public class Startup
    {

        public IConfigurationRoot Configuration { get; }
        /// <inheritdoc />
        public Startup(IHostingEnvironment environment)
        {
            //Configuration = configuration;
            //create configuration
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

            //add MyAppConfig configuration parameters
            services.AddConfiguration<MyAppConfig>(Configuration.GetSection("MyApp"));
            //add hosting configuration parameters
            services.AddConfiguration<HostingConfig>(Configuration.GetSection("Hosting"));

            //add accessor to HttpContext
            services.AddHttpContextAccessor();

            //create, initialize and configure the engine
            var engine = EngineContext.Create();
            var (rootPath, contentPath) = GetPaths(services);
            engine.Initialize(services, new MyAppFileProvider(rootPath, contentPath));

            var serviceProvider = engine.ConfigureServices(services, Configuration);

            if (DataSettingsManager.DatabaseIsInstalled)
            {
                //log application start
                EngineContext.Current.Resolve<ILogger>().Information("Application started", null);
            }
            return serviceProvider;
        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            EngineContext.Current.ConfigureRequestPipeline(app);

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });



        }

        #region Helper
        private static (string rootPath, string contentPath) GetPaths(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var hostingEnvironment = provider.GetRequiredService<IHostingEnvironment>();
            var webRootPath = File.Exists(hostingEnvironment.WebRootPath) ? Path.GetDirectoryName(hostingEnvironment.WebRootPath) : hostingEnvironment.WebRootPath;

            return (webRootPath, hostingEnvironment.ContentRootPath);
        }
        #endregion
    }
}
