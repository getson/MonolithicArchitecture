using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
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

        /// <summary>
        /// Gets or sets service provider
        /// </summary>
        private IServiceProvider _serviceProvider;

        
        protected ContainerBuilder ContainerBuilder { get; private set; }
        protected ITypeFinder TypeFinder { get; private set; }
        protected IAppFileProvider FileProvider { get; private set; }
        protected IConfiguration Configuration { get; private set; }

        /// <summary>
        /// Initialize engine
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="appFileProvider"></param>
        /// <param name="notificationProvider"></param>
        /// <param name="configuration"></param>
        public void Initialize(IAppFileProvider appFileProvider, IConfiguration configuration)
        {
            Configuration = configuration;
            FileProvider = appFileProvider;
            ContainerBuilder = new ContainerBuilder();
            TypeFinder = new AppTypeFinder(FileProvider);

            //register the instances to be available where are required
            ContainerBuilder.RegisterInstance(TypeFinder).As<ITypeFinder>().SingleInstance();
            ContainerBuilder.RegisterInstance(FileProvider).As<IAppFileProvider>().SingleInstance();

        }

        /// <summary>
        /// Add and configure services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        /// <returns>Service provider</returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //register dependencies
            _serviceProvider = RegisterDependencies(services);

            return _serviceProvider;
        }

        public void Register(Action<ContainerBuilder> registerAction)
        {
            registerAction(ContainerBuilder);
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
        /// Resolve all the concrete implementation of type t
        /// </summary>
        /// <typeparam name="T">Type of resolved services</typeparam>
        /// <returns>Collection of resolved services</returns>
        public IEnumerable<T> ResolveAll<T>()
        {
            return (IEnumerable<T>)GetServiceProvider().GetServices(typeof(T));
        }
        public IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true)
        {
            return TypeFinder.FindClassesOfType<T>(onlyConcreteClasses);
        }

        public IEnumerable<Type> FindClassesOfType<T>(Assembly assembly, bool onlyConcreteClasses = true)
        {
            return TypeFinder.FindClassesOfType<T>(new List<Assembly> { assembly }, onlyConcreteClasses);
        }

        public IEnumerable<Type> FindClassesOfType(Type type, bool onlyConcreteClasses = true)
        {
            return TypeFinder.FindClassesOfType(type, onlyConcreteClasses);
        }

        /// <summary>
        /// Service provider
        /// </summary>
        public virtual IServiceProvider ServiceProvider => _serviceProvider;
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
        /// <param name="configuration">Startup App configuration parameters</param>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="typeFinder">Type finder</param>
        protected virtual IServiceProvider RegisterDependencies(IServiceCollection services)
        {

            //find dependency registrars provided by other assemblies
            var dependencyRegistrars = TypeFinder.FindClassesOfType<IDependencyRegistration>();

            //create and sort instances of dependency registrars
            var instances = dependencyRegistrars
                .Select(dependencyRegistrar => (IDependencyRegistration)Activator.CreateInstance(dependencyRegistrar))
                .OrderBy(dependencyRegistrar => dependencyRegistrar.Order);

            //register all provided dependencies
            foreach (var dependencyRegistrar in instances)
                dependencyRegistrar.Register(ContainerBuilder, TypeFinder, Configuration);

            //populate Autofac container builder with the set of registered service descriptors
            ContainerBuilder.Populate(services);

            //create service provider
            _serviceProvider = new AutofacServiceProvider(ContainerBuilder.Build());
            return _serviceProvider;
        }

    }
}