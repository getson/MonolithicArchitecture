﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Core.Domain.Example.ProductAgg;

namespace MyApp.Infrastructure.Data.Mapping.Example
{
    public class ProductMap : MyAppEntityTypeConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(typeof(Product).Name);

            base.Configure(builder);
        }
    }
}
