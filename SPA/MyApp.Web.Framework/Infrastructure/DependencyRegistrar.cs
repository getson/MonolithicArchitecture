using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MyApp.Core;
using MyApp.Core.Common;
using MyApp.Core.Configuration;
using MyApp.Core.Domain.Common;
using MyApp.Core.Domain.Services.Configuration;
using MyApp.Core.Domain.Services.Events;
using MyApp.Core.Domain.Services.Installation;
using MyApp.Core.Domain.Services.Localization;
using MyApp.Core.Domain.Services.Logging;
using MyApp.Core.Domain.Services.Plugins;
using MyApp.Core.Infrastructure.Implementation;
using MyApp.Core.Infrastructure.Interfaces;
using MyApp.Core.Interfaces.Caching;
using MyApp.Core.Interfaces.Data;
using MyApp.Core.Interfaces.Plugin;
using MyApp.Core.Plugins;
using MyApp.Infrastructure.Cache;
using MyApp.Infrastructure.Common.Helpers;
using MyApp.Infrastructure.Data;
using MyApp.Web.Framework.Common;
using MyApp.Web.Framework.Routing;

namespace MyApp.Web.Framework.Infrastructure
{
    /// <summary>
    /// Dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, MyAppConfig config)
        {
            //file provider
            builder.RegisterType<MyAppFileProvider>().As<IMyAppFileProvider>().InstancePerLifetimeScope();

            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();

            //user agent helper
            builder.RegisterType<UserAgentHelper>().As<IUserAgentHelper>().InstancePerLifetimeScope();

            //data layer
            builder.RegisterType<EfDataProviderManager>().As<IDataProviderManager>().InstancePerDependency();
            builder.Register(context => context.Resolve<IDataProviderManager>().DataProvider).As<IDataProvider>().InstancePerDependency();
            builder.Register(context => new MyAppObjectContext(context.Resolve<DbContextOptions<MyAppObjectContext>>()))
                .As<IDbContext>().InstancePerLifetimeScope();

            //repositories
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            //plugins
            builder.RegisterType<PluginFinder>().As<IPluginFinder>().InstancePerLifetimeScope();
            builder.RegisterType<OfficialFeedManager>().As<IOfficialFeedManager>().InstancePerLifetimeScope();

            //cache manager
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().InstancePerLifetimeScope();

            //static cache manager
            if (config.RedisCachingEnabled)
            {
                builder.RegisterType<RedisConnectionWrapper>()
                    .As<ILocker>()
                    .As<IRedisConnectionWrapper>()
                    .SingleInstance();
                builder.RegisterType<RedisCacheManager>().As<IStaticCacheManager>().InstancePerLifetimeScope();
            }
            else
            {
                builder.RegisterType<MemoryCacheManager>()
                    .As<ILocker>()
                    .As<IStaticCacheManager>()
                    .SingleInstance();
            }

            //work context
            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerLifetimeScope();

          
            //services
           
            builder.RegisterType<LocalizationService>().As<ILocalizationService>().InstancePerLifetimeScope();
            builder.RegisterType<LocalizedEntityService>().As<ILocalizedEntityService>().InstancePerLifetimeScope();
            builder.RegisterType<LanguageService>().As<ILanguageService>().InstancePerLifetimeScope();
            //builder.RegisterType<DownloadService>().As<IDownloadService>().InstancePerLifetimeScope();
           
            //builder.RegisterType<EmailSender>().As<IEmailSender>().InstancePerLifetimeScope();
       
            
            builder.RegisterType<DefaultLogger>().As<ILogger>().InstancePerLifetimeScope();
            
            builder.RegisterType<DateTimeHelper>().As<IDateTimeHelper>().InstancePerLifetimeScope();
            //builder.RegisterType<ScheduleTaskService>().As<IScheduleTaskService>().InstancePerLifetimeScope();
            //builder.RegisterType<ExportManager>().As<IExportManager>().InstancePerLifetimeScope();
            //builder.RegisterType<ImportManager>().As<IImportManager>().InstancePerLifetimeScope();
            //builder.RegisterType<PdfService>().As<IPdfService>().InstancePerLifetimeScope();
            builder.RegisterType<UploadService>().As<IUploadService>().InstancePerLifetimeScope();
            //builder.RegisterType<ThemeProvider>().As<IThemeProvider>().InstancePerLifetimeScope();
            //builder.RegisterType<ThemeContext>().As<IThemeContext>().InstancePerLifetimeScope();
            //builder.RegisterType<ExternalAuthenticationService>().As<IExternalAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<RoutePublisher>().As<IRoutePublisher>().SingleInstance();
            builder.RegisterType<EventPublisher>().As<IEventPublisher>().SingleInstance();
            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().SingleInstance();
            builder.RegisterType<SettingService>().As<ISettingService>().InstancePerLifetimeScope();


            builder.RegisterType<ActionContextAccessor>().As<IActionContextAccessor>().InstancePerLifetimeScope();


            //picture service
            //if (!string.IsNullOrEmpty(config.AzureBlobStorageConnectionString))
            //    builder.RegisterType<AzurePictureService>().As<IPictureService>().InstancePerLifetimeScope();
            //else
            //    builder.RegisterType<PictureService>().As<IPictureService>().InstancePerLifetimeScope();

            //installation service
            if (!DataSettingsManager.DatabaseIsInstalled)
            {
                if (config.UseFastInstallationService)
                    builder.RegisterType<SqlFileInstallationService>().As<IInstallationService>().InstancePerLifetimeScope();
                else
                    builder.RegisterType<CodeFirstInstallationService>().As<IInstallationService>().InstancePerLifetimeScope();
            }

            //event consumers
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
            {
                builder.RegisterType(consumer)
                    .As(consumer.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IConsumer<>)))
                    .InstancePerLifetimeScope();
            }
        }

        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        public int Order => 0;
    }
}
