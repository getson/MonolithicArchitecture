using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp.Core.SharedKernel.Events
{
    public class DomainEvents
    {
        public static readonly DomainEvents Instance;

        private List<Delegate> _actions;
        private IServiceProvider _serviceProvider;

        static DomainEvents()
        {
            Instance = new DomainEvents();
        }
        public void Init(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            if (_actions == null)
            {
                _actions = new List<Delegate>();
            }
            _actions.Add(callback);
        }

        public void ClearCallbacks()
        {
            _actions = null;
        }
        //public void Dispatch(IDomainEvent domainEvent)
        //{
        //    foreach (var handlerType in _handlers)
        //    {
        //        bool canHandleEvent = CanHandleEvent(domainEvent, handlerType);

        //        if (!canHandleEvent) continue;

        //        dynamic handler = Activator.CreateInstance(handlerType);
        //        handler.Handle((dynamic)domainEvent);
        //    }
        //}

        private static bool CanHandleEvent(IDomainEvent domainEvent, Type handlerType)
        {
            return handlerType.GetInterfaces()
                .Any(x => x.IsGenericType
                    && x.GetGenericTypeDefinition() == typeof(IHandler<>)
                    && x.GenericTypeArguments[0] == domainEvent.GetType());
        }
        public void Raise<T>(T args) where T : IDomainEvent
        {
            var handlers = _serviceProvider.GetServices<IHandler<T>>();

            foreach (var handler in handlers)
            {
                handler.Handle(args);
            }

            if (_actions != null)
            {
                foreach (var action in _actions)
                {
                    (action as Action<T>)?.Invoke(args);
                }
            }
        }
    }
}
