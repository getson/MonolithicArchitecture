namespace BinaryOrigin.SeedWork.Queries
{
    public abstract class BasePagedQuery<TQueryResult> : IQuery<PagedCollection<TQueryResult>>
    {
        protected BasePagedQuery(int pageIndex, int rowsInPage)
        {
            PageIndex = pageIndex < 0 ? 0 : pageIndex;
            RowsInPage = rowsInPage;
        }

        public int PageIndex { get; private set; }

        public int RowsInPage { get; private set; }
    }
}