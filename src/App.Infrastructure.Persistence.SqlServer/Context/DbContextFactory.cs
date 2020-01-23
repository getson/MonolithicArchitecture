using App.Infrastructure.Persistence.SqlServer.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace App.Persistence.Context
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
            optionsBuilder.UseSqlServer(configuration["Db:ConnectionString"], x =>
            {
                x.MigrationsAssembly("App.Infrastructure.Persistence.SqlServer");
                x.MigrationsHistoryTable("MigrationHistory");
            });
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}