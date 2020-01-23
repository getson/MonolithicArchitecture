using BinaryOrigin.SeedWork.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace BinaryOrigin.SeedWork.WebApi
{
    public interface IAppWebEngine : IEngine
    {
        public void Initialize(IConfiguration configuration);
        /// <summary>
        /// Configure HTTP request pipeline
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        void ConfigureRequestPipeline(IApplicationBuilder application);
    }
}