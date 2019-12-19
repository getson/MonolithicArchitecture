using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using BinaryOrigin.SeedWork.WebApi;
using Microsoft.AspNetCore.Builder;
using App.Persistence.Context;
using BinaryOrigin.SeedWork.Core;
using Microsoft.EntityFrameworkCore;
using BinaryOrigin.SeedWork.Persistence.SqlServer;
using Autofac;
using BinaryOrigin.SeedWork.Persistence.Ef;

namespace App.WebApi.Extensions
{
    public static class RegistrationExtensions
    {
        public static void AddSqlDbContext(this IEngine engine)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(engine.Configuration.DbConnectionString);

            engine.Register(builder =>
            {
                builder.RegisterType<SqlServerDataProvider>()
                        .As<IDataProvider>()
                        .SingleInstance();
                builder.Register(instance => new AppDbContext(optionsBuilder.Options))
                        .As<IDbContext>()
                        .InstancePerLifetimeScope();
            });
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