namespace MyApp.Core.Abstractions.Data
{
    /// <summary>
    /// Represents a data provider manager
    /// </summary>
    public interface IDataProviderManager
    {
        #region Properties

        /// <summary>
        /// Gets data provider
        /// </summary>
        IDataProvider GetDataProvider();

        #endregion
    }
}