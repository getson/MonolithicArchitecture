using App.Core.Domain.ProjectBC;
using BinaryOrigin.SeedWork.Persistence.Ef;

namespace App.Infrastructure.Persistence.SqlServer.ProjectBC
{
    public sealed class ProjectRepository : EfRepository<Project>, IProjectRepository
    {
        public ProjectRepository(IDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}