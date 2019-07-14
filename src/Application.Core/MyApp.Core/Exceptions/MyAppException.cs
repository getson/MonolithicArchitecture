﻿using System;
using System.Runtime.Serialization;

namespace MyApp.Core.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Represents errors that occur during application execution
    /// </summary>
    [Serializable]
    public class MyAppException : Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the Exception class.
        /// </summary>
        public MyAppException()
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MyAppException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message.
        /// </summary>
        /// <param name="messageFormat">The exception message format.</param>
        /// <param name="args">The exception message arguments.</param>
        public MyAppException(string messageFormat, params object[] args)
            : base(string.Format(messageFormat, args))
        {
        }

        /// <summary>
        /// Initializes a new instance of the Exception class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected MyAppException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public MyAppException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
