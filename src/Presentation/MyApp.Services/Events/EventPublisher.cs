﻿using System;
using System.Linq;
using MyApp.Core.Infrastructure;
using MyApp.Services.Logging;

namespace MyApp.Services.Events
{
    /// <inheritdoc />
    /// <summary>
    /// Evnt publisher
    /// </summary>
    public class EventPublisher : IEventPublisher
    {
        private readonly ISubscriptionService _subscriptionService;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="subscriptionService"></param>
        public EventPublisher(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        /// <summary>
        /// Publish to cunsumer
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="x">Event consumer</param>
        /// <param name="eventMessage">Event message</param>
        protected virtual void PublishToConsumer<T>(IConsumer<T> x, T eventMessage)
        {
            try
            {
                x.HandleEvent(eventMessage);
            }
            catch (Exception exc)
            {
                //log error
                var logger = EngineContext.Current.Resolve<ILogger>();
                //we put in to nested try-catch to prevent possible cyclic (if some error occurs)
                try
                {
                    logger.Error(exc.Message, exc);
                }
                catch (Exception)
                {
                    //do nothing
                }
            }
        }

        /// <summary>
        /// Publish event
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="eventMessage">Event message</param>
        public virtual void Publish<T>(T eventMessage)
        {
            //get all event subscribers, excluding from not installed plugins
            var subscribers = _subscriptionService.GetSubscriptions<T>().ToList();

            //publish event to subscribers
            subscribers.ForEach(subscriber => PublishToConsumer(subscriber, eventMessage));
        }
    }
}
