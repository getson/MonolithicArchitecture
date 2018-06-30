using System;
using System.Linq;
using Autofac;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MyApp.Core.Configuration;
using MyApp.Core.Domain;
using MyApp.Core.Domain.Services.Banking;
using MyApp.Core.Interfaces.Caching;
using MyApp.Core.Interfaces.Data;
using MyApp.Core.Interfaces.Infrastructure;
using MyApp.Core.Interfaces.Mapping;
using MyApp.Core.Interfaces.Plugin;
using MyApp.Core.Interfaces.Web;
using MyApp.Core.SharedKernel;
using MyApp.Core.SharedKernel.Events;
using MyApp.Infrastructure.Cache;
using MyApp.Infrastructure.Cache.Providers;
using MyApp.Infrastructure.Cache.Providers.Redis;
using MyApp.Infrastructure.Common.Adapter;
using MyApp.Infrastructure.Common.Validator;
using MyApp.Infrastructure.Data;
using MyApp.Infrastructure.ExternalServices.Plugins;
using MyApp.Services.Events;
using MyApp.Services.Example;
using MyApp.Services.Installation;
using MyApp.Services.Logging;
using MyApp.Services.Plugins;
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
            //builder.RegisterType<MyAppFileProvider>().As<IMyAppFileProvider>().InstancePerLifetimeScope();

            RegisterWebUtilities(builder);

            //data layer
            RegisterDataLayer(builder, typeFinder);

            //plugins
            RegisterPlugins(builder, config);

            RegisterCache(builder, config);

            //work context
            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerLifetimeScope();

            //mapping
            RegisterMapping(builder);

            // services

            builder.RegisterType<DefaultLogger>().As<ILogger>().InstancePerLifetimeScope();
            builder.RegisterType<UserActivityService>().As<IUserActivityService>().InstancePerLifetimeScope();
            builder.RegisterType<UploadService>().As<IUploadService>().InstancePerLifetimeScope();
            builder.RegisterType<RoutePublisher>().As<IRoutePublisher>().SingleInstance();
            builder.RegisterType<EventPublisher>().As<IEventPublisher>().SingleInstance();
            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().SingleInstance();
            builder.RegisterType<ActionContextAccessor>().As<IActionContextAccessor>().InstancePerLifetimeScope();

            RegisterInstallationService(builder, config);

            RegisterApplicationServices(builder);

            RegisterDomainHandlers(builder,typeFinder);

            RegisterEventConsumers(builder, typeFinder);
        }

        private static void RegisterMapping(ContainerBuilder builder)
        {
            //Adapters
            var autoMapperAdapter = new AutomapperTypeAdapterFactory();
            builder.RegisterInstance(autoMapperAdapter).As<ITypeAdapterFactory>().SingleInstance();

            TypeAdapterFactory.SetCurrent(autoMapperAdapter);

        }
        private static void RegisterWebUtilities(ContainerBuilder builder)
        {
            //register ApiExplorer
            builder.RegisterType<DefaultApiVersionDescriptionProvider>().As<IApiVersionDescriptionProvider>().SingleInstance();

            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();
            //user agent helper
            builder.RegisterType<UserAgentHelper>().As<IUserAgentHelper>().InstancePerLifetimeScope();

            //Validator
            EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());
        }
        #region Helper methods
        private static void RegisterEventConsumers(ContainerBuilder builder, ITypeFinder typeFinder)
        {
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

        private static void RegisterDomainHandlers(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            var domainHandlers = typeFinder.FindClassesOfType(typeof(IHandler<>)).ToList();
            foreach (var consumer in domainHandlers)
            {
                builder.RegisterType(consumer)
                    .As(consumer.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IHandler<>)))
                    .InstancePerLifetimeScope();
            }
        }
        private static void RegisterRepositories(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            var repositories = typeFinder.FindClassesOfType(typeof(IRepository<>)).ToList();
            foreach (var repo in repositories)
            {
                if (repo.IsGenericType)
                    continue;

                var @interface = repo.GetInterfaces().FirstOrDefault(i => !i.IsGenericType);
                if (@interface == null)
                {
                    throw new InvalidOperationException($"Cannot find valid interface for {repo}");
                }
                builder.RegisterType(repo).As(@interface).InstancePerLifetimeScope();
            }
        }

        private static void RegisterDataLayer(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<EfDataProviderManager>().As<IDataProviderManager>().InstancePerDependency();
            builder.Register(context => context.Resolve<IDataProviderManager>().DataProvider).As<IDataProvider>().InstancePerDependency();
            builder.Register(context => new MyAppObjectContext(context.Resolve<DbContextOptions<MyAppObjectContext>>()))
                .As<IDbContext>().InstancePerLifetimeScope();

            //repositories
            //builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            RegisterRepositories(builder, typeFinder);
        }

        private static void RegisterApplicationServices(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerAppService>().As<ICustomerAppService>().InstancePerLifetimeScope();
            builder.RegisterType<SalesAppService>().As<ISalesAppService>().InstancePerLifetimeScope();
            builder.RegisterType<BankTransferService>().As<IBankTransferService>().InstancePerLifetimeScope();
            builder.RegisterType<BankAppService>().As<IBankAppService>().InstancePerLifetimeScope();
        }

        private static void RegisterPlugins(ContainerBuilder builder, MyAppConfig config)
        {
            builder.RegisterType<PluginFinder>().As<IPluginFinder>().InstancePerLifetimeScope();
            builder.RegisterType<OfficialFeedManager>().As<IOfficialFeedManager>().InstancePerLifetimeScope();
        }

        private static void RegisterCache(ContainerBuilder builder, MyAppConfig config)
        {
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
        }
        private static void RegisterInstallationService(ContainerBuilder builder, MyAppConfig config)
        {
            if (!DataSettingsManager.DatabaseIsInstalled)
            {
                if (config.UseFastInstallationService)
                    builder.RegisterType<SqlFileInstallationService>().As<IInstallationService>().InstancePerLifetimeScope();
                else
                    builder.RegisterType<CodeFirstInstallationService>().As<IInstallationService>().InstancePerLifetimeScope();
            }
        }

        #endregion
        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        public int Order => 0;
    }
}
