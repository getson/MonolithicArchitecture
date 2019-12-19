using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace BinaryOrigin.SeedWork.WebApi
{
    public class AppWebApiEngine : AppEngine, IAppWebEngine
    {
        /// <summary>
        /// Configure HTTP request pipeline
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void ConfigureRequestPipeline(IApplicationBuilder application)
        {
            var typeFinder = Resolve<ITypeFinder>();
            //find startup configurations provided by other assemblies
            var startupConfigurations = typeFinder.FindClassesOfType<IWebAppStartup>();

            //create and sort instances of startup configurations
            var instances = startupConfigurations
                .Select(startup => (IWebAppStartup)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Order);

            var configuration = Resolve<AppConfiguration>();

            //configure request pipeline
            foreach (var instance in instances)
            {
                instance.Configure(application, configuration);
            }
        }

        protected override IServiceProvider GetServiceProvider()
        {
            var accessor = ServiceProvider.GetService<IHttpContextAccessor>();
            var context = accessor.HttpContext;
            return context?.RequestServices ?? ServiceProvider;
        }
    }
}