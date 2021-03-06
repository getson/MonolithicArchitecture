﻿using Autofac;
using Microsoft.Extensions.Configuration;
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
        /// <summary>
        /// Initialize engine
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="appFileProvider">File provider</param>
        /// <param name="configuration"></param>
        void Initialize(IAppFileProvider appFileProvider, IConfiguration configuration);

        /// <summary>
        /// Add and configure services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        /// <returns>Service provider</returns>
        IServiceProvider ConfigureServices(IServiceCollection services);

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
        /// Resolve all the concrete implementation of type t
        /// </summary>
        /// <typeparam name="T">Type of resolved services</typeparam>
        /// <returns>Collection of resolved services</returns>
        IEnumerable<T> ResolveAll<T>();
    
        IEnumerable<Type> FindClassesOfType<T>(Assembly assembly, bool onlyConcreteClasses = true);

        IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);

        IEnumerable<Type> FindClassesOfType(Type type, bool onlyConcreteClasses = true);
    }
}