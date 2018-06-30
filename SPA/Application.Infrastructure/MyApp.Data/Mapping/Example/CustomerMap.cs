using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Core.Domain.Example.CustomerAgg;

namespace MyApp.Infrastructure.Data.Mapping.Example
{
    public class CustomerMap : MyAppEntityTypeConfiguration<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable(typeof(Customer).Name)
                   .OwnsOne(x => x.Address);

            base.Configure(builder);

        }
    }
}
