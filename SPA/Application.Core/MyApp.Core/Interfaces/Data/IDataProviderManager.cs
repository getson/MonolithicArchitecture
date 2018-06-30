namespace MyApp.Core.Interfaces.Data
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
        IDataProvider DataProvider { get; }
        
        #endregion
    }
}