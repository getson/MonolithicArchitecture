using App.Core;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Persistence.Ef;
using BinaryOrigin.SeedWork.WebApi;
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
                engine.AddDefaultSqlDbContext(connectionString);
            }
            engine.AddSqlServerDbExceptionParser(new DbErrorMessagesConfiguration
            {
                UniqueErrorTemplate = ErrorMessages.GenericUniqueError,
                CombinationUniqueErrorTemplate = ErrorMessages.GenericCombinationUniqueError
            });
            engine.AddRepositories();
            engine.AddRepositories();
        }

        public static void UseAppExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
        }

        /// <summary>
        /// Add HttpContextAccessor as a service
        /// </summary>
        /// <param name="services"></param>
        public static void AddHttpContextAccesor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}