using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Persistence.Ef.Extensions;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace BinaryOrigin.SeedWork.Persistence.Ef.MySql
{
    /// <summary>
    /// Represents My Sql data provider
    /// </summary>
    public class MySqlDataProvider : IDataProvider
    {
        /// <summary>
        /// Initialize database
        /// </summary>
        public virtual void InitializeDatabase()
        {
            var context = EngineContext.Current.Resolve<IDbContext>();
            var fileProvider = EngineContext.Current.Resolve<IAppFileProvider>();

            context.CreateDatabase();
        }

        /// <summary>
        /// Get a support database parameter object (used by Stored procedures)
        /// </summary>
        /// <returns>Parameter</returns>
        public virtual DbParameter GetParameter()
        {
            return new MySqlParameter();
        }

        /// <summary>
        /// Gets a value indicating whether this data provider supports backup
        /// </summary>
        public virtual bool BackupSupported => true;

        /// <summary>
        /// Gets a maximum length of the data for HASHBYTES functions, returns 0 if HASHBYTES function is not supported
        /// </summary>
        public virtual int SupportedLengthOfBinaryHash => 8000; //for My Sql 2008 and above HASHBYTES function has a limit of 8000 characters.

        protected virtual string MySqlScriptUpgradePath => "~/App_Data/Upgrade";

        public void UpdateDatabase()
        {
            var context = EngineContext.Current.Resolve<IDbContext>();

            //update schema
            context.UpdateDatabase();

            ExecuteUpgradeScripts(context);
        }

        private void ExecuteUpgradeScripts(IDbContext context)
        {
            var fileProvider = EngineContext.Current.Resolve<IAppFileProvider>();
            var files = fileProvider.GetFiles(fileProvider.MapPath(MySqlScriptUpgradePath), "*.sql", false);
            foreach (var sqlFile in files)
            {
                context.ExecuteSqlScriptFromFile(sqlFile);
            }
        }
    }
}