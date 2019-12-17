using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Abstractions.Infrastructure;
using MyApp.Core.Configuration;
using MyApp.WebApi.Infrastructure;
using MyApp.Application.Extensions;
using Newtonsoft.Json.Serialization;

namespace MyApp.WebApi
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

            //add httpContext accessor to HttpContext
            services.AddMyHttpContextAccesor();
            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    });
            services.AddMyApiVersioning();
            services.AddMySwagger();
            //add object context
            services.AddDbContext();
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="app">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(cfg =>
            {
                cfg.MapDefaultControllerRoute();
            });
            app.UseMySwagger();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 2;

    }
}
