using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryOrigin.SeedWork.Core.Domain
{
    public class ValidationError
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
