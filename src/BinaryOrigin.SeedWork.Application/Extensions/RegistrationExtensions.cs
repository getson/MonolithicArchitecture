using System.Linq;
using Autofac;
using BinaryOrigin.SeedWork.Application.Messaging;
using BinaryOrigin.SeedWork.Commands;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Core.Domain;
using BinaryOrigin.SeedWork.Persistence.Ef;
using BinaryOrigin.SeedWork.Queries;

namespace BinaryOrigin.SeedWork.Application.Extensions
{
    /// <summary>
    /// Register application dependencies
    /// </summary>

    public static class RegistrationExtensions
    {
        public static void AddInMemoryBus(this IEngine engine)
        {
            engine.Register(builder =>
            {
                builder.RegisterType<Bus>().As<IBus>().SingleInstance();
            });
        }
        public static void AddEfDbContext(this IEngine engine)
        {
            engine.Register(builder =>
            {
                builder.Register(instance => new EfObjectContext(engine.Configuration.DataSettings.DataConnectionString)).As<IDbContext>().InstancePerLifetimeScope();
            });
        }

        public static void AddCommandHandlers(this IEngine engine)
        {
            engine.Register(builder =>
            {
                var commandHandler = engine.FindClassesOfType(typeof(ICommandHandler<,>))
                    .FirstOrDefault(x => !x.AssemblyQualifiedName.Contains("SeedWork"));
                if (commandHandler != null)
                {
                    builder.RegisterAssemblyTypes(commandHandler.Assembly)
                        .AsClosedTypesOf(typeof(ICommandHandler<,>));
                }

                var queryHandler = engine.FindClassesOfType(typeof(IQueryHandler<,>))
                    .FirstOrDefault(x => !x.AssemblyQualifiedName.Contains("SeedWork"));
                if (queryHandler != null)
                {
                    builder.RegisterAssemblyTypes(queryHandler.Assembly)
                        .AsClosedTypesOf(typeof(IQueryHandler<,>));
                }
            });
        }
        public static void AddRepositories(this IEngine engine)
        {
            engine.Register(builder =>
            {
                builder.RegisterGeneric(typeof(EfRepository<,>)).As(typeof(IBaseRepository<,>));

                var repository = engine.FindClassesOfType(typeof(IRepository<>))
                    .FirstOrDefault(x => !x.AssemblyQualifiedName.Contains("SeedWork"));
                if (repository != null)
                {
                    builder.RegisterAssemblyTypes(repository.Assembly).AsClosedTypesOf(typeof(IRepository<>));
                }

            });
        }

        public static void AddAutoMapper(this IEngine engine)
        {
            engine.Register(builder =>
            {
                builder.RegisterType<AutoMapperTypeAdapterFactory>().As<ITypeAdapterFactory>().SingleInstance();
            });

        }

    }
}