using Autofac;
using AutoMapper;

using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Core.Domain;
using BinaryOrigin.SeedWork.Core.Extensions;
using BinaryOrigin.SeedWork.Messages;
using BinaryOrigin.SeedWork.Messages.Validation;
using BinaryOrigin.SeedWork.Persistence.Ef;
using BinaryOrigin.SeedWork.Persistence.SqlServer;

using BinaryOrigin.SeedWork.WebApi.Mapping;
using BinaryOrigin.SeedWork.WebApi.Validations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BinaryOrigin.SeedWork.WebApi.Extensions
{
    /// <summary>
    /// Register application dependencies
    /// </summary>

    public static class RegistrationExtensions
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

        public static void AddInMemoryDbContext(this IEngine engine)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EfObjectContext>();
            optionsBuilder.UseInMemoryDatabase("InMemoryDb");

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

        public static void AddInMemoryBus(this IEngine engine)
        {
            engine.Register(builder =>
            {
                builder.Register(x => new Bus(new FluentValidationProvider()))
                       .As<IBus>()
                       .SingleInstance();
            });
        }
        public static void AddFluentValidation(this IEngine engine)
        {
            engine.Register(builder =>
            {
                var asmsWithCommandValidator = engine.FindClassesOfType(typeof(IValidator<>))
                                                    .Where(x => !x.AssemblyQualifiedName.Contains("SeedWork"))
                                                    .Select(x => x.Assembly)
                                                    .DistinctBy(x => x.FullName)
                                                    .ToList();

                foreach (var asm in asmsWithCommandValidator)
                {
                    builder.RegisterAssemblyTypes(asm)
                            .AsClosedTypesOf(typeof(IValidator<>))
                            .InstancePerLifetimeScope();
                }
                builder.RegisterType<FluentValidationProvider>()
                .As<IValidationProvider>()
                .InstancePerLifetimeScope();
            });
        }
        public static void AddCommandHandlers(this IEngine engine)
        {
            engine.Register(builder =>
            {

                var commandHandlers = engine.FindClassesOfType(typeof(ICommandHandler<,>))
                    .Where(x => !x.AssemblyQualifiedName.Contains("SeedWork"))
                    .ToList();
                if (commandHandlers.Any())
                {
                    commandHandlers.ForEach(commandHandler =>
                    {
                        builder.RegisterAssemblyTypes(commandHandler.Assembly)
                                         .AsClosedTypesOf(typeof(ICommandHandler<,>))
                                         .InstancePerLifetimeScope();
                    });
                }

                var queryHandlers = engine.FindClassesOfType(typeof(IQueryHandler<,>))
                    .Where(x => !x.AssemblyQualifiedName.Contains("SeedWork"))
                    .ToList();
                if (queryHandlers.Any())
                {
                    queryHandlers.ForEach(queryHandler =>
                    {
                        builder.RegisterAssemblyTypes(queryHandler.Assembly)
                            .AsClosedTypesOf(typeof(IQueryHandler<,>))
                            .InstancePerLifetimeScope();
                    });
                }

                var messageHandlers = engine.FindClassesOfType(typeof(IEventHandler<>))
                      .Where(x => !x.AssemblyQualifiedName.Contains("SeedWork"))
                      .ToList();
                if (messageHandlers.Any())
                {
                    messageHandlers.ForEach(messageHandler =>
                    {
                        builder.RegisterType(messageHandler)
                        .AsSelf()
                        .InstancePerLifetimeScope();
                    });
                }
            });
        }

        public static void AddRepositories(this IEngine engine)
        {
            engine.Register(builder =>
            {
                var assembliesWithRepositories = engine.FindClassesOfType(typeof(IRepository<>))
                                                        .Where(x => !x.AssemblyQualifiedName.Contains("SeedWork"))
                                                        .Select(x => x.Assembly)
                                                        .DistinctBy(x => x.FullName)
                                                        .ToList();

                foreach (var asm in assembliesWithRepositories)
                {
                    builder.RegisterAssemblyTypes(asm)
                            .AsClosedTypesOf(typeof(IRepository<>))
                            .InstancePerLifetimeScope();
                }
            });
        }

        public static void AddAutoMapper(this IEngine engine)
        {
            //find mapper configurations provided by other assemblies
            var mapperConfigurations = engine.FindClassesOfType<IMapperProfile>();

            //create and sort instances of mapper configurations
            var instances = mapperConfigurations
                .Select(mapperConfiguration => (IMapperProfile)Activator.CreateInstance(mapperConfiguration))
                .OrderBy(mapperConfiguration => mapperConfiguration.Order);

            //create AutoMapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                foreach (var instance in instances)
                {
                    cfg.AddProfile(instance.GetType());
                }
            });

            engine.Register(builder =>
            {
                builder.RegisterInstance(config.CreateMapper()).As<IMapper>().SingleInstance();
                builder.RegisterType<AutoMapperTypeAdapter>().As<ITypeAdapter>().SingleInstance();
            });
        }
    }
}