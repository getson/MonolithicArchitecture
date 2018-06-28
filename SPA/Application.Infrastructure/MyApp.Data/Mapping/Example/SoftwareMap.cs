using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Core.Domain.Example.ProductAgg;

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
