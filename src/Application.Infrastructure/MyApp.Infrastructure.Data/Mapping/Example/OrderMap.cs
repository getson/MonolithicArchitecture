﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Domain.Example.OrderAgg;

namespace MyApp.Infrastructure.Data.Mapping.Example
{
    public class OrderMap : MyAppEntityTypeConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(x => x.ShippingInformation);
            builder.HasMany(x=>x.OrderLines);
            base.Configure(builder);
        }
    }
}
