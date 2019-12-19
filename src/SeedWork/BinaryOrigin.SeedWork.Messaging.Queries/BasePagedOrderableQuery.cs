namespace BinaryOrigin.SeedWork.Queries
{
    public abstract class BasePagedOrderableQuery<TQueryResult> : BasePagedQuery<TQueryResult>
    {
        protected BasePagedOrderableQuery(bool descending, string by, int pageIndex, int rowsInPage)
            : base(pageIndex, rowsInPage)
        {
            Descending = descending;
            By = by;
        }

        public bool Descending { get; private set; }

        public string By { get; private set; }
    }
}