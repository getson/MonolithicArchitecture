using System;

namespace BinaryOrigin.SeedWork.Queries
{
    public sealed class QueryResponseError
    {
        internal QueryResponseError(Exception exception)
        {
            ExceptionType = exception.GetType().Name;
            ExceptionMessage = exception.Message;
            ExceptionObject = exception;
        }

        public string ExceptionType { get; private set; }

        public string ExceptionMessage { get; private set; }

        public Exception ExceptionObject { get; private set; }
    }
}