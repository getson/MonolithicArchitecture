using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Core.Domain.Common;

namespace MyApp.Infrastructure.Data.Mapping.Common
{
    /// <summary>
    /// Represents a decimal query type mapping configuration
    /// </summary>
    public class DecimalQueryTypeMap : MyAppQueryTypeConfiguration<DecimalQueryType>
    {
        #region Methods

        /// <summary>
        /// Configures the query type
        /// </summary>
        /// <param name="builder">The builder to be used to configure the query type</param>
        public override void Configure(QueryTypeBuilder<DecimalQueryType> builder)
        {
            builder.Property(decimalValue => decimalValue.Value).HasColumnType("decimal(18, 4)");

            base.Configure(builder);
        }

        #endregion
    }
}