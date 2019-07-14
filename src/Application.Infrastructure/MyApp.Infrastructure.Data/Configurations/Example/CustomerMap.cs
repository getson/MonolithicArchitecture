using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Domain.Example.CustomerAgg;

namespace MyApp.Infrastructure.Data.Configurations.Example
{
    public class CustomerMap : MyAppEntityTypeConfiguration<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.OwnsOne(x => x.Address);

            base.Configure(builder);

        }
    }
}
