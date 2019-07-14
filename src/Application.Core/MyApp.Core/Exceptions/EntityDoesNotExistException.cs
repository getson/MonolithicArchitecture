using System;
using System.Runtime.Serialization;

namespace MyApp.Core.Exceptions
{
    [Serializable]
    public class EntityDoesNotExistException : MyAppException
    {
        public EntityDoesNotExistException()
        {
        }

        public EntityDoesNotExistException(string message) : base(message)
        {
        }

        public EntityDoesNotExistException(string messageFormat, params object[] args) : base(messageFormat, args)
        {
        }

        public EntityDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EntityDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
