using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BinaryOrigin.SeedWork.Core.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// The custom exception for validation errors
    /// </summary>
    [Serializable]
    public class ValidationException : GeneralException
    {
        /// <inheritdoc />
        /// <summary>
        /// Create new instance of Application validation errors exception
        /// </summary>
        /// <param name="validationErrors">The collection of validation errors</param>
        public ValidationException(IEnumerable<string> validationErrors)
            : base("validation_Exception")
        {
            ValidationErrors = validationErrors;
            Data["errors"] = validationErrors;
        }

        public ValidationException()
        {
        }

        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string messageFormat, params object[] args) : base(messageFormat, args)
        {
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Get or set the validation errors messages
        /// </summary>
        public IEnumerable<string> ValidationErrors { get; }
    }
}