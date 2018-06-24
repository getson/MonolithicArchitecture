using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MyApp.Core.Common;
using MyApp.Core.Configuration;
using MyApp.Core.Domain.Common;
using MyApp.Core.Domain.Configuration;
using MyApp.Core.Domain.Services.Events;
using MyApp.Core.Interfaces.Caching;
using MyApp.Core.Interfaces.Data;
using MyApp.Infrastructure.Cache;

namespace MyApp.Core.Domain.Services.Configuration
{
    /// <summary>
    /// Setting manager
    /// </summary>
    public partial class SettingService : ISettingService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string SETTINGS_ALL_KEY = "MyApp.setting.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string SETTINGS_PATTERN_KEY = "MyApp.setting.";

        #endregion

        #region Fields

        private readonly IRepository<Setting> _settingRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IStaticCacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Static cache manager</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="settingRepository">Setting repository</param>
        public SettingService(IStaticCacheManager cacheManager, 
            IEventPublisher eventPublisher,
            IRepository<Setting> settingRepository)
        {
            _cacheManager = cacheManager;
            _eventPublisher = eventPublisher;
            _settingRepository = settingRepository;
        }

        #endregion

        #region Nested classes

        /// <summary>
        /// Setting (for caching)
        /// </summary>
        [Serializable]
        public class SettingForCaching
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
            public int TenantId { get; set; }
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Settings</returns>
        protected virtual IDictionary<string, IList<SettingForCaching>> GetAllSettingsCached()
        {
            //cache
            var key = string.Format(SETTINGS_ALL_KEY);
            return _cacheManager.Get(key, () =>
            {
                //we use no tracking here for performance optimization
                //anyway records are loaded only for read-only operations
                var query = from s in _settingRepository.TableNoTracking
                            orderby s.Name, s.TenantId
                            select s;
                var settings = query.ToList();
                var dictionary = new Dictionary<string, IList<SettingForCaching>>();
                foreach (var s in settings)
                {
                    var resourceName = s.Name.ToLowerInvariant();
                    var settingForCaching = new SettingForCaching
                            {
                                Id = s.Id,
                                Name = s.Name,
                                Value = s.Value,
                                TenantId = s.TenantId
                            };
                    if (!dictionary.ContainsKey(resourceName))
                    {
                        //first setting
                        dictionary.Add(resourceName, new List<SettingForCaching>
                        {
                            settingForCaching
                        });
                    }
                    else
                    {
                        //already added
                        //most probably it's the setting with the same name but for some certain Tenant (TenantId > 0)
                        dictionary[resourceName].Add(settingForCaching);
                    }
                }
                return dictionary;
            });
        }

        /// <summary>
        /// Set setting value
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="TenantId">Tenant identifier</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        protected virtual void SetSetting(Type type, string key, object value, int TenantId = 0, bool clearCache = true)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            key = key.Trim().ToLowerInvariant();
            var valueStr = TypeDescriptor.GetConverter(type).ConvertToInvariantString(value);

            var allSettings = GetAllSettingsCached();
            var settingForCaching = allSettings.ContainsKey(key) ?
                allSettings[key].FirstOrDefault(x => x.TenantId == TenantId) : null;
            if (settingForCaching != null)
            {
                //update
                var setting = GetSettingById(settingForCaching.Id);
                setting.Value = valueStr;
                UpdateSetting(setting, clearCache);
            }
            else
            {
                //insert
                var setting = new Setting
                {
                    Name = key,
                    Value = valueStr,
                    TenantId = TenantId
                };
                InsertSetting(setting, clearCache);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a setting
        /// </summary>
        /// <param name="setting">Setting</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        public virtual void InsertSetting(Setting setting, bool clearCache = true)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));

            _settingRepository.Insert(setting);

            //cache
            if (clearCache)
                _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(setting);
        }

        /// <summary>
        /// Updates a setting
        /// </summary>
        /// <param name="setting">Setting</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        public virtual void UpdateSetting(Setting setting, bool clearCache = true)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));

            _settingRepository.Update(setting);

