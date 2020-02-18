using App.WebApi.Extensions;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Messages;
using BinaryOrigin.SeedWork.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.WebApi
{
    /// <inheritdoc />
    public class WebApiStartup : IWebAppStartup
    {
        /// <inheritdoc />
        public int Order => 1;

        /// <inheritdoc />
        public void ConfigureServices(IServiceCollection services, IEngine engine, IConfiguration configuration)
        {
            //add framework services
            services.AddCors();
            services.AddControllers()
                    .AddNewtonsoftJson()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddAppSwagger();
            services.AddDefaultEfSecondLevelCache();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddAuth(configuration);

            // add custom services
            engine.AddAutoMapper();
            engine.AddDbServices(configuration);
            engine.AddInMemoryBus();
            engine.AddFluentValidation();
            engine.AddHandlers();
            engine.AddDefaultDecorators();
            engine.AddDefaultPagination(c =>
            {
                c.MaxPageSizeAllowed = 100;
            });
        }

        /// <inheritdoc />
        public void Configure(IApplicationBuilder application, IConfiguration configuration)
        {
            application.UseAppExceptionHandler();

            application.UseAuthentication();

            application.UseRouting();

            application.UseAuthorization();

            application.UseEndpoints(cfg =>
            {
                cfg.MapControllers();
            });

            application.UseAppSwagger();
        }
    }
}