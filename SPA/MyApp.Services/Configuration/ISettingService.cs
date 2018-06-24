using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MyApp.Core.Configuration;
using MyApp.Core.Domain.Common;
using MyApp.Core.Domain.Configuration;

namespace MyApp.Core.Domain.Services.Configuration
{
    /// <summary>
    /// Setting service interface
    /// </summary>
    public partial interface ISettingService
    {
        /// <summary>
        /// Gets a setting by identifier
        /// </summary>
        /// <param name="settingId">Setting identifier</param>
        /// <returns>Setting</returns>
        Setting GetSettingById(int settingId);

        /// <summary>
        /// Deletes a setting
        /// </summary>
        /// <param name="setting">Setting</param>
        void DeleteSetting(Setting setting);

        /// <summary>
        /// Deletes settings
        /// </summary>
        /// <param name="settings">Settings</param>
        void DeleteSettings(IList<Setting> settings);

        /// <summary>
        /// Get setting by key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="TenantId">Tenant identifier</param>
        /// <param name="loadSharedValueIfNotFound">A value indicating whether a shared (for all Tenants) value should be loaded if a value specific for a certain is not found</param>
        /// <returns>Setting</returns>
        Setting GetSetting(string key, int TenantId = 0, bool loadSharedValueIfNotFound = false);

        /// <summary>
        /// Get setting value by key
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="TenantId">Tenant identifier</param>
        /// <param name="defaultValue">Default value</param>
        /// <param name="loadSharedValueIfNotFound">A value indicating whether a shared (for all Tenants) value should be loaded if a value specific for a certain is not found</param>
        /// <returns>Setting value</returns>
        T GetSettingByKey<T>(string key, T defaultValue = default(T), 
            int TenantId = 0, bool loadSharedValueIfNotFound = false);
        
        /// <summary>
        /// Set setting value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="TenantId">Tenant identifier</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        void SetSetting<T>(string key, T value, int TenantId = 0, bool clearCache = true);

        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Settings</returns>
        IList<Setting> GetAllSettings();

        /// <summary>
        /// Determines whether a setting exists
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="settings">Settings</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="TenantId">Tenant identifier</param>
        /// <returns>true -setting exists; false - does not exist</returns>
        bool SettingExists<T, TPropType>(T settings, 
            Expression<Func<T, TPropType>> keySelector, int TenantId = 0)
            where T : ISettings, new();

        /// <summary>
        /// Load settings
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="TenantId">Tenant identifier for which settings should be loaded</param>
        T LoadSetting<T>(int TenantId = 0) where T : ISettings, new();
        /// <summary>
        /// Load settings
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="TenantId">Tenant identifier for which settings should be loaded</param>
        ISettings LoadSetting(Type type, int TenantId = 0);

        /// <summary>
        /// Save settings object
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="TenantId">Tenant identifier</param>
        /// <param name="settings">Setting instance</param>
        void SaveSetting<T>(T settings, int TenantId = 0) where T : ISettings, new();
        
        /// <summary>
        /// Save settings object
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="settings">Settings</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="TenantId">Tenant ID</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        void SaveSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector,
            int TenantId = 0, bool clearCache = true) where T : ISettings, new();

        /// <summary>
        /// Save settings object (per Tenant). If the setting is not overridden per Tenantm then it'll be delete
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="settings">Settings</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="overrideForTenant">A value indicating whether to setting is overridden in some Tenant</param>
        /// <param name="TenantId">Tenant ID</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        void SaveSettingOverridablePerTenant<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector,
            bool overrideForTenant, int TenantId = 0, bool clearCache = true) where T : ISettings, new();

        /// <summary>
        /// Delete all settings
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        void DeleteSetting<T>() where T : ISettings, new();
        
        /// <summary>
        /// Delete settings object
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="settings">Settings</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="TenantId">Tenant ID</param>
        void DeleteSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector, int TenantId = 0) where T : ISettings, new();

        /// <summary>
        /// Clear cache
        /// </summary>
        void ClearCache();
    }
}
