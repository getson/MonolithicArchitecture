using BinaryOrigin.SeedWork.Core;
using Microsoft.AspNetCore.Builder;

namespace BinaryOrigin.SeedWork.WebApi
{
    public interface IAppWebEngine : IEngine
    {
        /// <summary>
        /// Configure HTTP request pipeline
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        void ConfigureRequestPipeline(IApplicationBuilder application);
    }
}