using BinaryOrigin.SeedWork.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryOrigin.SeedWork.Messages
{
    internal sealed class CommandHandlerResolver : IHandlerResolver
    {
        private readonly Dictionary<Type, Type> _handlers;

        internal CommandHandlerResolver()
        {
            var handlers = EngineContext.Current.FindClassesOfType(typeof(ICommandHandler<,>));

            _handlers = handlers.ToDictionary(
                        type => type.GetInterfaces()
                            .First(i => i.IsGenericType)
                            .GetGenericArguments()[0]
                    );
        }

        public Type Get(Type type)
        {
            return _handlers[type];
        }
    }
}