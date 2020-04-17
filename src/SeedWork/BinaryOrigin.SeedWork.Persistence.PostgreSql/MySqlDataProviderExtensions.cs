using Autofac;
using BinaryOrigin.SeedWork.Persistence.Ef;
using BinaryOrigin.SeedWork.Persistence.Ef.MySql;
using Microsoft.EntityFrameworkCore;
using System;

namespace BinaryOrigin.SeedWork.Core
{
    public static class MySqlDataProviderExtensions
    {
        public static void AddDbContext<TContext>(this IEngine engine, Func<TContext> @delegate)
            where TContext : IDbContext
        {
            engine.Register(builder =>
            {
                builder.RegisterType<NpgSqlDataProvider>()
                        .As<IDataProvider>()
                        .SingleInstance();
                builder.Register(instance => @delegate.Invoke())
                        .As<IDbContext>()
                        .InstancePerLifetimeScope();
            });
        }
        public static void AddDbContext<TContext>(this IEngine engine, string connectionString)
                 where TContext : EfObjectContext
        {
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            optionsBuilder.UseNpgsql(connectionString);

            engine.Register(builder =>
            {
                builder.RegisterType<NpgSqlDataProvider>()
                        .As<IDataProvider>()
                        .SingleInstance();
                builder.Register(instance => Activator.CreateInstance(typeof(TContext), new object[] { optionsBuilder.Options }))
                        .As<IDbContext>()
                        .InstancePerLifetimeScope();
            });
        }

        public static void AddDbContext<TContext>(this IEngine engine, DbContextOptions<TContext> options)
                where TContext : EfObjectContext
        {
            engine.Register(builder =>
            {
                builder.RegisterType<NpgSqlDataProvider>()
                        .As<IDataProvider>()
                        .SingleInstance();
                builder.Register(instance => Activator.CreateInstance(typeof(TContext), new object[] { options }))
                        .As<IDbContext>()
                        .InstancePerLifetimeScope();
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