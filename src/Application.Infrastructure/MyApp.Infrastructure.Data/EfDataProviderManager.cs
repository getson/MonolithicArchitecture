using MyApp.Core.Abstractions.Data;
using MyApp.Core.Configuration;
using MyApp.Core.Exceptions;
using MyApp.Infrastructure.Data.Providers;

namespace MyApp.Infrastructure.Data
{
    /// <summary>
    /// Represents the Entity Framework data provider manager
    /// </summary>
    public class EfDataProviderManager : IDataProviderManager
    {
        #region Properties

        /// <inheritdoc />
        /// <summary>
        /// Gets data provider
        /// </summary>
        public IDataProvider GetDataProvider()
        {
            var providerName = DataSettingsManager.Instance.LoadSettings()?.DataProvider;

            switch (providerName)
            {
                case DataProviderType.SqlServer:
                    return new SqlServerDataProvider();
                default:
                    throw new MyAppException($"Not supported data provider name: '{providerName}'");
            }

        }

        #endregion
    }
}