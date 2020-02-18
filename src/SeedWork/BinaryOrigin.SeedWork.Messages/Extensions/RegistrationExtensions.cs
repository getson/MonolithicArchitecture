using Autofac;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Messages.Validation;
using System;
using System.Linq;

namespace BinaryOrigin.SeedWork.Messages
{
    public static class RegistrationExtensions
    {
        public static void AddInMemoryBus(this IEngine engine)
        {
            engine.Register(builder =>
            {
                builder.Register(x => new HandlerResolver(engine))
                       .As<IHandlerResolver>()
                       .InstancePerLifetimeScope();

                builder.RegisterType<InMemoryBus>()
                       .As<IBus>()
                       .SingleInstance();
                builder.RegisterType<DefaultCommandValidationProvider>()
                       .As<ICommandValidationProvider>()
                       .SingleInstance();
                builder.RegisterType<NullLocalizerService>()
                       .As<ILocalizerService>()
                       .InstancePerLifetimeScope();
            });
        }
        public static void AddDefaultDecorators(this IEngine engine)
        {
            engine.Register(builder =>
            {
                var commandDecorators = engine.FindClassesOfType(typeof(ICommandHandlerDecorator<,>))
                                              .OrderBy(x => GetDecoratorOrder(x))
                                              .ToList();
                if (commandDecorators.Any())
                {
                    commandDecorators.ForEach(decorator =>
                    {
                        builder.RegisterGenericDecorator(decorator, typeof(ICommandHandler<,>));
                    });
                }

                var queryDecorators = engine.FindClassesOfType(typeof(IQueryHandlerDecorator<,>))
                                            .OrderBy(x => GetDecoratorOrder(x))
                                            .ToList();

                if (queryDecorators.Any())
                {
                    queryDecorators.ForEach(decorator =>
                    {
                        builder.RegisterGenericDecorator(decorator, typeof(IQueryHandler<,>));
                    });
                }
            });
        }

        private static int GetDecoratorOrder(Type x)
        {
            var decoratorOrder = x.GetCustomAttributes(typeof(DecoratorOrderAttribute), false)
                                   .FirstOrDefault();
            if (decoratorOrder == null)
                return 1000; // No order is specified
            return ((DecoratorOrderAttribute)decoratorOrder).Order;
        }
        public static void AddHandlers(this IEngine engine)
        {
            engine.Register(builder =>
            {
                var commandHandlersAsms = engine.FindClassesOfType(typeof(ICommandHandler<,>))
                    .Where(x => !x.AssemblyQualifiedName.Contains("SeedWork"))
                    .Select(x => x.Assembly)
                    .Distinct()
                    .ToList();
                if (commandHandlersAsms.Any())
                {
                    commandHandlersAsms.ForEach(asm =>
                    {
                        builder.RegisterAssemblyTypes(asm)
                                         .AsClosedTypesOf(typeof(ICommandHandler<,>))
                                         .InstancePerLifetimeScope();
                    });
                }

                var queryHandlersAsms = engine.FindClassesOfType(typeof(IQueryHandler<,>))
                    .Where(x => !x.AssemblyQualifiedName.Contains("SeedWork"))
                    .Select(x => x.Assembly)
                    .Distinct()
                    .ToList();
                if (queryHandlersAsms.Any())
                {
                    queryHandlersAsms.ForEach(asm =>
                    {
                        builder.RegisterAssemblyTypes(asm)
                            .AsClosedTypesOf(typeof(IQueryHandler<,>))
                            .InstancePerLifetimeScope();
                    });
                }

                var eventHandlersAsms = engine.FindClassesOfType(typeof(IEventHandler<>))
                      .Where(x => !x.AssemblyQualifiedName.Contains("SeedWork"))
                      .Select(x => x.Assembly)
                      .Distinct()
                      .ToList();
                if (eventHandlersAsms.Any())
                {
                    eventHandlersAsms.ForEach(asm =>
                    {
                        builder.RegisterAssemblyTypes(asm)
                            .AsClosedTypesOf(typeof(IEventHandler<>))
                            .InstancePerLifetimeScope();
                    });
                }
                var orderedEventHandlersAsms = engine.FindClassesOfType(typeof(ISequenceEventHandler<>))
                     .Where(x => !x.AssemblyQualifiedName.Contains("SeedWork"))
                     .Select(x => x.Assembly)
                     .Distinct()
                     .ToList();
                if (orderedEventHandlersAsms.Any())
                {
                    orderedEventHandlersAsms.ForEach(asm =>
                    {
                        builder.RegisterAssemblyTypes(asm)
                            .AsClosedTypesOf(typeof(ISequenceEventHandler<>))
                            .InstancePerLifetimeScope();
                    });
                }
            });
        }
    }
}