using BinaryOrigin.SeedWork.Persistence.Ef;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Persistence.Context
{
    public class AppDbContext : EfObjectContext
    {
        public AppDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
        }
    }
}