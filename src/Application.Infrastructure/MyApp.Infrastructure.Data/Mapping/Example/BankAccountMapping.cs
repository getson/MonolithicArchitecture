﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Domain.Example.BankAccountAgg;

namespace MyApp.Infrastructure.Data.Mapping.Example
{
    public class BankAccountMapping : MyAppEntityTypeConfiguration<BankAccount>
    {
        public override void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.OwnsOne(c => c.BankAccountNumber);
            base.Configure(builder);
        }
    }
}
