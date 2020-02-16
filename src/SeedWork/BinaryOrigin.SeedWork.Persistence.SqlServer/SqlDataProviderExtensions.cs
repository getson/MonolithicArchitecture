using Autofac;
using BinaryOrigin.SeedWork.Persistence.Ef;
using BinaryOrigin.SeedWork.Persistence.SqlServer;
using Microsoft.EntityFrameworkCore;
using System;

namespace BinaryOrigin.SeedWork.Core
{
    public static class SqlDataProviderExtensions
    {
        public static void AddDbContext<TContext>(this IEngine engine, TContext context)
            where TContext : IDbContext
        {
            engine.Register(builder =>
            {
                builder.RegisterType<SqlServerDataProvider>()
                        .As<IDataProvider>()
                        .SingleInstance();
                builder.Register(instance => context)
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