            //cache
            if (clearCache)
                _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(setting);
        }

        /// <summary>
        /// Deletes a setting
        /// </summary>
        /// <param name="setting">Setting</param>
        public virtual void DeleteSetting(Setting setting)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));

            _settingRepository.Delete(setting);

            //cache
            _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(setting);
        }

        /// <summary>
        /// Deletes settings
        /// </summary>
        /// <param name="settings">Settings</param>
        public virtual void DeleteSettings(IList<Setting> settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            _settingRepository.Delete(settings);

            //cache
            _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);

            //event notification
            foreach (var setting in settings)
            {
                _eventPublisher.EntityDeleted(setting);
            }
        }

        /// <summary>
        /// Gets a setting by identifier
        /// </summary>
        /// <param name="settingId">Setting identifier</param>
        /// <returns>Setting</returns>
        public virtual Setting GetSettingById(int settingId)
        {
            if (settingId == 0)
                return null;

            return _settingRepository.GetById(settingId);
        }

        /// <summary>
        /// Get setting by key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="TenantId">Tenant identifier</param>
        /// <param name="loadSharedValueIfNotFound">A value indicating whether a shared (for all Tenants) value should be loaded if a value specific for a certain is not found</param>
        /// <returns>Setting</returns>
        public virtual Setting GetSetting(string key, int TenantId = 0, bool loadSharedValueIfNotFound = false)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            var settings = GetAllSettingsCached();
            key = key.Trim().ToLowerInvariant();
            if (settings.ContainsKey(key))
            {
                var settingsByKey = settings[key];
                var setting = settingsByKey.FirstOrDefault(x => x.TenantId == TenantId);

                //load shared value?
                if (setting == null && TenantId > 0 && loadSharedValueIfNotFound)
                    setting = settingsByKey.FirstOrDefault(x => x.TenantId == 0);

                if (setting != null)
                    return GetSettingById(setting.Id);
            }

            return null;
        }

        /// <summary>
        /// Get setting value by key
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="defaultValue">Default value</param>
        /// <param name="TenantId">Tenant identifier</param>
        /// <param name="loadSharedValueIfNotFound">A value indicating whether a shared (for all Tenants) value should be loaded if a value specific for a certain is not found</param>
        /// <returns>Setting value</returns>
        public virtual T GetSettingByKey<T>(string key, T defaultValue = default(T), 
            int TenantId = 0, bool loadSharedValueIfNotFound = false)
        {
            if (string.IsNullOrEmpty(key))
                return defaultValue;

            var settings = GetAllSettingsCached();
            key = key.Trim().ToLowerInvariant();
            if (settings.ContainsKey(key))
            {
                var settingsByKey = settings[key];
                var setting = settingsByKey.FirstOrDefault(x => x.TenantId == TenantId);

                //load shared value?
                if (setting == null && TenantId > 0 && loadSharedValueIfNotFound)
                    setting = settingsByKey.FirstOrDefault(x => x.TenantId == 0);

                if (setting != null)
                    return CommonHelper.To<T>(setting.Value);
            }

            return defaultValue;
        }

        /// <summary>
        /// Set setting value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="TenantId">Tenant identifier</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        public virtual void SetSetting<T>(string key, T value, int TenantId = 0, bool clearCache = true)
        {
            SetSetting(typeof(T), key, value, TenantId, clearCache);
        }

        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Settings</returns>
        public virtual IList<Setting> GetAllSettings()
        {
            var query = from s in _settingRepository.Table
                        orderby s.Name, s.TenantId
                        select s;
            var settings = query.ToList();
            return settings;
        }

        /// <summary>
        /// Determines whether a setting exists
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="settings">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="TenantId">Tenant identifier</param>
        /// <returns>true -setting exists; false - does not exist</returns>
        public virtual bool SettingExists<T, TPropType>(T settings, 
            Expression<Func<T, TPropType>> keySelector, int TenantId = 0) 
            where T : ISettings, new()
        {
            var key = settings.GetSettingKey(keySelector);

            var setting = GetSettingByKey<string>(key, TenantId: TenantId);
            return setting != null;
        }

        /// <summary>
        /// Load settings
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="TenantId">Tenant identifier for which settings should be loaded</param>
        public virtual T LoadSetting<T>(int TenantId = 0) where T : ISettings, new()
        {
            return (T)LoadSetting(typeof(T), TenantId);
        }
        /// <summary>
        /// Load settings
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="TenantId">Tenant identifier for which settings should be loaded</param>
        public virtual ISettings LoadSetting(Type type, int TenantId = 0)
        {
            var settings = Activator.CreateInstance(type);

            foreach (var prop in type.GetProperties())
            {
                // get properties we can read and write to
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                var key = type.Name + "." + prop.Name;
                //load by Tenant
                var setting = GetSettingByKey<string>(key, TenantId: TenantId, loadSharedValueIfNotFound: true);
                if (setting == null)
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).IsValid(setting))
                    continue;

                var value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(setting);

                //set property
                prop.SetValue(settings, value, null);
            }

            return settings as ISettings;
        }

        /// <summary>
        /// Save settings object
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="TenantId">Tenant identifier</param>
        /// <param name="settings">Setting instance</param>
        public virtual void SaveSetting<T>(T settings, int TenantId = 0) where T : ISettings, new()
        {
            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            foreach (var prop in typeof(T).GetProperties())
            {
                // get properties we can read and write to
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;
                
                var key = typeof(T).Name + "." + prop.Name;
                var value = prop.GetValue(settings, null);
                if (value != null)
                    SetSetting(prop.PropertyType, key, value, TenantId, false);
                else
                    SetSetting(key, "", TenantId, false);
            }

            //and now clear cache
            ClearCache();
        }

        /// <summary>
        /// Save settings object
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="settings">Settings</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="TenantId">Tenant ID</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        public virtual void SaveSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector,
            int TenantId = 0, bool clearCache = true) where T : ISettings, new()
        {
            var member = keySelector.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    keySelector));
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format(
                       "Expression '{0}' refers to a field, not a property.",
                       keySelector));
            }

            var key = settings.GetSettingKey(keySelector);
            var value = (TPropType)propInfo.GetValue(settings, null);
            if (value != null)
                SetSetting(key, value, TenantId, clearCache);
            else
                SetSetting(key, "", TenantId, clearCache);
        }

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
        public virtual void SaveSettingOverridablePerTenant<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector,
            bool overrideForTenant, int TenantId = 0, bool clearCache = true) where T : ISettings, new()
        {
            if (overrideForTenant || TenantId == 0)
                SaveSetting(settings, keySelector, TenantId, clearCache);
            else if (TenantId > 0)
                DeleteSetting(settings, keySelector, TenantId);
        }

        /// <summary>
        /// Delete all settings
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        public virtual void DeleteSetting<T>() where T : ISettings, new()
        {
            var settingsToDelete = new List<Setting>();
            var allSettings = GetAllSettings();
            foreach (var prop in typeof(T).GetProperties())
            {
                var key = typeof(T).Name + "." + prop.Name;
                settingsToDelete.AddRange(allSettings.Where(x => x.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase)));
            }

            DeleteSettings(settingsToDelete);
        }

        /// <summary>
        /// Delete settings object
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="settings">Settings</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="TenantId">Tenant ID</param>
        public virtual void DeleteSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector, int TenantId = 0) where T : ISettings, new()
        {
            var key = settings.GetSettingKey(keySelector);
            key = key.Trim().ToLowerInvariant();

            var allSettings = GetAllSettingsCached();
            var settingForCaching = allSettings.ContainsKey(key) ?
                allSettings[key].FirstOrDefault(x => x.TenantId == TenantId) : null;
            if (settingForCaching != null)
            {
                //update
                var setting = GetSettingById(settingForCaching.Id);
                DeleteSetting(setting);
            }
        }

        /// <summary>
        /// Clear cache
        /// </summary>
        public virtual void ClearCache()
        {
            _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);
        }

        #endregion
    }
}