using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MyApp.Core.SharedKernel.Entities;
using MyApp.Core.SharedKernel.Specification;

namespace MyApp.Core.SharedKernel
{


    /// <summary>
    /// Provide REPOSITORIES only for AGGREGATE roots
    /// that actually need direct access. Keep the client focused on the model, delegating all
    /// object storage and access to the REPOSITORIES.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        #region Methods

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        TEntity GetById(object id);

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Insert(TEntity entity);

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Insert(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(TEntity entity);

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Update(IEnumerable<TEntity> entities);
       
        /// <summary>
        /// Sets modified entity into the repository. 
        /// </summary>
        /// <param name="persisted">The persisted item</param>
        /// <param name="current">The current item</param>
        void Merge(TEntity persisted, TEntity current);
        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Delete(IEnumerable<TEntity> entities);

        IEnumerable<TEntity> AllMatching(ISpecification<TEntity> specification);
        IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter);
        IEnumerable<TEntity> GetFilteredNoTracking(Expression<Func<TEntity, bool>> filter);
        #endregion

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<TEntity> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<TEntity> TableNoTracking { get; }

        #endregion
    }
}