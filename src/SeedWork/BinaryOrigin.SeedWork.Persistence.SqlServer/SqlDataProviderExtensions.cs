using Autofac;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Persistence.Ef;
using Microsoft.EntityFrameworkCore;

namespace BinaryOrigin.SeedWork.Persistence.SqlServer
{
    public static class SqlDataProviderExtensions
    {
        public static void AddDefaultSqlDbContext(this IEngine engine)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EfObjectContext>();
            optionsBuilder.UseSqlServer(engine.Configuration.DbConnectionString);

            engine.Register(builder =>
            {
                builder.RegisterType<SqlServerDataProvider>()
                        .As<IDataProvider>()
                        .SingleInstance();
                builder.Register(instance => new EfObjectContext(optionsBuilder.Options))
                        .As<IDbContext>()
                        .InstancePerLifetimeScope();
            });
        }
        public static void AddSqlServerDbExceptionParser(this IEngine engine, DbErrorMessagesConfiguration errorMessagesConfig)
        {
            engine.Register(builder =>
            {
                builder.Register(x => new SqlServerDbExeptionParser(errorMessagesConfig))
                       .As<IDbExceptionParser>()
                       .SingleInstance();
            });
        }
    }
}