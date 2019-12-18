using System;
using System.Collections.Generic;

namespace BinaryOrigin.SeedWork.WebApi.Messaging
{
    public interface IMessageHandlerResolver
    {
       IEnumerable<Type> Get(Type type);
    }
}