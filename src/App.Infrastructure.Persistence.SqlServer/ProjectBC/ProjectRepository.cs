using System;
using BinaryOrigin.SeedWork.Persistence.Ef;
using App.Core.Domain.ProjectBC;

namespace App.Persistence.ProjectBC
{
    public sealed class ProjectRepository :EfRepository<Project>, IProjectRepository
    {
        public ProjectRepository(IDbContext dbContext)
            :base(dbContext)
        {
        }
    }
}