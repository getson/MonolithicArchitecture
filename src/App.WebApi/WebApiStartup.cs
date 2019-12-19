using BinaryOrigin.SeedWork.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using App.WebApi.Extensions;
using BinaryOrigin.SeedWork.WebApi;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.WebApi.Extensions;
using BinaryOrigin.SeedWork.Persistence.SqlServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

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
            services.AddControllers(c =>
                    {
                        var policy = ScopePolicy.Create("openid", "api");
                        c.Filters.Add(new AuthorizeFilter(policy));
                    })
                    .AddNewtonsoftJson()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddAuthentication("Bearer")
                   .AddJwtBearer("Bearer", options =>
                   {
                       options.Authority = "http://localhost:5000";
                       options.RequireHttpsMetadata = false;
                       options.Audience = "myApp";
                   });
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
        public void Configure(IApplicationBuilder app, AppConfiguration configuration)
        {

            app.UseAppExceptionHandler();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(cfg =>
            {
                cfg.MapControllers()
                   .RequireAuthorization();
            });

            app.UseAppSwagger();
        }
    }
}