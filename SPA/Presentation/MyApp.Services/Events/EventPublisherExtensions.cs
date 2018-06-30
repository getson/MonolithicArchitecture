﻿using MyApp.Core.SharedKernel.ApplicationEvents;
using MyApp.Core.SharedKernel.Entities;
using MyApp.Core.SharedKernel.Events;

namespace MyApp.Services.Events
{
    /// <summary>
    /// Event publisher extensions
    /// </summary>
    public static class EventPublisherExtensions
    {
        /// <summary>
        /// Entity inserted
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="entity">Entity</param>
        public static void EntityInserted<T>(this IEventPublisher eventPublisher, T entity) where T : BaseEntity, IDomainEvent
        {
            eventPublisher.Publish(new EntityInsertedEvent<T>(entity));
        }

        /// <summary>
        /// Entity updated
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="entity">Entity</param>
        public static void EntityUpdated<T>(this IEventPublisher eventPublisher, T entity) where T : BaseEntity, IDomainEvent
        {
            eventPublisher.Publish(new EntityUpdatedEvent<T>(entity));
        }

        /// <summary>
        /// Entity deleted
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="entity">Entity</param>
        public static void EntityDeleted<T>(this IEventPublisher eventPublisher, T entity) where T : BaseEntity,IDomainEvent
        {
            eventPublisher.Publish(new EntityDeletedEvent<T>(entity));
        }
    }
}