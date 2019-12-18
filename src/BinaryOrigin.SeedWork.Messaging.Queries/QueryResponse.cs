using System;

namespace BinaryOrigin.SeedWork.Queries
{
    public sealed class QueryResponse<TQueryResult>
    {
        public QueryResponse()
        {
        }

        public QueryResponse(TQueryResult result)
        {
            Result = result;
        }

        public QueryResponse(Exception exception)
        {
            Error = new QueryResponseError(exception);
        }

        public bool Successful => Error == null;

        public bool HasResult => Result != null;

        public TQueryResult Result { get; private set; }

        public QueryResponseError Error { get; private set; }
    }
}