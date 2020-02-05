using Autofac;
using BinaryOrigin.SeedWork.Persistence.Ef;
using BinaryOrigin.SeedWork.Persistence.Ef.MySql;
using Microsoft.EntityFrameworkCore;

namespace BinaryOrigin.SeedWork.Core
{
    public static class MySqlDataProviderExtensions
    {
        public static void AddDefaultMySqlDbContext(this IEngine engine, string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EfObjectContext>();
            optionsBuilder.UseMySQL(connectionString);

            engine.Register(builder =>
            {
                builder.RegisterType<MySqlDataProvider>().As<IDataProvider>().SingleInstance();
                builder.Register(instance => new EfObjectContext(optionsBuilder.Options)).As<IDbContext>().InstancePerLifetimeScope();
            });
        }

        public static void AddMySqlDbExceptionParser(this IEngine engine, DbErrorMessagesConfiguration errorMessagesConfig)
        {
            engine.Register(builder =>
            {
                builder.Register(x => new MySqlDbExeptionParserProvider(errorMessagesConfig))
                       .As<IDbExceptionParserProvider>()
                       .SingleInstance();
            });
        }
    }
}