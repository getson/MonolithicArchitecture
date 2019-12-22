using Autofac;
using Autofac.Extensions.DependencyInjection;
using BinaryOrigin.SeedWork.Core.Configuration;
using BinaryOrigin.SeedWork.Core.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BinaryOrigin.SeedWork.Core
{
    /// <inheritdoc />
    /// <summary>
    /// Represents App engine
    /// </summary>
    public class AppEngine : IEngine
    {
        #region Properties

        /// <summary>
        /// Gets or sets service provider
        /// </summary>
        private IServiceProvider _serviceProvider;

        private ContainerBuilder _containerBuilder;
        private ITypeFinder _typeFinder;

        /// <summary>
        /// Gets or sets the default file provider
        /// </summary>
        public IAppFileProvider FileProvider { get; private set; }

        public AppConfiguration Configuration { get; set; }

        #endregion Properties

        #region Utilities

        /// <summary>
        /// Get IServiceProvider
        /// </summary>
        /// <returns>IServiceProvider</returns>
        protected virtual IServiceProvider GetServiceProvider()
        {
            return ServiceProvider;
        }

        /// <summary>
        /// Register dependencies using Autofac
        /// </summary>
        /// <param name="appConfig">Startup App configuration parameters</param>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="typeFinder">Type finder</param>
        protected virtual IServiceProvider RegisterDependencies(AppConfiguration appConfig, IServiceCollection services, ITypeFinder typeFinder)
        {
            //register type finder
            _containerBuilder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();
            _containerBuilder.RegisterInstance(FileProvider).As<IAppFileProvider>().SingleInstance();

            //find dependency registrars provided by other assemblies
            var dependencyRegistrars = typeFinder.FindClassesOfType<IDependencyRegistration>();

            //create and sort instances of dependency registrars
            var instances = dependencyRegistrars
                .Select(dependencyRegistrar => (IDependencyRegistration)Activator.CreateInstance(dependencyRegistrar))
                .OrderBy(dependencyRegistrar => dependencyRegistrar.Order);

            //register all provided dependencies
            foreach (var dependencyRegistrar in instances)
                dependencyRegistrar.Register(_containerBuilder, typeFinder, appConfig);

            //populate Autofac container builder with the set of registered service descriptors
            _containerBuilder.Populate(services);

            //create service provider
            _serviceProvider = new AutofacServiceProvider(_containerBuilder.Build());
            return _serviceProvider;
        }

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Initialize engine
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="appFileProvider"></param>
        /// <param name="notificationProvider"></param>
        /// <param name="appConfig"></param>
        public void Initialize(IServiceCollection services, IAppFileProvider appFileProvider, AppConfiguration appConfig)
        {
            Configuration = appConfig;
            FileProvider = appFileProvider;
            _containerBuilder = new ContainerBuilder();
            //find startup configurations provided by other assemblies
            _typeFinder = new AppTypeFinder(FileProvider);
        }

        /// <summary>
        /// Add and configure services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        /// <returns>Service provider</returns>
        public IServiceProvider ConfigureServices(IServiceCollection services, AppConfiguration configuration)
        {
            //add all services defined in Startup classes
            ConfigureStartupServices(services, configuration, _typeFinder);

            //register dependencies
            _serviceProvider = RegisterDependencies(configuration, services, _typeFinder);

            return _serviceProvider;
        }

        public void Register(Action<ContainerBuilder> registerAction)
        {
            registerAction(_containerBuilder);
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
        public object Resolve(object param, Type type)
        {
            var paramType = param.GetType();
            var genericType = type.MakeGenericType(paramType);
            return GetServiceProvider().GetService(genericType);
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

        /// <summary>
        /// Resolve unregistered service
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual object ResolveUnregistered(Type type)
        {
            Exception innerException = null;
            foreach (var constructor in type.GetConstructors())
            {
                try
                {
                    //try to resolve constructor parameters
                    var parameters = constructor.GetParameters().Select(parameter =>
                    {
                        var service = Resolve(parameter.ParameterType);
                        if (service == null)
                            throw new GeneralException("Unknown dependency");
                        return service;
                    });

                    //all is ok, so create instance
                    return Activator.CreateInstance(type, parameters.ToArray());
                }
                catch (Exception ex)
                {
                    innerException = ex;
                }
            }

            throw new GeneralException("No constructor was found that had all the dependencies satisfied.", innerException);
        }

        public IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true)
        {
            return _typeFinder.FindClassesOfType<T>(onlyConcreteClasses);
        }

        public IEnumerable<Type> FindClassesOfType<T>(Assembly assembly, bool onlyConcreteClasses = true)
        {
            return _typeFinder.FindClassesOfType<T>(new List<Assembly> { assembly }, onlyConcreteClasses);
        }

        public IEnumerable<Type> FindClassesOfType(Type type, bool onlyConcreteClasses = true)
        {
            return _typeFinder.FindClassesOfType(type, onlyConcreteClasses);
        }

        /// <summary>
        /// create an instance per each Startup class that implements IAppStartup and then call ConfigureServices
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="typeFinder"></param>
        protected virtual void ConfigureStartupServices(IServiceCollection services, AppConfiguration configuration, ITypeFinder typeFinder)
        {
            var startupConfigurations = typeFinder.FindClassesOfType<IAppStartup>();
            //create and sort instances of startup configurations
            var instances = startupConfigurations
                .Select(startup => (IAppStartup)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Order);

            //configure services
            foreach (var instance in instances)
            {
                instance.ConfigureServices(services, this, configuration);
            }
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Service provider
        /// </summary>
        public virtual IServiceProvider ServiceProvider => _serviceProvider;

        #endregion Properties
    }
}