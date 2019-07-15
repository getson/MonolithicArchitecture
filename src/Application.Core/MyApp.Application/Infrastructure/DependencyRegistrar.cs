using System;
using System.Linq;
using Autofac;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MyApp.Application.Routing;
using MyApp.Core.Abstractions.Caching;
using MyApp.Core.Abstractions.Data;
using MyApp.Core.Abstractions.Infrastructure;
using MyApp.Core.Abstractions.Mapping;
using MyApp.Core.Abstractions.Web;
using MyApp.Core.Configuration;
using MyApp.Core.Infrastructure;
using MyApp.Domain.Example.BankAccountAgg;
using MyApp.Infrastructure.Cache;
using MyApp.Infrastructure.Cache.Providers;
using MyApp.Infrastructure.Cache.Providers.Redis;
using MyApp.Infrastructure.Data;
using MyApp.Services.Events;
using MyApp.Services.Example;
using MyApp.SharedKernel.Domain;
using MyApp.SharedKernel.Validator;

namespace MyApp.Application.Infrastructure
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

            RegisterUtilities(builder);

            //data layer
            RegisterDataLayer(builder, typeFinder);

            RegisterCache(builder, config);

            //work context
            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerLifetimeScope();

            //mapping
            RegisterMapping(builder);

            // services
            builder.RegisterType<RoutePublisher>().As<IRoutePublisher>().SingleInstance();
            builder.RegisterType<EventPublisher>().As<IEventPublisher>().SingleInstance();
            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().SingleInstance();
            builder.RegisterType<ActionContextAccessor>().As<IActionContextAccessor>().InstancePerLifetimeScope();
            RegisterApplicationServices(builder);


            RegisterEventConsumers(builder, typeFinder);
        }

        private static void RegisterMapping(ContainerBuilder builder)
        {
            //Adapters
            var autoMapperAdapter = new AutoMapperTypeAdapterFactory();
            builder.RegisterInstance(autoMapperAdapter).As<ITypeAdapterFactory>().SingleInstance();
        }
        private static void RegisterUtilities(ContainerBuilder builder)
        {
            builder.RegisterType<DefaultApiVersionDescriptionProvider>().As<IApiVersionDescriptionProvider>().SingleInstance();
            //Adapters

            builder.RegisterType<AutoMapperTypeAdapterFactory>().As<ITypeAdapterFactory>().SingleInstance();
            builder.RegisterType<DataAnnotationsEntityValidatorFactory>().As<IEntityValidatorFactory>().SingleInstance();

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
            builder.Register(context => context.Resolve<IDataProviderManager>().GetDataProvider()).As<IDataProvider>().InstancePerDependency();
            builder.Register(context => new MyAppObjectContext(context.Resolve<DbContextOptions<MyAppObjectContext>>()))
                .As<IDbContext>().InstancePerLifetimeScope();

            //repositories

            RegisterRepositories(builder, typeFinder);
        }
        private static void RegisterApplicationServices(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerAppService>().As<ICustomerAppService>().InstancePerLifetimeScope();
            builder.RegisterType<SalesAppService>().As<ISalesAppService>().InstancePerLifetimeScope();
            builder.RegisterType<BankTransferService>().As<IBankTransferService>().InstancePerLifetimeScope();
            builder.RegisterType<BankAppService>().As<IBankAppService>().InstancePerLifetimeScope();
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
        #endregion
        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        public int Order => 0;
    }
}
