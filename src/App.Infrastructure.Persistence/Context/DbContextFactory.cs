using App.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace App.Infrastructure.Persistence.Context
{
    public class AppObjectContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        /// <summary>
        /// create design time DbContext
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var dbConfig = new DbConfig();

            configuration.GetSection(nameof(DbConfig)).Bind(dbConfig);
            if (dbConfig.ProviderType == DataProviderType.InMemory)
            {
                optionsBuilder.UseInMemoryDatabase("InMemoryDb");
            }
            else
            {
                optionsBuilder.UseNpgsql(dbConfig.ConnectionString, x =>
                {
                    x.MigrationsAssembly(GetType().Assembly.GetName().Name);
                    x.MigrationsHistoryTable("__EFMigrationsHistory");
                });
            }

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}