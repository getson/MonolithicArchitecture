using BinaryOrigin.SeedWork.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryOrigin.SeedWork.Messages
{
    public class HandlerResolver : IHandlerResolver
    {
        private readonly IEngine _engine;

        public HandlerResolver(IEngine engine)
        {
            _engine = engine;
        }

        public object ResolveHandler(Type handlerType)
        {
            return _engine.Resolve(handlerType);
        }

        public object ResolveCommandHandler(object param, Type type)
        {
            var commandType = param.GetType();
            var commandInterface = commandType.GetInterfaces().Last();
            var resultType = commandInterface.GetGenericArguments()[0];
            var handlerType = type.MakeGenericType(commandType, resultType);

            return ResolveHandler(handlerType);
        }

        public object ResolveQueryHandler(object query, Type type)
        {
            var queryType = query.GetType();
            var queryInterface = queryType.GetInterfaces()[0];
            var resultType = queryInterface.GetGenericArguments()[0];
            var handlerType = type.MakeGenericType(queryType, resultType);

            return ResolveHandler(handlerType);
        }

        public IEnumerable<T> ResolveEventHandlers<T>()
        {
            return _engine.ResolveAll<T>();
        }
    }
}