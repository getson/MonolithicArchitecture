using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryOrigin.SeedWork.WebApi.Messaging
{
    internal sealed class QueryHandlerResolver : IHandlerResolver
    {
        private readonly Dictionary<Type, Type> _handlers;

        internal QueryHandlerResolver()
        {
            var handlers = EngineContext.Current.FindClassesOfType(typeof(IQueryHandler<,>));

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