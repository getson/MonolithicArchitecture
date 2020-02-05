using App.Core;
using App.WebApi.Extensions;
using BinaryOrigin.SeedWork.Core;

using BinaryOrigin.SeedWork.Messages;
using BinaryOrigin.SeedWork.Persistence.Ef;
using BinaryOrigin.SeedWork.Persistence.SqlServer;
using BinaryOrigin.SeedWork.WebApi;
using BinaryOrigin.SeedWork.WebApi.Extensions;
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
            services.AddHttpContextAccesor();
            services.AddCors();
            services.AddControllers()
                    .AddNewtonsoftJson()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddAppSwagger();
            services.AddDefaultEfSecondLevelCache();

            // add custom services
            engine.AddAutoMapper();
            engine.AddDbServices(configuration);
            engine.AddInMemoryBus();
            engine.AddFluentValidation();
            engine.AddHandlers();
            engine.AddDefaultDecorators();
            engine.AddDefaultPagination();
        }

        /// <inheritdoc />
        public void Configure(IApplicationBuilder application, IConfiguration configuration)
        {
            application.UseAppExceptionHandler();

            application.UseRouting();

            application.UseEndpoints(cfg =>
            {
                cfg.MapControllers();
            });

            application.UseAppSwagger();
        }
    }
}