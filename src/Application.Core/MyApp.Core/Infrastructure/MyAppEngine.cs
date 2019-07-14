using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Abstractions.Infrastructure;
using MyApp.Core.Abstractions.Mapping;
using MyApp.Core.Configuration;

namespace MyApp.Core.Infrastructure
{
    /// <inheritdoc />
    /// <summary>
    /// Represents MyApp engine
    /// </summary>
    public class MyAppEngine : IEngine
    {
        #region Properties

        /// <summary>
        /// Gets or sets service provider
        /// </summary>
        private IServiceProvider _serviceProvider;
        /// <summary>
        /// Gets or sets the default file provider
        /// </summary>
        public IMyAppFileProvider FileProvider { get; private set; }
        public MyAppConfig Configuration { get; set; }

        #endregion

        #region Utilities

        /// <summary>
        /// Get IServiceProvider
        /// </summary>
        /// <returns>IServiceProvider</returns>
        protected IServiceProvider GetServiceProvider()
        {
            var accessor = ServiceProvider.GetService<IHttpContextAccessor>();
            var context = accessor.HttpContext;
            return context?.RequestServices ?? ServiceProvider;
        }

        /// <summary>
        /// Register dependencies using Autofac
        /// </summary>
        /// <param name="MyAppConfig">Startup MyApp configuration parameters</param>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="typeFinder">Type finder</param>
        protected virtual IServiceProvider RegisterDependencies(MyAppConfig MyAppConfig, IServiceCollection services, ITypeFinder typeFinder)
        {

            var containerBuilder = new ContainerBuilder();

          

            //register type finder
            containerBuilder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();
            containerBuilder.RegisterInstance(FileProvider).As<IMyAppFileProvider>().SingleInstance();
            //find dependency registrars provided by other assemblies
            var dependencyRegistrars = typeFinder.FindClassesOfType<IDependencyRegistrar>();

            //create and sort instances of dependency registrars
            var instances = dependencyRegistrars
                .Select(dependencyRegistrar => (IDependencyRegistrar)Activator.CreateInstance(dependencyRegistrar))
                .OrderBy(dependencyRegistrar => dependencyRegistrar.Order);

            //register all provided dependencies
            foreach (var dependencyRegistrar in instances)
                dependencyRegistrar.Register(containerBuilder, typeFinder, MyAppConfig);

            //populate Autofac container builder with the set of registered service descriptors
            containerBuilder.Populate(services);

            //create service provider
            _serviceProvider = new AutofacServiceProvider(containerBuilder.Build());
            return _serviceProvider;
        }

        /// <summary>
        /// Register and configure AutoMapper
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="typeFinder">Type finder</param>
        protected virtual void RegisterMappingProfiles(IServiceCollection services, ITypeFinder typeFinder)
        {
            //find mapper configurations provided by other assemblies
            var mapperConfigurations = typeFinder.FindClassesOfType<IMapperProfile>();

            //create and sort instances of mapper configurations
            var instances = mapperConfigurations
                .Select(mapperConfiguration => (IMapperProfile)Activator.CreateInstance(mapperConfiguration))
                .OrderBy(mapperConfiguration => mapperConfiguration.Order);

            //create AutoMapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                foreach (var instance in instances)
                {
                    cfg.AddProfile(instance.GetType());
                }
            });
            AutoMapperConfiguration.Init(config);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize engine
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="MyAppFileProvider"></param>
        /// <param name="config"></param>
        public void Initialize(IServiceCollection services, IMyAppFileProvider MyAppFileProvider, MyAppConfig MyAppConfig)
        {
            Configuration = MyAppConfig;
            FileProvider = MyAppFileProvider;
        }
        /// <summary>
        /// Add and configure services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        /// <returns>Service provider</returns>
        public IServiceProvider ConfigureServices(IServiceCollection services, MyAppConfig configuration)
        {
            //find startup configurations provided by other assemblies
            var typeFinder = new MyAppTypeFinder(FileProvider);
            //add all services defined in Startup classes
            ConfigureStartupsServices(services, configuration, typeFinder);

            //register mapper configurations
            RegisterMappingProfiles(services, typeFinder);
            //register dependencies
            _serviceProvider = RegisterDependencies(configuration, services, typeFinder);


            return _serviceProvider;
        }
        /// <summary>
        /// create an instance per each Startup class that implements IMyAppStartup and then call ConfigureServices
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="typeFinder"></param>
        private void ConfigureStartupsServices(IServiceCollection services, MyAppConfig configuration, ITypeFinder typeFinder)
        {
            var startupConfigurations = typeFinder.FindClassesOfType<IMyAppStartup>();
            //create and sort instances of startup configurations
            var instances = startupConfigurations
                .Select(startup => (IMyAppStartup)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Order);

            //configure services
            foreach (var instance in instances)
                instance.ConfigureServices(services, configuration);
        }

        /// <inheritdoc />
        /// <summary>
        /// Configure HTTP request pipeline
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void ConfigureRequestPipeline(IApplicationBuilder application)
        {
            var typeFinder = Resolve<ITypeFinder>();
            //find startup configurations provided by other assemblies
            var startupConfigurations = typeFinder.FindClassesOfType<IMyAppStartup>();

            //create and sort instances of startup configurations
            var instances = startupConfigurations
                //.Where(startup => PluginManager.FindPlugin(startup)?.Installed ?? true) //ignore not installed plugins
                .Select(startup => (IMyAppStartup)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Order);

            //configure request pipeline
            foreach (var instance in instances)
                instance.Configure(application);
        }

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <typeparam name="T">Type of resolved service</typeparam>
        /// <returns>Resolved service</returns>
        public T Resolve<T>() where T : class
        {

            return (T)GetServiceProvider().GetRequiredService(typeof(T));
        }

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <param name="type">Type of resolved service</param>
        /// <returns>Resolved service</returns>
        public object Resolve(Type type)
        {
            return GetServiceProvider().GetRequiredService(type);
        }

        /// <summary>
        /// Resolve dependencies
        /// </summary>
        /// <typeparam name="T">Type of resolved services</typeparam>
        /// <returns>Collection of resolved services</returns>
        public IEnumerable<T> ResolveAll<T>()
        {
            return (IEnumerable<T>)GetServiceProvider().GetServices(typeof(T));
        }
        #endregion

        #region Properties

        /// <summary>
        /// Service provider
        /// </summary>
        public virtual IServiceProvider ServiceProvider => _serviceProvider;

        #endregion
    }
}
