using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Abstractions.Infrastructure;
using MyApp.Core.Configuration;
using MyApp.Spa.Infrastructure;

namespace MyApp.Spa
{
    /// <inheritdoc />
    /// Startup class for configuring SPA services
    public class ApiStartup : IMyAppStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        public void ConfigureServices(IServiceCollection services, MyAppConfig configuration)
        {
            services.AddMyApiVersioning();
            services.AddMySwagger();
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            application.UseMySwagger();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 2;

        #region Helper


        #endregion

    }
}
