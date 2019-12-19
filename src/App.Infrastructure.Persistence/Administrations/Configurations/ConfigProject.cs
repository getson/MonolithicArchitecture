using App.Core.Domain.Administrations;
using BinaryOrigin.SeedWork.Persistence.Ef;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Persistence.Administrations.Configurations
{
    internal class ConfigProject : EfEntityTypeConfiguration<Project>
    {
        public override void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.name).HasMaxLength(300).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);

            base.Configure(builder);
        }
    }
}