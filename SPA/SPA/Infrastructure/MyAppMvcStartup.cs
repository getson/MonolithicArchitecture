using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Infrastructure;
using SPA.Infrastructure.Extensions;

namespace SPA.Infrastructure
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
            //add MiniProfiler services
            services.AddMyAppMiniProfiler();

            //add and configure MVC feature
            services.AddMyAppMvc();

            //add custom redirect result executor
            //services.AddMyAppRedirectResultExecutor();
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            //add MiniProfiler
            application.UseMiniProfiler();

            //MVC routing
            application.UseMyAppMvc();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order
        {
            //MVC should be loaded last
            get { return 1000; }
        }
    }
}