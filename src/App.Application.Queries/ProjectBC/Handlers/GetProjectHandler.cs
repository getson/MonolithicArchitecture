﻿using App.Core;
using App.Core.Domain.ProjectBC;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Core.Domain;
using BinaryOrigin.SeedWork.Messages;
using BinaryOrigin.SeedWork.Persistence.Ef;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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

            if (result == null)
            {
                return Result.Fail<GetProjectResult>(ErrorMessages.ProjectNotFound);
            }

            return Result.Ok(_typeAdapter.Adapt<GetProjectResult>(result));
        }
    }
}