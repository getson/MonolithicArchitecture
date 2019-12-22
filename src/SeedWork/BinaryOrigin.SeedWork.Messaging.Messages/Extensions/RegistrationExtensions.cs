using Autofac;
using BinaryOrigin.SeedWork.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            });
            engine.AddHandlers();
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
