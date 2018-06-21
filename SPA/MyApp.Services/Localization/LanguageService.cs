using System;
using System.Collections.Generic;
using System.Linq;
using MyApp.Core.Domain.Localization;
using MyApp.Core.Domain.Services.Configuration;
using MyApp.Core.Domain.Services.Events;
using MyApp.Core.Interfaces.Caching;
using MyApp.Core.Interfaces.Data;
using MyApp.Infrastructure.Cache;

namespace MyApp.Core.Domain.Services.Localization
{
    /// <summary>
    /// Language service
    /// </summary>
    public partial class LanguageService : ILanguageService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string LANGUAGES_BY_ID_KEY = "Nop.language.id-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        private const string LANGUAGES_ALL_KEY = "Nop.language.all-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string LANGUAGES_PATTERN_KEY = "Nop.language.";

        #endregion

        #region Fields

        private readonly IRepository<Language> _languageRepository;
        private readonly IStaticCacheManager _cacheManager;
        private readonly ISettingService _settingService;
        private readonly LocalizationSettings _localizationSettings;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="languageRepository">Language repository</param>
        /// <param name="storeMappingService">Store mapping service</param>
        /// <param name="settingService">Setting service</param>
        /// <param name="localizationSettings">Localization settings</param>
        /// <param name="eventPublisher">Event publisher</param>
        public LanguageService(IStaticCacheManager cacheManager,
            IRepository<Language> languageRepository,
            ISettingService settingService,
            LocalizationSettings localizationSettings,
            IEventPublisher eventPublisher)
        {
            _cacheManager = cacheManager;
            _languageRepository = languageRepository;
            _settingService = settingService;
            _localizationSettings = localizationSettings;
            _eventPublisher = eventPublisher;
        }

        #endregion
        
        #region Methods

        /// <summary>
        /// Deletes a language
        /// </summary>
        /// <param name="language">Language</param>
        public virtual void DeleteLanguage(Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            if (language is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            //update default admin area language (if required)
            if (_localizationSettings.DefaultAdminLanguageId == language.Id)
            {
                foreach (var activeLanguage in GetAllLanguages())
                {
                    if (activeLanguage.Id != language.Id)
                    {
                        _localizationSettings.DefaultAdminLanguageId = activeLanguage.Id;
                        _settingService.SaveSetting(_localizationSettings);
                        break;
                    }
                }
            }
            
            _languageRepository.Delete(language);

            //cache
            _cacheManager.RemoveByPattern(LANGUAGES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(language);
        }

        /// <summary>
        /// Gets all languages
        /// </summary>
        /// <param name="storeId">Load records allowed only in a specified store; pass 0 to load all records</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="loadCacheableCopy">A value indicating whether to load a copy that could be cached (workaround until Entity Framework supports 2-level caching)</param>
        /// <returns>Languages</returns>
        public virtual IList<Language> GetAllLanguages(bool showHidden = false, int storeId = 0, bool loadCacheableCopy = true)
        {
            Func<IList<Language>> loadLanguagesFunc = () =>
            {
                var query = _languageRepository.Table;
                if (!showHidden)
                    query = query.Where(l => l.Published);
                query = query.OrderBy(l => l.DisplayOrder).ThenBy(l => l.Id);
                return query.ToList();
            };

            IList<Language> languages;
            if (loadCacheableCopy)
            {
                //cacheable copy
                var key = string.Format(LANGUAGES_ALL_KEY, showHidden);
                languages = _cacheManager.Get(key, () =>
                {
                    var result = new List<Language>();
                    foreach (var language in loadLanguagesFunc())
                        result.Add(new LanguageForCaching(language));
                    return result;
                });
            }
            else
            {
                languages = loadLanguagesFunc();
            }
            return languages;
        }

        /// <summary>
        /// Gets a language
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <param name="loadCacheableCopy">A value indicating whether to load a copy that could be cached (workaround until Entity Framework supports 2-level caching)</param>
        /// <returns>Language</returns>
        public virtual Language GetLanguageById(int languageId, bool loadCacheableCopy = true)
        {
            if (languageId == 0)
                return null;

            Func<Language> loadLanguageFunc = () =>
            {
                return _languageRepository.GetById(languageId);
            };

            if (loadCacheableCopy)
            {
                //cacheable copy
                var key = string.Format(LANGUAGES_BY_ID_KEY, languageId);
                return _cacheManager.Get(key, () =>
                {
                    var language = loadLanguageFunc();
                    if (language == null)
                        return null;
                    return new LanguageForCaching(language);
                });
            }

            return loadLanguageFunc();
        }

        /// <summary>
        /// Inserts a language
        /// </summary>
        /// <param name="language">Language</param>
        public virtual void InsertLanguage(Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            if (language is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _languageRepository.Insert(language);

            //cache
            _cacheManager.RemoveByPattern(LANGUAGES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(language);
        }

        /// <summary>
        /// Updates a language
        /// </summary>
        /// <param name="language">Language</param>
        public virtual void UpdateLanguage(Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            if (language is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            //update language
            _languageRepository.Update(language);

            //cache
            _cacheManager.RemoveByPattern(LANGUAGES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(language);
        }

        #endregion
    }
}
