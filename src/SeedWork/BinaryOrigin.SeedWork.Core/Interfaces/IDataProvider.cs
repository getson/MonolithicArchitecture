using System.Data.Common;

namespace BinaryOrigin.SeedWork.Core
{
    /// <summary>
    /// Represents a data provider
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// Initialize database
        /// </summary>
        void InitializeDatabase();
        /// <summary>
        /// Get a support database parameter object (used by stored procedures)
        /// </summary>
        /// <returns>Parameter</returns>
        DbParameter GetParameter();
        void UpdateDatabase();
    }
}