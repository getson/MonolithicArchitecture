using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Core.Domain
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task CreateAsync(TEntity entity);
        Task CreateAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<Maybe<TEntity>> GetByIdAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}