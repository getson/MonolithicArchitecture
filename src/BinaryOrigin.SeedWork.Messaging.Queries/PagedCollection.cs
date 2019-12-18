using System.Collections;
using System.Collections.Generic;

namespace BinaryOrigin.SeedWork.Queries
{
    public sealed class PagedCollection<TResult> : IEnumerable<TResult>
    {
        private readonly IEnumerable<TResult> _items;

        public PagedCollection(TResult[] source, int totalRows, int pageIndex, int rowsInPage, bool descending, string by)
        {
            _items = source;

            TotalRows = totalRows;
            TotalPages = GetPageCount(rowsInPage, TotalRows);
            PageIndex = pageIndex;
            RowsInPage = rowsInPage;
            Descending = descending;
            By = by;
        }

        public int TotalRows { get; private set; }

        public int TotalPages { get; private set; }

        public int PageIndex { get; private set; }

        public int RowsInPage { get; private set; }

        public bool Descending { get; private set; }

        public string By { get; private set; }

        public IEnumerator<TResult> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_items).GetEnumerator();
        }

        private static int GetPageCount(int rowsInPage, int totalRows)
        {
            if (rowsInPage == 0)
            {
                return 0;
            }

            var remainder = totalRows % rowsInPage;
            return totalRows / rowsInPage + (remainder == 0 ? 0 : 1);
        }
    }
}