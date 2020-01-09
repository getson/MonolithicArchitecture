using System;

namespace BinaryOrigin.SeedWork.Messages
{
    public class DecoratorOrderAttribute : Attribute
    {
        public DecoratorOrderAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; }
    }
}
