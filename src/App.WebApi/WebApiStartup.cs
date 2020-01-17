using App.Core;
using App.WebApi.Extensions;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Core.Configuration;
using BinaryOrigin.SeedWork.Messages;
using BinaryOrigin.SeedWork.Persistence.Ef;
using BinaryOrigin.SeedWork.Persistence.SqlServer;
using BinaryOrigin.SeedWork.WebApi;
using BinaryOrigin.SeedWork.WebApi.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace App.WebApi
{
    /// <inheritdoc />
    public class WebApiStartup : IWebAppStartup
    {
        /// <inheritdoc />
        public int Order => 2;

        /// <inheritdoc />
        public void ConfigureServices(IServiceCollection services, IEngine engine, AppConfiguration appConfiguration)
        {
            //add accessor to HttpContext
            services.AddHttpContextAccesor();
            services.AddCors();
            services.AddControllers()
                    .AddNewtonsoftJson()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddAppSwagger();

            engine.AddAutoMapper();
            if (appConfiguration.Environment == "Testing")
            {
                engine.AddInMemoryDbContext();
            }
            else
            {
                engine.AddDefaultSqlDbContext();
            }
            engine.AddSqlServerDbExceptionParser(new DbErrorMessagesConfiguration
            {
                UniqueErrorTemplate = ErrorMessages.GenericUniqueError,
                CombinationUniqueErrorTemplate = ErrorMessages.GenericCombinationUniqueError
            });
            engine.AddInMemoryBus();
            engine.AddFluentValidation();
            engine.AddHandlers();
            engine.AddDefaultDecorators();
            engine.AddRepositories();
            services.AddEfSecondLevelCache();
        }

        /// <inheritdoc />
        public void Configure(IApplicationBuilder application, AppConfiguration configuration)
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