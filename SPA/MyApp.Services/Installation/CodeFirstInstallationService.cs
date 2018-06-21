using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using MyApp.Core.Domain.Common;
using MyApp.Core.Domain.Localization;
using MyApp.Core.Domain.Services.Configuration;
using MyApp.Core.Domain.Services.Localization;
using MyApp.Core.Domain.Services.Logging;
using MyApp.Core.Infrastructure;
using MyApp.Core.Infrastructure.Common;
using MyApp.Core.Interfaces.Data;

namespace MyApp.Core.Domain.Services.Installation
{
    /// <summary>
    /// Code first installation service
    /// </summary>
    public partial class CodeFirstInstallationService : IInstallationService
    {
        #region Fields

        private readonly IRepository<Language> _languageRepository;
        private readonly IRepository<ActivityLogType> _activityLogTypeRepository;
        private readonly IWebHelper _webHelper;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMyAppFileProvider _fileProvider;

        #endregion

        #region Ctor

        public CodeFirstInstallationService(IRepository<Language> languageRepository,
                IRepository<SearchTerm> searchTermRepository,
                IWebHelper webHelper,
                IHostingEnvironment hostingEnvironment,
                IMyAppFileProvider fileProvider,
                IRepository<ActivityLogType> activityTypeRepository)
        {
            _languageRepository = languageRepository;
            _webHelper = webHelper;
            _hostingEnvironment = hostingEnvironment;
            _fileProvider = fileProvider;
            _activityLogTypeRepository = activityTypeRepository;
        }

        #endregion

        #region Utilities

        protected virtual void InstallLanguages()
        {
            var language = new Language
            {
                Name = "English",
                LanguageCulture = "en-US",
                UniqueSeoCode = "en",
                FlagImageFileName = "us.png",
                Published = true,
                DisplayOrder = 1
            };
            _languageRepository.Insert(language);
        }

        protected virtual void InstallLocaleResources()
        {
            //'English' language
            var language = _languageRepository.Table.Single(l => l.Name == "English");

            //save resources
            foreach (var filePath in _fileProvider.EnumerateFiles(_fileProvider.MapPath("~/App_Data/Localization/"), "*.nopres.xml"))
            {
                var localesXml = _fileProvider.ReadAllText(filePath, Encoding.UTF8);
                var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
                localizationService.ImportResourcesFromXml(language, localesXml);
            }

        }

        protected virtual void InstallSettings(bool installSampleData)
        {
            var settingService = EngineContext.Current.Resolve<ISettingService>();
            settingService.SaveSetting(new PdfSettings
            {
                LogoPictureId = 0,
                LetterPageSizeEnabled = false,
                RenderOrderNotes = true,
                FontFileName = "FreeSerif.ttf",
                InvoiceFooterTextColumn1 = null,
                InvoiceFooterTextColumn2 = null,
            });

            settingService.SaveSetting(new CommonSettings
            {
                UseSystemEmailForContactUsForm = true,
                UseStoredProcedureForLoadingCategories = true,
                SitemapEnabled = true,
                SitemapPageSize = 200,
                SitemapIncludeCategories = true,
                SitemapIncludeManufacturers = true,
                SitemapIncludeProducts = false,
                SitemapIncludeProductTags = false,
                DisplayJavaScriptDisabledWarning = false,
                UseFullTextSearch = false,
                FullTextMode = FulltextSearchMode.ExactMatch,
                Log404Errors = true,
                BreadcrumbDelimiter = "/",
                RenderXuaCompatible = false,
                XuaCompatibleValue = "IE=edge",
                BbcodeEditorOpenLinksInNewWindow = false,
                PopupForTermsOfServiceLinks = true
            });



            settingService.SaveSetting(new LocalizationSettings
            {
                DefaultAdminLanguageId = _languageRepository.Table.Single(l => l.Name == "English").Id,
                UseImagesForLanguageSelection = false,
                SeoFriendlyUrlsForLanguagesEnabled = false,
                AutomaticallyDetectLanguage = false,
                LoadAllLocaleRecordsOnStartup = true,
                LoadAllLocalizedPropertiesOnStartup = true,
                LoadAllUrlRecordsOnStartup = false,
                IgnoreRtlPropertyForAdminArea = false
            });




            //settingService.SaveSetting(new ExternalAuthenticationSettings
            //{
            //    RequireEmailValidation = false,
            //    AllowusersToRemoveAssociations = true
            //});




            //settingService.SaveSetting(new SecuritySettings
            //{
            //    ForceSslForAllPages = true,
            //    EncryptionKey = CommonHelper.GenerateRandomDigitCode(16),
            //    AdminAreaAllowedIpAddresses = null,
            //    EnableXsrfProtectionForAdminArea = true,
            //    EnableXsrfProtectionForPublicStore = true,
            //    HoneypotEnabled = false,
            //    HoneypotInputName = "hpinput",
            //    AllowNonAsciiCharactersInHeaders = true
            //});


            settingService.SaveSetting(new DisplayDefaultMenuItemSettings
            {
                DisplayHomePageMenuItem = !installSampleData,
                DisplayNewProductsMenuItem = !installSampleData,
                DisplayProductSearchMenuItem = !installSampleData,
                DisplayUserInfoMenuItem = !installSampleData,
                DisplayBlogMenuItem = !installSampleData,
                DisplayForumsMenuItem = !installSampleData,
                DisplayContactUsMenuItem = !installSampleData
            });
            //settingService.SaveSetting(new CaptchaSettings
            //{
            //    ReCaptchaDefaultLanguage = "",
            //    AutomaticallyChooseLanguage = true
            //});
        }

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
            InstallLanguages();

            InstallSettings(installSampleData);
            InstallLocaleResources();
            InstallActivityLogTypes();


            if (installSampleData)
            {
                //InstallActivityLog(defaultUserEmail);
                //InstallSearchTerms();
            }
        }

        #endregion
    }
}