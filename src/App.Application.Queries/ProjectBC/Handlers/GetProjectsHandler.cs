using App.Core.Domain.ProjectBC;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Core.Domain;
using BinaryOrigin.SeedWork.Messages;
using BinaryOrigin.SeedWork.Persistence.Ef;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace App.Application.Queries.ProjectBC.Handlers
{
    public class GetProjectsHandler : IQueryHandler<GetProjects, PaginatedItemsResult<GetProjectResult>>
    {
        private readonly IDbContext _dbContext;
        private readonly IPaginationService _paginationService;

        public GetProjectsHandler(IDbContext dbContext,
                                  IPaginationService paginationService)
        {
            _dbContext = dbContext;
            _paginationService = paginationService;
        }

        public async Task<Result<PaginatedItemsResult<GetProjectResult>>> HandleAsync(GetProjects queryModel)
        {
            var baseQuery = _dbContext.Set<Project>().AsNoTracking();

            var result = await _paginationService.PaginateAsync<GetProjectResult>
                                (
                                    baseQuery,
                                    queryModel.PageIndex,
                                    queryModel.PageSize
                                );

            return Result.Ok(result);
        }
    }
}