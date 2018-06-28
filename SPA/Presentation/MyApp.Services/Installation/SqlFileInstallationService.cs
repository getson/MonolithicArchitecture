using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MyApp.Core.Infrastructure;
using MyApp.Core.Interfaces.Data;
using MyApp.Core.Interfaces.Infrastructure;
using MyApp.Core.Interfaces.Web;
using MyApp.Infrastructure.Data;

namespace MyApp.Services.Installation
{
    /// <summary>
    /// Installation service using SQL files (fast installation)
    /// </summary>
    public partial class SqlFileInstallationService : IInstallationService
    {
        #region Fields
        private readonly IDbContext _dbContext;
        private readonly IWebHelper _webHelper;
        private readonly IMyAppFileProvider _fileProvider;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="dbContext">DB context</param>
        /// <param name="webHelper">Web helper</param>
        /// <param name="fileProvider">File provider</param>
        public SqlFileInstallationService(IDbContext dbContext,
            IWebHelper webHelper,
            IMyAppFileProvider fileProvider)
        {
            _dbContext = dbContext;
            _webHelper = webHelper;
            _fileProvider = fileProvider;
        }

        #endregion

        #region Utilities

     
     /// <summary>
        /// Execute SQL file
        /// </summary>
        /// <param name="path">File path</param>
        protected virtual void ExecuteSqlFile(string path)
        {
            var statements = new List<string>();
            
            using (var reader = new StreamReader(path))
            {
                string statement;
                while ((statement = ReadNextStatementFromStream(reader)) != null)
                    statements.Add(statement);
            }

            foreach (var stmt in statements)
                _dbContext.ExecuteSqlCommand(stmt);
        }

        /// <summary>
        /// Read next statement from stream
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <returns>Result</returns>
        protected virtual string ReadNextStatementFromStream(StreamReader reader)
        {
            var sb = new StringBuilder();
            while (true)
            {
                var lineOfText = reader.ReadLine();
                if (lineOfText == null)
                {
                    if (sb.Length > 0)
                        return sb.ToString();

                    return null;
                }

                if (lineOfText.TrimEnd().ToUpper() == "GO")
                    break;

                sb.Append(lineOfText + Environment.NewLine);
            }

            return sb.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Install data
        /// </summary>
        /// <param name="defaultUserEmail">Default user email</param>
        /// <param name="defaultUserPassword">Default user password</param>
        /// <param name="installSampleData">A value indicating whether to install sample data</param>
        public virtual void InstallData(string defaultUserEmail,
            string defaultUserPassword, bool installSampleData = true)
        {
            ExecuteSqlFile(_fileProvider.MapPath("~/App_Data/Install/Fast/create_required_data.sql"));
        
            if (installSampleData)
            {
                ExecuteSqlFile(_fileProvider.MapPath("~/App_Data/Install/Fast/create_sample_data.sql"));
            }
        }

        #endregion
    }
}