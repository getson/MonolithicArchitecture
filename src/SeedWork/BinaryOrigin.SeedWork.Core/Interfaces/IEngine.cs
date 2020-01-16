using Autofac;
using BinaryOrigin.SeedWork.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BinaryOrigin.SeedWork.Core
{
    /// <summary>
    /// Classes implementing this interface can serve as a portal for the various services composing the App engine.
    /// Edit functionality, modules and implementations access most App functionality through this interface.
    /// </summary>
    public interface IEngine
    {
        IAppFileProvider FileProvider { get; }

        /// <summary>
        /// Get application configuration
        /// </summary>
        AppConfiguration Configuration { get; }

        /// <summary>
        /// Initialize engine
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="appFileProvider">File provider</param>
        /// <param name="appConfig"></param>
        void Initialize(IServiceCollection services, IAppFileProvider appFileProvider, AppConfiguration appConfig);

        /// <summary>
        /// Add and configure services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        /// <returns>Service provider</returns>
        IServiceProvider ConfigureServices(IServiceCollection services, AppConfiguration configuration);

        void Register(Action<ContainerBuilder> registerAction);

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <typeparam name="T">Type of resolved service</typeparam>
        /// <returns>Resolved service</returns>
        T Resolve<T>() where T : class;
        /// <summary>
        /// Resolve generic type
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        object Resolve(object param, Type type);

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <param name="type">Type of resolved service</param>
        /// <returns>Resolved service</returns>
        object Resolve(Type type);

        /// <summary>
        /// Resolve dependencies
        /// </summary>
        /// <typeparam name="T">Type of resolved services</typeparam>
        /// <returns>Collection of resolved services</returns>
        IEnumerable<T> ResolveAll<T>();

        object ResolveUnregistered(Type type);

        IEnumerable<Type> FindClassesOfType<T>(Assembly assembly, bool onlyConcreteClasses = true);

        IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);

        IEnumerable<Type> FindClassesOfType(Type type, bool onlyConcreteClasses = true);
    }
}