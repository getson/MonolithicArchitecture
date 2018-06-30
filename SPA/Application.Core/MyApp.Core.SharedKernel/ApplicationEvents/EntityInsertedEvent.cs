using MyApp.Core.SharedKernel.Entities;
using MyApp.Core.SharedKernel.Events;

namespace MyApp.Core.SharedKernel.ApplicationEvents
{
    /// <summary>
    /// A container for entities that have been inserted.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityInsertedEvent<T> where T : BaseEntity, IDomainEvent
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="entity">Entity</param>
        public EntityInsertedEvent(T entity)
        {
            Entity = entity;
        }

        /// <summary>
        /// Entity
        /// </summary>
        public T Entity { get; }
    }
}
