using Autofac;
using BinaryOrigin.SeedWork.Core;
using Microsoft.EntityFrameworkCore;

namespace BinaryOrigin.SeedWork.Persistence.Ef.MySql
{
    public static class MySqlDataProviderExtensions
    {
        public static void AddDefaultMySqlDbContext(this IEngine engine)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EfObjectContext>();
            optionsBuilder.UseMySQL(engine.Configuration.DbConnectionString);

            engine.Register(builder =>
            {
                builder.RegisterType<MySqlDataProvider>()
                        .As<IDataProvider>()
                        .InstancePerLifetimeScope();
                builder.Register(instance => new EfObjectContext(optionsBuilder.Options))
                    .As<IDbContext>()
                    .InstancePerLifetimeScope();
            });
        }
    }
}