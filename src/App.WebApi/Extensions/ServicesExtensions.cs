using App.Core;
using App.Infrastructure.Persistence.SqlServer.Context;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Persistence.Ef;
using BinaryOrigin.SeedWork.WebApi;
using BinaryOrigin.SeedWork.WebApi.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.WebApi.Extensions
{
    public static class ServicesExtensions
    {
        /// <summary>
        /// Add entity framework services
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="configuration"></param>
        public static void AddDbServices(this IEngine engine, IConfiguration configuration)
        {
            var connectionString = configuration["Db:ConnectionString"];
            var dbType = configuration["Db:Type"];

            if (dbType == "InMemory")
            {
                engine.AddInMemoryDbContext();
            }
            else
            {
                // 1
                engine.AddDbContext<AppDbContext>(connectionString);
                //var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                //optionsBuilder.UseSqlServer(connectionString);
                // 2 - engine.AddDbContext(() => new AppDbContext(optionsBuilder.Options));

                // 3 -  engine.AddDbContext<AppDbContext>(optionsBuilder.Options);
            }
            engine.AddSqlServerDbExceptionParser(new DbErrorMessagesConfiguration
            {
                UniqueErrorTemplate = ErrorMessages.GenericUniqueError,
                CombinationUniqueErrorTemplate = ErrorMessages.GenericCombinationUniqueError
            });
            engine.AddRepositories();
        }

        public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
        
        {
            var authConfig = configuration.GetSection("Auth");

            if (configuration.GetValue<bool>("IsTesting"))
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Test Scheme";
                    options.DefaultChallengeScheme = "Test Scheme";
                }).AddTestAuth(options =>
                {
                    options.Scopes = ReflectionHelper.GetConstants<Scopes,string>();
                    options.Authority = authConfig["Authority"];
                });
            }
            else
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    options.Authority = authConfig["Authority"];
                    options.RequireHttpsMetadata = authConfig.GetValue<bool>("RequireHttps");
                    options.Audience = authConfig["Audience"];
                });
            }
            services.AddAuthorization();
            services.AddSingleton<IAuthorizationPolicyProvider, ScopeAuthorizationPolicyProvider>();
            services.AddTransient<IClaimsTransformation, UserClaimsExtender>(x=>new UserClaimsExtender(authConfig["Authority"]));
        }
        public static void UseAppExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}