using MyApp.Core;
using MyApp.Core.Domain.Common;
using MyApp.Core.Domain.Localization;

namespace MyApp.Core.Domain.Configuration
{
    /// <summary>
    /// Represents a setting
    /// </summary>
    public partial class Setting : BaseEntity, ILocalizedEntity,ISettings
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public Setting()
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        /// <param name="tenantId">Tenant identifier</param>
        public Setting(string name, string value, int tenantId = 0) 
        {
            Name = name;
            Value = value;
            TenantId = tenantId;
        }
        
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the Tenant for which this setting is valid. 0 is set when the setting is for all Tenants
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>Result</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
