namespace MyApp.Infrastructure.Cache.Providers.Redis
{
    /// <summary>
    /// Redis settings
    /// </summary>
    public static class RedisConfiguration
    {
        /// <summary>
        /// Get the key used to Tenant the protection key list (used with the PersistDataProtectionKeyTenantdis option enabled)
        /// </summary>
        public static string DataProtectionKeysName => "MyApp.DataProtectionKeys";
    }
}