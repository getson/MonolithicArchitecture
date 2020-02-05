using CacheManager.Core;
using EFSecondLevelCache.Core;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServicesExtensions
    {
        /// <summary>
        /// Add Second level cache for entity framework while using InMemory cache
        /// </summary>
        /// <param name="services"></param>
        public static void AddDefaultEfSecondLevelCache(this IServiceCollection services)
        {
            services.AddEFSecondLevelCache();

            // Add an in-memory cache service provider
            services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));
            services.AddSingleton(typeof(ICacheManagerConfiguration),
                                new ConfigurationBuilder()
                                        .WithJsonSerializer()
                                        .WithMicrosoftMemoryCacheHandle(instanceName: "MemoryCache1")
                                        .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(10))
                                        .Build());
        }
    }
}