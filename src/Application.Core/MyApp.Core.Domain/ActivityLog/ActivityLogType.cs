using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyApp.SharedKernel.Domain;

namespace MyApp.Domain.ActivityLog
{
    /// <summary>
    /// Represents an activity log type record
    /// </summary>
    public class ActivityLogType : AggregateRoot
    {
        /// <summary>
        /// Gets or sets the system keyword
        /// </summary>
        public string SystemKeyword { get; set; }

        /// <summary>
        /// Gets or sets the display name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the activity log type is enabled
        /// </summary>
        public bool Enabled { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
