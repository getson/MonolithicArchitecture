using System.Linq;
using BinaryOrigin.SeedWork.Persistence.Ef;
using BinaryOrigin.SeedWork.Queries;
using App.Application.Queries.ProjectBC;
using App.Persistence.ProjectBC;
using BinaryOrigin.SeedWork.Core;
using System.Collections.Generic;
using BinaryOrigin.SeedWork.Core.Domain;
using System.Threading.Tasks;
using App.Core.Domain.ProjectBC;
using Microsoft.EntityFrameworkCore;

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

            var result =await baseQuery.Skip(queryModel.Offset)
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