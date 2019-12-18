using System;
using System.Collections.Generic;
using BinaryOrigin.SeedWork.Commands;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Queries;

namespace BinaryOrigin.SeedWork.Application.Messaging
{
    public class BaseBus : IBus
    {
        private readonly HashSet<Type> _commandHandlerDecorators = new HashSet<Type>();
        private readonly HashSet<Type> _queryHandlerDecorators = new HashSet<Type>();
        private readonly IHandlerResolver _commandHandlerResolver;
        private readonly IHandlerResolver _queryHandlerResolver;

        protected BaseBus(IHandlerResolver commandHandlerResolver, IHandlerResolver queryHandlerResolver)
        {
            _commandHandlerResolver = commandHandlerResolver;
            _queryHandlerResolver = queryHandlerResolver;
        }

        public CommandResponse<TCommandResult> Execute<TCommandResult>(ICommand<TCommandResult> command)
        {
            var handlerType = _commandHandlerResolver.Get(command.GetType());
            var instance = EngineContext.Current.Resolve(handlerType.GetInterfaces()[0]);

            var handler = DecorateCommand(command, instance);

            if (handler.Validate((dynamic)command))
            {
                return handler.Handle((dynamic)command);
            }

            // DAMTODO
            throw new ArgumentException("Invalid Command");
        }

        public QueryResponse<TQueryResult> Query<TQueryResult>(IQuery<TQueryResult> queryObject)
        {
            var handlerType = _queryHandlerResolver.Get(queryObject.GetType());
            var handlerInstance = EngineContext.Current.Resolve(handlerType.GetInterfaces()[0]);

            var handler = DecorateQuery(queryObject, handlerInstance);
            return handler.Handle((dynamic)queryObject);
        }

        public void RegisterCommandHandlerDecorator(Type decorator)
        {
            _commandHandlerDecorators.Add(decorator);
        }

        public void RegisterQueryHandlerDecorator(Type decorator)
        {
            _queryHandlerDecorators.Add(decorator);
        }

        private dynamic DecorateCommand<TCommandResult>(ICommand<TCommandResult> command, dynamic handler)
        {
            foreach (var decorator in _commandHandlerDecorators)
            {
                handler = Activator.CreateInstance(
                    decorator.MakeGenericType(command.GetType(), typeof(TCommandResult)),
                    handler
                );
            }

            return handler;
        }

        private dynamic DecorateQuery<TQueryResult>(IQuery<TQueryResult> queryObject, dynamic handler)
        {
            foreach (var decorator in _queryHandlerDecorators)
            {
                handler = Activator.CreateInstance(
                    decorator.MakeGenericType(queryObject.GetType(), typeof(TQueryResult)),
                    handler
                );
            }

            return handler;
        }
    }
}