using System;
using System.Collections.Generic;
using System.Linq;
using MyApp.Core.Common;
using MyApp.Core.Domain.ActivityLog;
using MyApp.Core.Extensions;
using MyApp.Core.Interfaces.Caching;
using MyApp.Core.Interfaces.Pagination;
using MyApp.Core.Interfaces.Web;
using MyApp.Core.SharedKernel.Entities;

namespace MyApp.Services.Logging
{
    //TODO refactoring...move quieries to repository
    /// <summary>
    /// User activity service
    /// </summary>
    public class UserActivityService : IUserActivityService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string ActivitytypeAllKey = "MyApp.activitytype.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string ActivitytypePatternKey = "MyApp.activitytype.";

        #endregion

        #region Fields

        private readonly IStaticCacheManager _cacheManager;
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly IActivityLogTypeRepository _activityLogTypeRepository;
        private readonly IWorkContext _workContext;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Static cache manager</param>
        /// <param name="activityLogRepository">Activity log repository</param>
        /// <param name="activityLogTypeRepository">Activity log type repository</param>
        /// <param name="workContext">Work context</param>
        /// >
        /// <param name="webHelper">Web helper</param>
        public UserActivityService(IStaticCacheManager cacheManager,
                                    IActivityLogRepository activityLogRepository,
                                    IActivityLogTypeRepository activityLogTypeRepository,
                                    IWorkContext workContext,
                                    IWebHelper webHelper)
        {
            _cacheManager = cacheManager;
            _activityLogRepository = activityLogRepository;
            _activityLogTypeRepository = activityLogTypeRepository;
            _workContext = workContext;
            _webHelper = webHelper;
        }

        #endregion

        #region Nested classes

