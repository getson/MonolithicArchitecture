using BinaryOrigin.SeedWork.Core.Configuration;
using BinaryOrigin.SeedWork.Persistence.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

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
            //create instance of config
            var appConfig = new AppConfiguration();

            //bind it to the appropriate section of configuration
            configuration.GetSection("App").Bind(appConfig);
            optionsBuilder.UseSqlServer(appConfig.DbConnectionString, x =>
            {
                x.MigrationsAssembly("App.Persistence");
                x.MigrationsHistoryTable("MigrationHistory");
            });
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
