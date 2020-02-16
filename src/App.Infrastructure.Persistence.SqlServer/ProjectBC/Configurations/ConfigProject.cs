using App.Core.Domain.ProjectBC;
using BinaryOrigin.SeedWork.Persistence.Ef;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Persistence.SqlServer.ProjectBC.Configurations
{
    internal class ConfigProject : EfEntityTypeConfiguration<Project>
    {
        public override void PostConfigure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(300).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);

            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}