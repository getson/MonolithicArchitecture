using System;

namespace BinaryOrigin.SeedWork.Messages
{
    public interface IHandlerResolver
    {
        Type Get(Type type);
    }
}