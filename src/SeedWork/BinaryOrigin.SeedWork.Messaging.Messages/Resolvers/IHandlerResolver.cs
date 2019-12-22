using System;
using System.Collections.Generic;

namespace BinaryOrigin.SeedWork.Messages
{
    public interface IHandlerResolver
    {
        object ResolveHandler(Type handlerType);

        object ResolveCommandHandler(object param, Type type);

        object ResolveQueryHandler(object query, Type type);

        IEnumerable<T> ResolveEventHandlers<T>();
    }
}