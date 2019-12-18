using System;
using System.Runtime.Serialization;

namespace BinaryOrigin.SeedWork.Core.Exceptions
{
    /// <summary>
    /// Represent un authorized action during action execution
    /// </summary>
    [Serializable]
    public class UnAuthorizedException : GeneralException
    {
        public UnAuthorizedException()
        {
        }

        public UnAuthorizedException(string message) : base(message)
        {
        }

        public UnAuthorizedException(string messageFormat, params object[] args) : base(messageFormat, args)
        {
        }

        public UnAuthorizedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnAuthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}