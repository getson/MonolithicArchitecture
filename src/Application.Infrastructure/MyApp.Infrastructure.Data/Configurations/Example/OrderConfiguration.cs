using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Domain.Example.OrderAgg;

namespace MyApp.Infrastructure.Data.Configurations.Example
{
    public class OrderConfiguration : MyAppEntityTypeConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(x => x.ShippingInformation);
            builder.HasMany(x=>x.OrderLines);
            base.Configure(builder);
        }
    }
}
