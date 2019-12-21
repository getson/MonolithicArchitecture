﻿using App.Application.Queries.ProjectBC;
using App.Core.Domain.ProjectBC;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Core.Domain;
using BinaryOrigin.SeedWork.Messages;
using BinaryOrigin.SeedWork.Persistence.Ef;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Application.QueryHandlers.ProjectBC
{
    public class GetProjectsHandler : IQueryHandler<GetProjects, GetProjectsResult>
    {
        private readonly IDbContext _dbContext;
        private readonly ITypeAdapter _typeAdapter;

        public GetProjectsHandler(IDbContext dbContext, ITypeAdapter typeAdapter)
        {
            _dbContext = dbContext;
            _typeAdapter = typeAdapter;
        }

        public async Task<Result<GetProjectsResult>> HandleAsync(GetProjects queryModel)
        {
            var baseQuery = _dbContext.Set<Project>().AsNoTracking();

            var result = await baseQuery.Skip(queryModel.Offset)
                                  .Take(queryModel.Limit)
                                  .ToListAsync();

            var count = await baseQuery.CountAsync();

            return Result.Ok(new GetProjectsResult
            {
                Total = count,
                Items = _typeAdapter.Adapt<IEnumerable<GetProjectResult>>(result)
            });
        }
    }
}