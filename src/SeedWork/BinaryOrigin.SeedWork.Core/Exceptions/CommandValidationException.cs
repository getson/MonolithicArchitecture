using BinaryOrigin.SeedWork.Core.Domain;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BinaryOrigin.SeedWork.Core.Exceptions
{
    public class CommandValidationException : Exception
    {
        private readonly IEnumerable<ValidationError> _validationErrors;

        public CommandValidationException(IEnumerable<ValidationError> validationErrors)
        {
            _validationErrors = validationErrors;
            foreach (var validationError in _validationErrors)
            {
                Data[validationError.PropertyName] = validationError.ErrorMessage;
            }
        }

        protected CommandValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}