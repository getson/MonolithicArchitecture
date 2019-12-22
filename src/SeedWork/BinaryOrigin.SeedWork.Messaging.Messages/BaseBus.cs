using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Core.Domain;
using BinaryOrigin.SeedWork.Core.Exceptions;
using BinaryOrigin.SeedWork.Messages.Validation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Messages
{
    public class BaseBus : IBus
    {
        private readonly HashSet<Type> _commandHandlerDecorators = new HashSet<Type>();
        private readonly HashSet<Type> _queryHandlerDecorators = new HashSet<Type>();
        private readonly IHandlerResolver _handlerResolver;
        private readonly ICommandValidationProvider _validationProvider;

        protected BaseBus(IHandlerResolver handlerResolver,
                          ICommandValidationProvider validationProvider)
        {
            _handlerResolver = handlerResolver;
            _validationProvider = validationProvider;
        }

        public async Task<Result<TCommandResult>> ExecuteAsync<TCommandResult>(ICommand<TCommandResult> command)
        {

            var handlerInstance = _handlerResolver.ResolveCommandHandler(command, typeof(ICommandHandler<,>));
            var handler = DecorateCommand(command, handlerInstance);

            var validationResult = await _validationProvider.ValidateAsync(command);
            if (validationResult.IsValid)
            {
                return await handler.HandleAsync((dynamic)command);
            }
            throw new CommandValidationException(validationResult.Errors);
        }

        public async Task<Result<TQueryResult>> QueryAsync<TQueryResult>(IQuery<TQueryResult> query)
        {
            var handlerInstance = _handlerResolver.ResolveQueryHandler(query, typeof(IQueryHandler<,>));
            var handler = DecorateQuery(query, handlerInstance);

            return await handler.HandleAsync((dynamic)query);
        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var handlers = _handlerResolver.ResolveEventHandlers<IEventHandler<TEvent>>();
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(@event);
            }
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

        private dynamic DecorateQuery<TQueryResult>(IQuery<TQueryResult> queryModel, dynamic handler)
        {
            foreach (var decorator in _queryHandlerDecorators)
            {
                handler = Activator.CreateInstance(
                    decorator.MakeGenericType(queryModel.GetType(), typeof(TQueryResult)),
                    handler
                );
            }

            return handler;
        }
    }
}