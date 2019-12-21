using System;
using System.Collections.Generic;

namespace BinaryOrigin.SeedWork.Messages
{
    public interface IEventHandlerResolver
    {
        IEnumerable<Type> Get(Type type);
    }
}