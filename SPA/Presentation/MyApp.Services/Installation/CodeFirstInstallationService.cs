using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using MyApp.Core.Domain.Logging;
using MyApp.Core.Infrastructure;
using MyApp.Core.Interfaces.Data;
using MyApp.Core.Interfaces.Infrastructure;
using MyApp.Core.Interfaces.Web;

namespace MyApp.Services.Installation
{
    /// <summary>
    /// Code first installation service
    /// </summary>
    public partial class CodeFirstInstallationService : IInstallationService
    {
        #region Fields

        private readonly IRepository<ActivityLogType> _activityLogTypeRepository;
        private readonly IWebHelper _webHelper;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMyAppFileProvider _fileProvider;

        #endregion

        #region Ctor

        public CodeFirstInstallationService( IWebHelper webHelper,
                IHostingEnvironment hostingEnvironment,
                IMyAppFileProvider fileProvider,
                IRepository<ActivityLogType> activityTypeRepository)
        {
            _webHelper = webHelper;
            _hostingEnvironment = hostingEnvironment;
            _fileProvider = fileProvider;
            _activityLogTypeRepository = activityTypeRepository;
        }

        #endregion

        #region Utilities
        protected virtual void InstallActivityLogTypes()
        {
            var activityLogTypes = new List<ActivityLogType>
            {
                //admin area activities
                new ActivityLogType
                {
                    SystemKeyword = "AddNewAttribute",
                    Enabled = true,
                    Name = "Add a new attribute"
                },           
            };
            _activityLogTypeRepository.Insert(activityLogTypes);
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
         
            InstallActivityLogTypes();


            if (installSampleData)
            {
                //InstallActivityLog(defaultUserEmail);
            }
        }

        #endregion
    }
}