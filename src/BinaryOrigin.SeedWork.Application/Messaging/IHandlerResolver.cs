using System;

namespace BinaryOrigin.SeedWork.Application.Messaging
{
    public interface IHandlerResolver
    {
        Type Get(Type type);
    }
}