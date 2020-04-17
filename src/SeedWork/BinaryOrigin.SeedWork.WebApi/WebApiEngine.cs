using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace BinaryOrigin.SeedWork.WebApi
{
    public class AppWebApiEngine : AppEngine, IAppWebEngine
    {
        /// <summary>
        /// Initialize WebApiEngine
        /// </summary>
        /// <param name="configuration"></param>
        public void Initialize(IConfiguration configuration)
        {
            var fileProvider = new AppFileProvider(AppContext.BaseDirectory);
            Initialize(fileProvider, configuration);
        }

        protected override IServiceProvider GetServiceProvider()
        {
            var accessor = ServiceProvider.GetService<IHttpContextAccessor>();
            var context = accessor.HttpContext;
            return context?.RequestServices ?? ServiceProvider;
        }
    }
}