        /// <summary>
        /// Activity log type for caching
        /// </summary>
        [Serializable]
        public class ActivityLogTypeForCaching
        {
            /// <summary>
            /// Identifier
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// System keyword
            /// </summary>
            public string SystemKeyword { get; set; }
            /// <summary>
            /// Name
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Enabled
            /// </summary>
            public bool Enabled { get; set; }
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Gets all activity log types (class for caching)
        /// </summary>
        /// <returns>Activity log types</returns>
        protected virtual IList<ActivityLogTypeForCaching> GetAllActivityTypesCached()
        {
            //cache
            var key = string.Format(ActivitytypeAllKey);
            return _cacheManager.Get(key, () =>
            {
                var result = new List<ActivityLogTypeForCaching>();
                var activityLogTypes = GetAllActivityTypes();
                foreach (var alt in activityLogTypes)
                {
                    var altForCaching = new ActivityLogTypeForCaching
                    {
                        Id = alt.Id,
                        SystemKeyword = alt.SystemKeyword,
                        Name = alt.Name,
                        Enabled = alt.Enabled
                    };
                    result.Add(altForCaching);
                }
                return result;
            });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inserts an activity log type item
        /// </summary>
        /// <param name="activityLogType">Activity log type item</param>
        public virtual void InsertActivityType(ActivityLogType activityLogType)
        {
            if (activityLogType == null)
                throw new ArgumentNullException(nameof(activityLogType));

            _activityLogTypeRepository.Insert(activityLogType);
            _cacheManager.RemoveByPattern(ActivitytypePatternKey);
        }

        /// <summary>
        /// Updates an activity log type item
        /// </summary>
        /// <param name="activityLogType">Activity log type item</param>
        public virtual void UpdateActivityType(ActivityLogType activityLogType)
        {
            if (activityLogType == null)
                throw new ArgumentNullException(nameof(activityLogType));

            _activityLogTypeRepository.Update(activityLogType);
            _cacheManager.RemoveByPattern(ActivitytypePatternKey);
        }

        /// <summary>
        /// Deletes an activity log type item
        /// </summary>
        /// <param name="activityLogType">Activity log type</param>
        public virtual void DeleteActivityType(ActivityLogType activityLogType)
        {
            if (activityLogType == null)
                throw new ArgumentNullException(nameof(activityLogType));

            _activityLogTypeRepository.Delete(activityLogType);
            _cacheManager.RemoveByPattern(ActivitytypePatternKey);
        }

        /// <summary>
        /// Gets all activity log type items
        /// </summary>
        /// <returns>Activity log type items</returns>
        public virtual IList<ActivityLogType> GetAllActivityTypes()
        {
            return _activityLogTypeRepository.GetAll().OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// Gets an activity log type item
        /// </summary>
        /// <param name="activityLogTypeId">Activity log type identifier</param>
        /// <returns>Activity log type item</returns>
        public virtual ActivityLogType GetActivityTypeById(int activityLogTypeId)
        {
            if (activityLogTypeId == 0)
                return null;

            return _activityLogTypeRepository.GetById(activityLogTypeId);
        }

        /// <summary>
        /// Inserts an activity log item
        /// </summary>
        /// <param name="systemKeyword">System keyword</param>
        /// <param name="comment">Comment</param>
        /// <param name="entity">Entity</param>
        /// <returns>Activity log item</returns>
        public virtual Core.Domain.ActivityLog.ActivityLog InsertActivity(string systemKeyword, string comment, BaseEntity entity = null)
        {
            return InsertActivity(_workContext.CurrentUser, systemKeyword, comment, entity);
        }

        /// <summary>
        /// Inserts an activity log item
        /// </summary>
        /// <param name="customer">User</param>
        /// <param name="systemKeyword">System keyword</param>
        /// <param name="comment">Comment</param>
        /// <param name="entity">Entity</param>
        /// <returns>Activity log item</returns>
        public virtual Core.Domain.ActivityLog.ActivityLog InsertActivity(Core.SharedKernel.User.User customer, string systemKeyword, string comment, BaseEntity entity = null)
        {
            if (customer == null)
                return null;

            //try to get activity log type by passed system keyword
            var activityLogType = GetAllActivityTypesCached().FirstOrDefault(type => type.SystemKeyword.Equals(systemKeyword));
            if (!activityLogType?.Enabled ?? true)
                return null;

            //insert log item
            var logItem = new Core.Domain.ActivityLog.ActivityLog
            {
                ActivityLogTypeId = activityLogType.Id,
                EntityId = entity?.Id,
                EntityName = entity?.GetUnproxiedEntityType().Name,
                UserId = customer.Id,
                Comment = CommonHelper.EnsureMaximumLength(comment ?? string.Empty, 4000),
                CreatedOnUtc = DateTime.UtcNow,
                IpAddress = _webHelper.GetCurrentIpAddress()
            };
            _activityLogRepository.Insert(logItem);

            return logItem;
        }

        /// <summary>
        /// Deletes an activity log item
        /// </summary>
        /// <param name="activityLog">Activity log type</param>
        public virtual void DeleteActivity(Core.Domain.ActivityLog.ActivityLog activityLog)
        {
            if (activityLog == null)
                throw new ArgumentNullException(nameof(activityLog));

            _activityLogRepository.Delete(activityLog);
        }
        //TODO specifications pattern
        /// <summary>
        /// Gets all activity log items
        /// </summary>
        /// <param name="createdOnFrom">Log item creation from; pass null to load all records</param>
        /// <param name="createdOnTo">Log item creation to; pass null to load all records</param>
        /// <param name="customerId">User identifier; pass null to load all records</param>
        /// <param name="activityLogTypeId">Activity log type identifier; pass null to load all records</param>
        /// <param name="ipAddress">IP address; pass null or empty to load all records</param>
        /// <param name="entityName">Entity name; pass null to load all records</param>
        /// <param name="entityId">Entity identifier; pass null to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Activity log items</returns>
        public virtual IPagedList<Core.Domain.ActivityLog.ActivityLog> GetAllActivities(DateTime? createdOnFrom = null, DateTime? createdOnTo = null,
            int? customerId = null, int? activityLogTypeId = null, string ipAddress = null, string entityName = null, int? entityId = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _activityLogRepository.Table;

            //filter by IP
            if (!string.IsNullOrEmpty(ipAddress))
                query = query.Where(logItem => logItem.IpAddress.Contains(ipAddress));

            //filter by creation date
            if (createdOnFrom.HasValue)
                query = query.Where(logItem => createdOnFrom.Value <= logItem.CreatedOnUtc);
            if (createdOnTo.HasValue)
                query = query.Where(logItem => createdOnTo.Value >= logItem.CreatedOnUtc);

            //filter by log type
            if (activityLogTypeId.HasValue && activityLogTypeId.Value > 0)
                query = query.Where(logItem => activityLogTypeId == logItem.ActivityLogTypeId);

            //filter by customer
            if (customerId.HasValue && customerId.Value > 0)
                query = query.Where(logItem => customerId.Value == logItem.UserId);

            //filter by entity
            if (!string.IsNullOrEmpty(entityName))
                query = query.Where(logItem => logItem.EntityName.Equals(entityName));
            if (entityId.HasValue && entityId.Value > 0)
                query = query.Where(logItem => entityId.Value == logItem.EntityId);

            query = query.OrderByDescending(logItem => logItem.CreatedOnUtc).ThenBy(logItem => logItem.Id);

            return new PagedList<Core.Domain.ActivityLog.ActivityLog>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets an activity log item
        /// </summary>
        /// <param name="activityLogId">Activity log identifier</param>
        /// <returns>Activity log item</returns>
        public virtual Core.Domain.ActivityLog.ActivityLog GetActivityById(int activityLogId)
        {
            return activityLogId == 0 ? null : _activityLogRepository.GetById(activityLogId);
        }

        #endregion
    }
}