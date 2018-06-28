using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Core.Domain.Example.BankAccountAgg;

namespace MyApp.Infrastructure.Data.Mapping.Example
{
    public partial class BankAccountMapping : MyAppEntityTypeConfiguration<BankAccount>
    {
        public override void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.ToTable(typeof(BankAccount).Name)
                   .OwnsOne(c => c.BankAccountNumber);
            base.Configure(builder);
        }
    }
}
