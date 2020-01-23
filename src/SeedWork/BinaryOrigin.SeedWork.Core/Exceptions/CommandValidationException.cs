using BinaryOrigin.SeedWork.Core.Domain;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BinaryOrigin.SeedWork.Core
{
    /// <summary>
    /// Exception wich can be used  to throw error when command is not valid
    /// </summary>
    [Serializable]
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

        protected CommandValidationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}