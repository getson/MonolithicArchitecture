using BinaryOrigin.SeedWork.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryOrigin.SeedWork.Messages
{
    internal sealed class EventHandlerResolver : IEventHandlerResolver
    {
        private readonly Dictionary<Type, IEnumerable<Type>> _handlers;

        internal EventHandlerResolver()
        {
            _handlers = new Dictionary<Type, IEnumerable<Type>>();

            var handlers = EngineContext.Current.FindClassesOfType(typeof(IEventHandler<>));
            var messages = EngineContext.Current.FindClassesOfType<IEvent>();

            foreach (var message in messages)
            {
                var messageHandlers = handlers.Where(handler =>
                                handler.GetInterfaces()
                                   .First(i => i.IsGenericType)
                                   .GetGenericArguments()[0] == message);
                _handlers[message] = messageHandlers;
            }
        }

        public IEnumerable<Type> Get(Type type)
        {
            return _handlers[type];
        }
    }
}