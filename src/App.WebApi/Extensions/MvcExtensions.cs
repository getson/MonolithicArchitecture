using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using BinaryOrigin.SeedWork.WebApi;
using Microsoft.AspNetCore.Builder;

namespace App.WebApi.Extensions
{
    public static class MvcExtensions
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