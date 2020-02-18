using App.Core;
using App.Infrastructure.Persistence.SqlServer.Context;
using Autofac;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Persistence.Ef;
using BinaryOrigin.SeedWork.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
                //var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                //optionsBuilder.UseSqlServer(connectionString);

                //engine.AddDbContext(() => new AppDbContext(optionsBuilder.Options));

                engine.AddDbContext<AppDbContext>(connectionString);

             //   engine.AddDbContext<AppDbContext>(optionsBuilder.Options);
            }
            engine.AddSqlServerDbExceptionParser(new DbErrorMessagesConfiguration
            {
                UniqueErrorTemplate = ErrorMessages.GenericUniqueError,
                CombinationUniqueErrorTemplate = ErrorMessages.GenericCombinationUniqueError
            });
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