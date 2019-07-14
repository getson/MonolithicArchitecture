using System;
using System.Runtime.Serialization;

namespace MyApp.Core.Exceptions
{
    [Serializable]
    public class InvalidServiceRequestException : MyAppException
    {
        public InvalidServiceRequestException()
        {
        }

        public InvalidServiceRequestException(string message) : base(message)
        {
        }

        public InvalidServiceRequestException(string messageFormat, params object[] args) : base(messageFormat, args)
        {
        }

        public InvalidServiceRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidServiceRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
