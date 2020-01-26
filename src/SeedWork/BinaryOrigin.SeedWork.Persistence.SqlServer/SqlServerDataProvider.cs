using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Persistence.Ef;
using BinaryOrigin.SeedWork.Persistence.Ef.Extensions;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace BinaryOrigin.SeedWork.Persistence.SqlServer
{
    /// <summary>
    /// Represents SQL Server data provider
    /// </summary>
    public class SqlServerDataProvider : IDataProvider
    {
        /// <summary>
        /// Initialize database
        /// </summary>
        public virtual void InitializeDatabase()
        {
            var context = EngineContext.Current.Resolve<IDbContext>();
            var fileProvider = EngineContext.Current.Resolve<IAppFileProvider>();

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

        /// <summary>
        /// Gets a path to the file that contains script to create SQL Server indexes
        /// </summary>
        protected virtual string SqlServerIndexesFilePath => "~/App_Data/Install/SqlServer.Indexes.sql";

        /// <summary>
        /// Gets a path to the file that contains script to create SQL Server Stored procedures
        /// </summary>
        protected virtual string SqlServerStoredProceduresFilePath => "~/App_Data/Install/SqlServer.StoredProcedures.sql";

        protected virtual string SqlServerScriptUpgradePath => "~/App_Data/Upgrade";

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
            var files = fileProvider.GetFiles(fileProvider.MapPath(SqlServerScriptUpgradePath), "*.sql", false);
            foreach (var sqlFile in files)
            {
                context.ExecuteSqlScriptFromFile(sqlFile);
            }
        }
    }
}