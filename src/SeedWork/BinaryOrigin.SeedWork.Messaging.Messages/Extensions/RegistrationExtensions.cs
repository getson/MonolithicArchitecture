using Autofac;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Messages.Decorators;
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
                builder.RegisterType<NullLocalizerService>()
                       .As<ILocalizerService>()
                       .SingleInstance();
            });
            engine.AddHandlers();
            engine.AddDecorators();
        }
        private static void AddDecorators(this IEngine engine)
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

        private static void AddHandlers(this IEngine engine)
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

                var messageHandlersAsms = engine.FindClassesOfType(typeof(IEventHandler<>))
                      .Where(x => !x.AssemblyQualifiedName.Contains("SeedWork"))
                      .Select(x => x.Assembly)
                      .Distinct()
                      .ToList();
                if (messageHandlersAsms.Any())
                {
                    messageHandlersAsms.ForEach(asm =>
                    {
                        builder.RegisterAssemblyTypes(asm)
                            .AsClosedTypesOf(typeof(IEventHandler<>))
                            .InstancePerLifetimeScope();
                    });
                }
            });
        }
    }
}