using BinaryOrigin.SeedWork.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace BinaryOrigin.SeedWork.WebApi
{
    public interface IAppWebEngine : IEngine
    {
        public void Initialize(IConfiguration configuration);
    }
}