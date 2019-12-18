using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Core.Configuration;
using Microsoft.AspNetCore.Builder;

namespace BinaryOrigin.SeedWork.WebApi
{
    /// <inheritdoc />
    /// <summary>
    /// Represents object for the configuring services and middleware on application startup
    /// </summary>
    public interface IWebAppStartup : IAppStartup
    {
        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        /// <param name="configuration"></param>
        void Configure(IApplicationBuilder application, AppConfiguration configuration);
    }
}