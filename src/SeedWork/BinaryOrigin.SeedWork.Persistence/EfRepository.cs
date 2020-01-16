using BinaryOrigin.SeedWork.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Persistence.Ef
{
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private DbSet<TEntity> _entities;
        protected readonly IDbContext _dbContext;

        public EfRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<TEntity> Table => Entities;

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

        /// <summary>
        /// Gets an entity set
        /// </summary>
        protected virtual DbSet<TEntity> Entities => _entities ?? (_entities = _dbContext.Set<TEntity>());

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }

        public async Task CreateAsync(TEntity entity)
        {
            Entities.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateAsync(IEnumerable<TEntity> entities)
        {
            Entities.AddRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async virtual Task<Maybe<TEntity>> GetByIdAsync(Guid id)
        {
            return Maybe<TEntity>.From(
                await Entities.SingleOrDefaultAsync(t => t.Id == id));
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public async virtual Task UpdateAsync(TEntity entity)
        {
            Entities.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            Entities.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async virtual Task DeleteAsync(TEntity entity)
        {
            Entities.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return (await Entities.SingleOrDefaultAsync(x => x.Id == id)) != null;
        }
    }
}