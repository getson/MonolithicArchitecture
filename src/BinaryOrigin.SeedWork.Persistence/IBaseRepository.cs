using System;
using System.Linq;
using BinaryOrigin.SeedWork.Core.Domain;

namespace BinaryOrigin.SeedWork.Persistence.Ef
{
    /// <summary>
    /// Base repository interface
    /// </summary>
    /// <typeparam name="TEntity">Domain entity type</typeparam>
    /// <typeparam name="TData">Data object type</typeparam>
    public interface IBaseRepository<in TEntity, out TData>
        where TEntity : BaseEntity
        where TData : IData, new()
    {
        /// <summary>
        /// Add the entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Add(TEntity entity);

        /// <summary>
        /// Update the entity
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// Delete the entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);

        IQueryable<TData> GetById(Guid id);

        /// <summary>
        /// Map Entity object to Data object.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TData Map(TEntity entity);
    }
}