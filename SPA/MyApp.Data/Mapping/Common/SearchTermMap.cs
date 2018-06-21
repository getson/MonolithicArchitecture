using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Core.Common;
using MyApp.Core.Domain.Common;

namespace MyApp.Infrastructure.Data.Mapping.Common
{
    /// <summary>
    /// Represents a search term mapping configuration
    /// </summary>
    public partial class SearchTermMap : MyAppEntityTypeConfiguration<SearchTerm>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<SearchTerm> builder)
        {
            builder.ToTable(nameof(SearchTerm));
            builder.HasKey(searchTerm => searchTerm.Id);

            base.Configure(builder);
        }

        #endregion
    }
}