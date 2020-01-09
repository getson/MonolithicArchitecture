using System;
using System.Collections.Generic;
using System.Text;

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
