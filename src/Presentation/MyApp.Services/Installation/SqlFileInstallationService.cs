using MyApp.Core.Abstractions.Data;
using MyApp.Core.Abstractions.Infrastructure;

namespace MyApp.Services.Installation
{
    /// <summary>
    /// Installation service using SQL files (fast installation)
    /// </summary>
    public class SqlFileInstallationService : IInstallationService
    {
        #region Fields

        private readonly ISqlExecutor _sqlExecutor;
        private readonly IMyAppFileProvider _fileProvider;

        #endregion

        #region Ctor

        public SqlFileInstallationService(ISqlExecutor sqlExecutor,
                                          IMyAppFileProvider fileProvider)
        {
            _sqlExecutor = sqlExecutor;
            _fileProvider = fileProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Install data
        /// </summary>
        /// <param name="installSampleData">A value indicating whether to install sample data</param>
        public virtual void InstallData(bool installSampleData = true)
        {
            _sqlExecutor.ExecuteSqlFile(_fileProvider.MapPath(InstallationDefaults.RequiredDataPath));
            if (installSampleData)
            {
                _sqlExecutor.ExecuteSqlFile(_fileProvider.MapPath(InstallationDefaults.SampleDataPath));
            }
        }

       #endregion
    }
}