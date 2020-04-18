using App.Core;
using App.WebApi.Extensions;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Messages;
using BinaryOrigin.SeedWork.Persistence.Ef;
using BinaryOrigin.SeedWork.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace App.WebApi
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IWebHostEnvironment environment)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            
            services.Configure<AuthConfig>(Configuration.GetSection(nameof(AuthConfig)));
            services.Configure<DbConfig>(Configuration.GetSection(nameof(DbConfig)));

            var provider = services.BuildServiceProvider();

            var authConfig = provider.GetRequiredService<IOptionsMonitor<AuthConfig>>().CurrentValue;
            var dbConfig = provider.GetRequiredService<IOptionsMonitor<DbConfig>>().CurrentValue;

            //add framework services
            services.AddCors();
            services.AddControllers()
                    .AddNewtonsoftJson()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddAppSwagger();
            services.AddDefaultEfSecondLevelCache();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddAuth(authConfig);
            
            var engine = (AppWebApiEngine)EngineContext.Create<AppWebApiEngine>();

            engine.Initialize(Configuration);

            // add custom services
            engine.AddAutoMapper();
            engine.AddDbServices(dbConfig);
            engine.AddInMemoryBus();
            engine.AddFluentValidation();
            engine.AddHandlers();
            engine.AddDefaultDecorators();
            engine.AddDefaultPagination(c =>
            {
                c.MaxPageSizeAllowed = 100;
            });
            return engine.ConfigureServices(services);
        }

        /// <summary>
        /// Configure the application HTTP request pipeline
        /// </summary>
        /// <param name="app"></param>
        public void Configure(IApplicationBuilder app)
        {
            app.UseAppExceptionHandler();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(cfg =>
            {
                cfg.MapControllers();
            });
            app.ApplicationServices.GetRequiredService<IDbContext>()
                .CreateDatabase();
            app.UseAppSwagger();
        }
    }
}