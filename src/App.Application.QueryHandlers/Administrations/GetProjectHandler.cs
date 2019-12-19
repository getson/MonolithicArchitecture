﻿using System.Linq;
using BinaryOrigin.SeedWork.Persistence.Ef;
using BinaryOrigin.SeedWork.Queries;
using App.Application.Queries.Administrations;
using App.Persistence.Administrations;
using App.Core.Domain.Administrations;
using System.Threading.Tasks;
using BinaryOrigin.SeedWork.Core.Domain;
using Microsoft.EntityFrameworkCore;
using BinaryOrigin.SeedWork.Core;

namespace App.Application.QueryHandlers.Administrations
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