using MyApp.Core.Domain.Common;
using MyApp.Core.Domain.Configuration;

namespace MyApp.Web.Framework.Common
{
    /// <summary>
    /// DateTime settings
    /// </summary>
    public class DateTimeSettings : ISettings
    {
        /// <summary>
        /// Gets or sets a default Tenant time zone identifier
        /// </summary>
        public string DefaultTenantTimeZoneId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether customers are allowed to select theirs time zone
        /// </summary>
        public bool AllowCustomersToSetTimeZone { get; set; }
    }
}