using System;

namespace BinaryOrigin.SeedWork.WebApi.Messaging
{
    public interface IHandlerResolver
    {
        Type Get(Type type);
    }
}