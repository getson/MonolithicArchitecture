using System;
using System.Runtime.Serialization;

namespace BinaryOrigin.SeedWork.Core.Exceptions
{
    [Serializable]
    public class GeneralConflictException : GeneralException
    {
        public GeneralConflictException()
        {
        }

        public GeneralConflictException(string message) : base(message)
        {
        }

        public GeneralConflictException(string messageFormat, params object[] args) : base(messageFormat, args)
        {
        }

        public GeneralConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GeneralConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}