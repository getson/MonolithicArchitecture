using System;
using MyApp.Core.SharedKernel.Entities;

namespace MyApp.Core.Domain.ActivityLog
{
    /// <summary>
    /// Represents an activity log record
    /// </summary>
    public class ActivityLog : AggregateRoot
    {
        /// <summary>
        /// Gets or sets the activity log type identifier
        /// </summary>
        public int ActivityLogTypeId { get; set; }

        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public int? EntityId { get; set; }

        /// <summary>
        /// Gets or sets the entity name
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the activity comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }
        public virtual User.User User { get; set; }
        /// <summary>
        /// Gets the activity log type
        /// </summary>
        public virtual ActivityLogType ActivityLogType { get; set; }

        ///// <summary>
        ///// Gets the user
        ///// </summary>
        //public virtual user user { get; set; }

        /// <summary>
        /// Gets or sets the IP address
        /// </summary>
        public virtual string IpAddress { get; set; }
    }
}