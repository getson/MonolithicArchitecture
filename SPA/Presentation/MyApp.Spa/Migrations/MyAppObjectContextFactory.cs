using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MyApp.Infrastructure.Data;

namespace MyApp.Spa.Migrations
{
    /// <summary>
    /// factory for implementing design time creation of db context
    /// </summary>
    public class MyAppObjectContextFactory : IDesignTimeDbContextFactory<MyAppObjectContext>
    {
        /// <summary>
        /// create design time DbContext
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual MyAppObjectContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyAppObjectContext>();
            const string connectionString = "Data Source=GETSON-SERVER\\SQL2016;Initial Catalog=MyApp;Integrated Security=False;Persist Security Info=False;User ID=sa;Password=bosh";

            optionsBuilder.UseSqlServer(connectionString, x =>
            {
                x.MigrationsAssembly("MyApp.Infrastructure.Data.Migrations");
                x.MigrationsHistoryTable("MigrationHistory");   
            });
            
            return new MyAppObjectContext(optionsBuilder.Options);
        }
    }

}