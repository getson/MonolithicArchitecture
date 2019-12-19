﻿using BinaryOrigin.SeedWork.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using App.WebApi.Extensions;
using BinaryOrigin.SeedWork.WebApi;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.WebApi.Extensions;
using BinaryOrigin.SeedWork.Persistence.SqlServer;
using Microsoft.AspNetCore.Mvc;

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
                engine.AddSqlDbContext();
            }
            engine.AddInMemoryBus();
            engine.AddRepositories();
            engine.AddCommandHandlers();


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