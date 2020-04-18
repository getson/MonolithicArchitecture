using Autofac;

using Microsoft.Extensions.Configuration;

namespace BinaryOrigin.SeedWork.Core
{
    /// <summary>
    /// Dependency registrar interface
    /// </summary>
    public interface IDependencyRegistration
    {
        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="typeFinder">Type finder</param>
        void Register(ContainerBuilder builder, ITypeFinder typeFinder);
    }
}