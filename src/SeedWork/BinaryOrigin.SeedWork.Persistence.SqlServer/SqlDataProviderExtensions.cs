using Autofac;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Persistence.Ef;
using Microsoft.EntityFrameworkCore;

namespace BinaryOrigin.SeedWork.Persistence.SqlServer
{
    public static class SqlDataProviderExtensions
    {
        public static void AddDefaultSqlDbContext(this IEngine engine, string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EfObjectContext>();
            optionsBuilder.UseSqlServer(connectionString);

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
        /// <summary>
        /// Add default sql server exception parser which will be executed in case of a DbUpdateException,
        /// The goal of this parser is to build a human error message based on constraint names and error message;  
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="errorMessagesConfig"></param>
        public static void AddSqlServerDbExceptionParser(this IEngine engine, DbErrorMessagesConfiguration errorMessagesConfig)
        {
            engine.Register(builder =>
            {
                builder.Register(x => new SqlServerDbExceptionParserProvider(errorMessagesConfig))
                       .As<IDbExceptionParserProvider>()
                       .SingleInstance();
            });
        }
    }
}