﻿using MyApp.Domain.Example.CountryAgg;

namespace MyApp.Infrastructure.Data.Repositories
{
    public class CountryRepository:EfRepository<Country>,ICountryRepository
    {
        public CountryRepository(IDbContext context) : base(context)
        {
        }
    }
}