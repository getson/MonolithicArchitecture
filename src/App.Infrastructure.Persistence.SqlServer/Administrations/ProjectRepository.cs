using System;
using BinaryOrigin.SeedWork.Persistence.Ef;
using App.Core.Domain.Administrations;

namespace App.Persistence.Administrations
{
    public sealed class ProjectRepository :EfRepository<Project>, IProjectRepository
    {
        public ProjectRepository(IDbContext dbContext)
            :base(dbContext)
        {
        }
    }
}