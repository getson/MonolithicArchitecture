using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Domain.ActivityLog;

namespace MyApp.Infrastructure.Data.Configurations.Logging
{
    /// <summary>
    /// Represents an activity log type mapping configuration
    /// </summary>
    public class ActivityLogTypeMap : MyAppEntityTypeConfiguration<ActivityLogType>
    {
       

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ActivityLogType> builder)
        {
            builder.HasKey(logType => logType.Id);

            builder.Property(logType => logType.SystemKeyword).HasMaxLength(100).IsRequired();
            builder.Property(logType => logType.Name).HasMaxLength(200).IsRequired();

            base.Configure(builder);
        }

 
    }
}