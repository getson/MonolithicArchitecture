using App.Infrastructure.Persistence.SqlServer.Context;
using Autofac;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Persistence.Ef;
using BinaryOrigin.SeedWork.Persistence.SqlServer;
using BinaryOrigin.SeedWork.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace App.WebApi.Extensions
{
    public static class ServicesExtensions
    {
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