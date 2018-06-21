using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
using MyApp.Core.Configuration;
using MyApp.Core.Domain.Security;
using MyApp.Core.Infrastructure;
using MyApp.Core.Infrastructure.Common;
using MyApp.Infrastructure.Data;
using SPA.Infrastructure.Extensions;

namespace SPA.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring common features and middleware on application startup
    /// </summary>
    public class MyAppCommonStartup : IMyAppStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            //compression
            services.AddResponseCompression();

            //add options feature
            services.AddOptions();

            //add memory cache
            services.AddMemoryCache();

            //add distributed memory cache
            services.AddDistributedMemoryCache();
                        
            //add HTTP sesion state feature
            services.AddHttpSession();

            //add anti-forgery
            services.AddAntiForgery();

            //add localization
            services.AddLocalization();

            //add theme support
            services.AddThemes();
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            var MyAppConfig = EngineContext.Current.Resolve<MyAppConfig>();
            var fileProvider = EngineContext.Current.Resolve<IMyAppFileProvider>();

            //compression
            if (MyAppConfig.UseResponseCompression)
            {
                //gzip by default
                application.UseResponseCompression();
            }

            //static files
            application.UseStaticFiles(new StaticFileOptions
            {
                //TODO duplicated code (below)
                OnPrepareResponse = ctx =>
                {
                    if (!string.IsNullOrEmpty(MyAppConfig.StaticFilesCacheControl))
                        ctx.Context.Response.Headers.Append(HeaderNames.CacheControl, MyAppConfig.StaticFilesCacheControl);
                }
            });
            //themes
            application.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(fileProvider.MapPath(@"Themes")),
                RequestPath = new PathString("/Themes"),
                OnPrepareResponse = ctx =>
                {
                    if (!string.IsNullOrEmpty(MyAppConfig.StaticFilesCacheControl))
                        ctx.Context.Response.Headers.Append(HeaderNames.CacheControl, MyAppConfig.StaticFilesCacheControl);
                }
            });
            
            //plugins
            var staticFileOptions = new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(fileProvider.MapPath(@"Plugins")),
                RequestPath = new PathString("/Plugins"),
                OnPrepareResponse = ctx =>
                {
                    if (!string.IsNullOrEmpty(MyAppConfig.StaticFilesCacheControl))
                        ctx.Context.Response.Headers.Append(HeaderNames.CacheControl,
                            MyAppConfig.StaticFilesCacheControl);
                }
            };
            //whether database is installed
            if (DataSettingsManager.DatabaseIsInstalled)
            {
                var securitySettings = EngineContext.Current.Resolve<SecuritySettings>();
                if (!string.IsNullOrEmpty(securitySettings.PluginStaticFileExtensionsBlacklist))
                {
                    var fileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();

                    foreach (var ext in securitySettings.PluginStaticFileExtensionsBlacklist
                        .Split(';', ',')
                        .Select(e => e.Trim().ToLower())
                        .Select(e => $"{(e.StartsWith(".") ? string.Empty : ".")}{e}")
                        .Where(fileExtensionContentTypeProvider.Mappings.ContainsKey))
                    {
                        fileExtensionContentTypeProvider.Mappings.Remove(ext);
                    }

                    staticFileOptions.ContentTypeProvider = fileExtensionContentTypeProvider;
                }
            }
            application.UseStaticFiles(staticFileOptions);

            //add support for backups
            var provider = new FileExtensionContentTypeProvider
            {
                Mappings = {[".bak"] = MimeTypes.ApplicationOctetStream}
            };

            application.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(fileProvider.GetAbsolutePath("db_backups")),
                RequestPath = new PathString("/db_backups"),
                ContentTypeProvider = provider
            });

            //check whether requested page is keep alive page
            application.UseKeepAlive();

            //check whether database is installed
            application.UseInstallUrl();

            //use HTTP session
            application.UseSession();

            //use request localization
            application.UseRequestLocalization();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order
        {
            //common services should be loaded after error handlers
            get { return 100; }
        }
    }
}
