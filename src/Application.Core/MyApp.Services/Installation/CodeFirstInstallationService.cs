using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using MyApp.Core.Abstractions.Infrastructure;
using MyApp.Core.Abstractions.Web;

namespace MyApp.Services.Installation
{
    /// <summary>
    /// Code first installation service
    /// </summary>
    public class CodeFirstInstallationService : IInstallationService
    {
        #region Fields
        private readonly IWebHelper _webHelper;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMyAppFileProvider _fileProvider;

        #endregion

        #region Ctor

        public CodeFirstInstallationService(IWebHelper webHelper,
                IHostingEnvironment hostingEnvironment,
                IMyAppFileProvider fileProvider)
        {
            _webHelper = webHelper;
            _hostingEnvironment = hostingEnvironment;
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

            if (installSampleData)
            {
                //InstallActivityLog(defaultUserEmail);
            }
        }

        #endregion
    }
}