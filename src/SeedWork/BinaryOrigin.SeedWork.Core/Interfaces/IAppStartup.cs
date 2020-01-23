using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BinaryOrigin.SeedWork.Core
{
    /// <summary>
    /// Represents object for the configuring services and middleware on application startup
    /// </summary>
    public interface IAppStartup
    {
        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="engine"></param>
        /// <param name="configuration">Configuration root of the application</param>
        void ConfigureServices(IServiceCollection services, IEngine engine, IConfiguration configuration);
    }
}