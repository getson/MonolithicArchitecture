using App.Core;
using App.Infrastructure.Persistence.Context;
using App.WebApi.Infrastructure.Authorization;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Persistence.Ef;
using BinaryOrigin.SeedWork.WebApi;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace App.WebApi.Extensions
{
    public static class ServicesExtensions
    {
        /// <summary>
        /// Add entity framework services
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="dbConfig"></param>
        public static void AddDbServices(this IEngine engine, DbConfig dbConfig)
        {
            if (dbConfig.ProviderType == DataProviderType.InMemory)
            {
                engine.AddInMemoryDbContext();
            }
            else
            {
                engine.AddDbContext<AppDbContext>(dbConfig.ConnectionString);
            }

            engine.AddPgSqlDbExceptionParser(new DbErrorMessagesConfiguration
            {
                UniqueErrorTemplate = ErrorMessages.GenericUniqueError,
                CombinationUniqueErrorTemplate = ErrorMessages.GenericCombinationUniqueError
            });
            engine.AddRepositories();
        }

        public static void AddAuth(this IServiceCollection services, AuthConfig authConfig)

        {
            if (authConfig.ByPass)
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Test Scheme";
                    options.DefaultChallengeScheme = "Test Scheme";
                }).AddTestAuth(options =>
                {
                    options.Scopes = ReflectionHelper.GetConstants<Scopes>();
                    options.Authority = authConfig.Authority;
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
                    options.Authority = authConfig.Authority;
                    options.RequireHttpsMetadata = authConfig.RequireHttps;
                    options.Audience = authConfig.Audience;
                });
            }
            services.AddAuthorization();
            services.AddSingleton<IAuthorizationPolicyProvider, ScopeAuthorizationPolicyProvider>();
            services.AddTransient<IClaimsTransformation, UserClaimsExtender>(
                x => new UserClaimsExtender(authConfig.Authority)
            );
        }

        public static void UseAppExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}