using BinaryOrigin.SeedWork.Commands;
using BinaryOrigin.SeedWork.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryOrigin.SeedWork.WebApi.Messaging
{
    internal sealed class MessageHandlerResolver : IMessageHandlerResolver
    {
        private readonly Dictionary<Type, IEnumerable<Type>> _handlers;

        internal MessageHandlerResolver()
        {
            _handlers = new Dictionary<Type, IEnumerable<Type>>();

            var handlers = EngineContext.Current.FindClassesOfType(typeof(IMessageHandler<>));
            var messages = EngineContext.Current.FindClassesOfType<IMessage>();

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