using System;
using System.Text;
using MyApp.Core.Abstractions.Infrastructure;
using MyApp.Core.Configuration;
using MyApp.Core.Infrastructure;
using MyApp.Core.SharedKernel;
using Newtonsoft.Json;

namespace MyApp.Infrastructure.Data
{
    /// <summary>
    /// Represents the data settings manager
    /// </summary>
    public class DataSettingsManager
    {
        #region Fields

        private bool? _databaseIsInstalled;
        private bool? _databaseUpdated;
        private DataSettings _dataSettings;
        public static readonly DataSettingsManager Instance;
        #endregion

        static DataSettingsManager()
        {
            Instance = new DataSettingsManager();
        }

        #region Methods

        /// <summary>
        /// Load data settings
        /// </summary>
        /// <param name="filePath">File path; pass null to use the default settings file</param>
        /// <param name="reloadSettings">Whether to reload data, if they already loaded</param>
        /// <param name="fileProvider">file provider</param>
        /// <returns>Data settings</returns>
        public DataSettings LoadSettings(string filePath = null, bool reloadSettings = false, IMyAppFileProvider fileProvider = null)
        {
            if (!reloadSettings && _dataSettings != null)
            {
                return _dataSettings;
            }

            fileProvider = fileProvider ?? EngineContext.Current.FileProvider;
            filePath = filePath ?? fileProvider.MapPath(DefaultConfiguration.DataSettingsFilePath);

            if (!fileProvider.FileExists(filePath))
            {
                return new DataSettings();
            }

            var text = fileProvider.ReadAllText(filePath, Encoding.UTF8);

            if (string.IsNullOrEmpty(text))
            {
                return new DataSettings();
            }

            //get data settings from the JSON file
            _dataSettings = JsonConvert.DeserializeObject<DataSettings>(text);

            return _dataSettings;
        }

        /// <summary>
        /// Save data settings to the file
        /// </summary>
        /// <param name="settings">Data settings</param>
        /// <param name="fileProvider">File provider</param>
        public void SaveSettings(DataSettings settings, IMyAppFileProvider fileProvider)
        {
            _dataSettings = settings ?? throw new ArgumentNullException(nameof(settings));

            var filePath = fileProvider.MapPath(DefaultConfiguration.DataSettingsFilePath);

            //create file if not exists
            fileProvider.CreateFile(filePath);

            //save data settings to the file
            var text = JsonConvert.SerializeObject(_dataSettings, Formatting.Indented);
            fileProvider.WriteAllText(filePath, text, Encoding.UTF8);
        }

        /// <summary>
        /// Reset "database is installed" cached information
        /// </summary>
        public void ResetCache()
        {
            _databaseIsInstalled = null;
            _databaseUpdated = null;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether database is already installed
        /// </summary>
        public bool DatabaseIsInstalled
        {
            get
            {
                if (_databaseIsInstalled.HasValue)
                {
                    return _databaseIsInstalled.Value;
                }

                var settings = LoadSettings(reloadSettings: true);

                _databaseIsInstalled = !string.IsNullOrEmpty(settings?.DataConnectionString);

                return _databaseIsInstalled.Value;
            }
        }

        public bool DatabaseIsUpdated
        {
            get
            {
                if (_databaseUpdated.HasValue)
                {
                    return _databaseUpdated.Value;
                }
                var dbContext = EngineContext.Current.Resolve<IDbContext>();
                _databaseUpdated = dbContext.IsDatabaseUpdated();
                return _databaseUpdated.Value;
            }
        }

        #endregion
    }
}