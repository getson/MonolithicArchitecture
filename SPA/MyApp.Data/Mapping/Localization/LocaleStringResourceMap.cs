using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Core.Domain.Localization;

namespace MyApp.Infrastructure.Data.Mapping.Localization
{
    /// <summary>
    /// Represents a locale string resource mapping configuration
    /// </summary>
    public partial class LocaleStringResourceMap : MyAppEntityTypeConfiguration<LocaleStringResource>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<LocaleStringResource> builder)
        {
            builder.ToTable(nameof(LocaleStringResource));
            builder.HasKey(locale => locale.Id);

            builder.Property(locale => locale.ResourceName).HasMaxLength(200).IsRequired();
            builder.Property(locale => locale.ResourceValue).IsRequired();

            builder.HasOne(locale => locale.Language)
                .WithMany()
                .HasForeignKey(locale => locale.LanguageId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}