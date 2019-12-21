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
        private readonly IDbContext _dbContext;

        public MySqlDataProvider(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Initialize database
        /// </summary>
        public virtual void InitializeDatabase()
        {
            _dbContext.CreateDatabase();
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
        protected virtual string MySqlScriptUpgradePath => "~/App_Data/Upgrade";

        public void UpdateDatabase()
        {
            //update schema
            _dbContext.UpdateDatabase();

            ExecuteUpgradeScripts(_dbContext);
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