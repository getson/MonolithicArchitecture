using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Abstractions.Infrastructure;
using MyApp.Core.Configuration;
using MyApp.Spa.Infrastructure;

namespace MyApp.Spa
{
    /// <inheritdoc />
    /// Startup class for configuring SPA services
    public class SpaStartup : IMyAppStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        public void ConfigureServices(IServiceCollection services, MyAppConfig configuration)
        {
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(config =>
            {
                config.RootPath = "ClientApp/dist";
            });
            services.AddMyApiVersioning();
            services.AddMySwagger();
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            application.UseSpaStaticFiles();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            //application.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            //application.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //});
            application.UseMySwagger();

        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 200;

        #region Helper


        #endregion

    }
}
