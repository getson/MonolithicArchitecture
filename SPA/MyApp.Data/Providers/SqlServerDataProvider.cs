﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using MyApp.Core.Common;
using MyApp.Core.Domain.Common;
using MyApp.Core.Infrastructure;
using MyApp.Core.Interfaces.Data;
using MyApp.Infrastructure.Data.Extensions;

namespace MyApp.Infrastructure.Data
{
    /// <summary>
    /// Represents SQL Server data provider
    /// </summary>
    public partial class SqlServerDataProvider : IDataProvider
    {
        #region Methods

        /// <summary>
        /// Initialize database
        /// </summary>
        public virtual void InitializeDatabase()
        {
            var context = EngineContext.Current.Resolve<IDbContext>();

            //check some of table names to ensure that we have MyApp 2.00+ installed
            var tableNamesToValidate = new List<string> { "Customer", "Discount", "Order", "Product", "ShoppingCartItem" };
            var existingTableNames = context
                .QueryFromSql<StringQueryType>("SELECT table_name AS Value FROM INFORMATION_SCHEMA.TABLES WHERE table_type = 'BASE TABLE'")
                .Select(stringValue => stringValue.Value).ToList();
            var createTables = !existingTableNames.Intersect(tableNamesToValidate, StringComparer.InvariantCultureIgnoreCase).Any();
            if (!createTables)
                return;

            var fileProvider = EngineContext.Current.Resolve<IMyAppFileProvider>();

            //create tables
            context.ExecuteSqlScript(context.GenerateCreateScript());

            //create indexes
            context.ExecuteSqlScriptFromFile(fileProvider.MapPath(SqlServerIndexesFilePath));

            //create Tenantd procedures 
            context.ExecuteSqlScriptFromFile(fileProvider.MapPath(SqlServerTenantdProceduresFilePath));
        }

        /// <summary>
        /// Get a support database parameter object (used by Tenantd procedures)
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

        /// <summary>
        /// Gets a path to the file that contains script to create SQL Server indexes
        /// </summary>
        protected virtual string SqlServerIndexesFilePath => "~/App_Data/Install/SqlServer.Indexes.sql";

        /// <summary>
        /// Gets a path to the file that contains script to create SQL Server Tenantd procedures
        /// </summary>
        protected virtual string SqlServerTenantdProceduresFilePath => "~/App_Data/Install/SqlServer.TenantdProcedures.sql";

        #endregion
    }
}