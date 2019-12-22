using App.Core.Domain.ProjectBC;
using BinaryOrigin.SeedWork.Core.Domain;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Messages;
using BinaryOrigin.SeedWork.Persistence.Ef;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace App.Application.Queries.ProjectBC.Handlers
{
    public class GetProjectHandler : IQueryHandler<GetProject, GetProjectResult>
    {
        private readonly IDbContext _dbContext;
        private readonly ITypeAdapter _typeAdapter;

        public GetProjectHandler(IDbContext dbContext, ITypeAdapter typeAdapter)
        {
            _dbContext = dbContext;
            _typeAdapter = typeAdapter;
        }

        public async Task<Result<GetProjectResult>> HandleAsync(GetProject queryModel)
        {
            var result = await _dbContext.Set<Project>()
                .SingleOrDefaultAsync(p => p.Id == queryModel.Id);

            return Result.Ok(_typeAdapter.Adapt<GetProjectResult>(result));
        }
    }

}
