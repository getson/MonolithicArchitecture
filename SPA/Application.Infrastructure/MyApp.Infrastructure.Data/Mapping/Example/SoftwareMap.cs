using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Domain.Example.ProductAgg;

namespace MyApp.Infrastructure.Data.Mapping.Example
{
    public class SoftwareMap:MyAppEntityTypeConfiguration<Software>
    {
        public override void Configure(EntityTypeBuilder<Software> builder)
        {
            builder.ToTable(typeof(Software).Name);
            base.Configure(builder);
        }
    }
}
