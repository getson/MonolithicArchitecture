using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BinaryOrigin.SeedWork.Persistence.Ef
{
    public class PaginationService : IPaginationService
    {
        private static readonly int _maxPageSize = 20;
        private readonly ITypeAdapter _typeAdapter;

        public PaginationService(ITypeAdapter typeAdapter)
        {
            _typeAdapter = typeAdapter;
        }

        public async Task<PaginatedItemsResult<BaseEntity>> PaginateAsync
            (
                IQueryable<BaseEntity> source,
                int pageIndex,
                int pageSize
            )
        {
            if (pageIndex < 0)
            {
                pageIndex = 0;
            }

            pageSize = pageSize == 0 ? _maxPageSize : Math.Min(pageSize, _maxPageSize);

            var count = await source.LongCountAsync();

            var data = await source.Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedItemsResult<BaseEntity>
                (
                    data,
                    (int)Math.Floor((decimal)count / pageSize),
                    count
                );
        }

        public async Task<PaginatedItemsResult<TItemResult>> PaginateAsync<TItemResult>
            (
                IQueryable<BaseEntity> source,
                int pageIndex,
                int pageSize
            ) where TItemResult : class
        {
            var paginatedResult = await PaginateAsync(source, pageIndex, pageSize);

            return new PaginatedItemsResult<TItemResult>
                (
                    _typeAdapter.Adapt<IEnumerable<TItemResult>>(paginatedResult.Data),
                    paginatedResult.TotalPages,
                    paginatedResult.Count
                );
        }
    }
}
