using BinaryOrigin.SeedWork.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Core
{
    public interface IPaginationService
    {
        /// <summary>
        /// Get a IQueryable source and execute apply pagination against it. 
        /// It queries DB two times, first time to get the totatl count of elements
        /// and the second time to get items based on pageSize and PageIndex.
        /// </summary>
        /// <typeparam name="TItemResult"></typeparam>
        /// <param name="source">IQueryable<<see cref="BaseEntity"/>> where paginated will be performed</param>
        /// <param name="pageIndex">Current page index (0 based)</param>
        /// <param name="pageSize">Number of rows per page</param>
        /// <returns></returns>
        Task<PaginatedItemsResult<BaseEntity>> PaginateAsync(IQueryable<BaseEntity> source,
                                                                        int pageIndex,
                                                                        int pageSize);
        /// <summary>
        /// Get a IQueryable source and execute apply pagination against it. 
        /// It queries DB two times, first time to get the totatl count of elements
        /// and the second time to get items based on pageSize and PageIndex.
        /// After queries are executed a mapping oepration will be executed
        /// </summary>
        /// <typeparam name="TItemResult"></typeparam>
        /// <param name="source">IQueryable<<see cref="BaseEntity"/>> where paginated will be performed</param>
        /// <param name="pageIndex">Current page index (0 based)</param>
        /// <param name="pageSize">Number of rows per page</param>
        /// <returns></returns>
        Task<PaginatedItemsResult<TItemResult>> PaginateAsync<TItemResult>(IQueryable<BaseEntity> source,
                                                                          int pageIndex,
                                                                          int pageSize) where TItemResult : class;
    }
    public class PaginatedItemsResult<TItemResult> where TItemResult : class
    {
        public PaginatedItemsResult(IEnumerable<TItemResult> data, int totalPages, long count)
        {
            TotalPages = totalPages;
            Count = count;
            Data = data;
        }

        public int TotalPages { get; private set; }
        public long Count { get; private set; }
        public IEnumerable<TItemResult> Data { get; private set; }
    }
}
