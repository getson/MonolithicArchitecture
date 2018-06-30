using MyApp.Core.Configuration;
using MyApp.Core.Exceptions;
using MyApp.Core.Interfaces.Data;
using MyApp.Infrastructure.Data.Providers;

namespace MyApp.Infrastructure.Data
{
    /// <summary>
    /// Represents the Entity Framework data provider manager
    /// </summary>
    public class EfDataProviderManager : IDataProviderManager
    {
        #region Properties

        /// <summary>
        /// Gets data provider
        /// </summary>
        public IDataProvider DataProvider
        {
            get
            {
                var providerName = DataSettingsManager.LoadSettings()?.DataProvider;
                switch (providerName)
                {
                    case DataProviderType.SqlServer:
                        return new SqlServerDataProvider();

                    //starting version 4.10 we support MS SQL Server only. SQL Server Compact is not supported anymore
                    //but we leave this code because we plan to support other databases soon (e.g. MySQL)

                    //case "sqlce":
                    //    return new SqlCeDataProvider();

                    default:
                        throw new MyAppException($"Not supported data provider name: '{providerName}'");
                }
            }
        }

        #endregion
    }
}