using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MyApp.Core.Exceptions
{
    /// <summary>
	/// The custom exception for validation errors
	/// </summary>
    [Serializable]
	public class ApplicationValidationErrorsException : MyAppException
    {
        /// <summary>
        /// Get or set the validation errors messages
        /// </summary>
        public IEnumerable<string> ValidationErrors { get; }

        /// <summary>
        /// Create new instance of Application validation errors exception
        /// </summary>
        /// <param name="validationErrors">The collection of validation errors</param>
        public ApplicationValidationErrorsException(IEnumerable<string> validationErrors)
            : base("validation_Exception")
        {
            ValidationErrors = validationErrors;
        }

        public ApplicationValidationErrorsException()
        {
        }

        public ApplicationValidationErrorsException(string message) : base(message)
        {
        }

        public ApplicationValidationErrorsException(string messageFormat, params object[] args) : base(messageFormat, args)
        {
        }

        protected ApplicationValidationErrorsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ApplicationValidationErrorsException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
