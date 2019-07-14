using System.Data.Common;
using System.Data.SqlClient;
using MyApp.Core.Abstractions.Data;
using MyApp.Core.Abstractions.Infrastructure;
using MyApp.Core.Infrastructure;
using MyApp.Infrastructure.Data.Extensions;

namespace MyApp.Infrastructure.Data.Providers
{
    /// <summary>
    /// Represents SQL Server data provider
    /// </summary>
    public class SqlServerDataProvider : IDataProvider
    {
        #region Methods

        /// <summary>
        /// Initialize database
        /// </summary>
        public virtual void InitializeDatabase()
        {

            var context = EngineContext.Current.Resolve<IDbContext>();
            var fileProvider = EngineContext.Current.Resolve<IMyAppFileProvider>();

            context.CreateDatabase();

            //create indexes
            context.ExecuteSqlScriptFromFile(fileProvider.MapPath(SqlServerIndexesFilePath));

            //create Stored procedures 
            context.ExecuteSqlScriptFromFile(fileProvider.MapPath(SqlServerStoredProceduresFilePath));
        }

        /// <summary>
        /// Get a support database parameter object (used by Stored procedures)
        /// </summary>
        /// <returns>Parameter</returns>
        public virtual DbParameter GetParameter()
        {
            return new SqlParameter();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this data provider supports backup
        /// </summary>
        public virtual bool BackupSupported => true;

        /// <summary>
        /// Gets a maximum length of the data for HASHBYTES functions, returns 0 if HASHBYTES function is not supported
        /// </summary>
        public virtual int SupportedLengthOfBinaryHash => 8000; //for SQL Server 2008 and above HASHBYTES function has a limit of 8000 characters.

        public void UpdateDatabase()
        {
            var context = EngineContext.Current.Resolve<IDbContext>();


            //update schema
            context.UpdateDatabase();

            ExecuteUpgradeScripts(context);
        }

        private void ExecuteUpgradeScripts(IDbContext context)
        {
            var fileProvider = EngineContext.Current.Resolve<IMyAppFileProvider>();
            var files = fileProvider.GetFiles(fileProvider.MapPath(SqlServerScriptUpgradePath), "*.sql", false);
            foreach (var sqlFile in files)
            {
                context.ExecuteSqlScriptFromFile(sqlFile);
            }
        }

        /// <summary>
        /// Gets a path to the file that contains script to create SQL Server indexes
        /// </summary>
        protected virtual string SqlServerIndexesFilePath => "~/App_Data/Install/SqlServer.Indexes.sql";

        /// <summary>
        /// Gets a path to the file that contains script to create SQL Server Stored procedures
        /// </summary>
        protected virtual string SqlServerStoredProceduresFilePath => "~/App_Data/Install/SqlServer.StoredProcedures.sql";
        protected virtual string SqlServerScriptUpgradePath => "~/App_Data/Upgrade";

        #endregion
    }
}