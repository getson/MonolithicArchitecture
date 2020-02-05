using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Persistence.Ef
{
    public class PaginationService : IPaginationService
    {
        private readonly ITypeAdapter _typeAdapter;
        private readonly PaginationOptions _options;

        public PaginationService(ITypeAdapter typeAdapter, PaginationOptions options)
        {
            _typeAdapter = typeAdapter;
            _options = options;
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

            pageSize = pageSize == 0 ? _options.DefaultPageSize : Math.Min(pageSize, _options.MaxPageSizeAllowed);

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

    public class PaginationOptions
    {
        /// <summary>
        /// Default is 20
        /// </summary>
        public int MaxPageSizeAllowed { get; set; } = 20;

        public int DefaultPageSize { get; set; } = 20;
    }
}