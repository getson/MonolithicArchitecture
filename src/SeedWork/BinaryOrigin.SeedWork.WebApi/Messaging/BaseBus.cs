using BinaryOrigin.SeedWork.Commands;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Core.Domain;
using BinaryOrigin.SeedWork.Queries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.WebApi.Messaging
{
    public class BaseBus : IBus
    {
        private readonly HashSet<Type> _commandHandlerDecorators = new HashSet<Type>();
        private readonly HashSet<Type> _queryHandlerDecorators = new HashSet<Type>();
        private readonly IHandlerResolver _commandHandlerResolver;
        private readonly IHandlerResolver _queryHandlerResolver;
        private readonly IMessageHandlerResolver _messageHandlerResolver;

        protected BaseBus(IHandlerResolver commandHandlerResolver,
                          IHandlerResolver queryHandlerResolver,
                          IMessageHandlerResolver messageHandlerResolver)
        {
            _commandHandlerResolver = commandHandlerResolver;
            _queryHandlerResolver = queryHandlerResolver;
            _messageHandlerResolver = messageHandlerResolver;
        }

        public async Task<Result<TCommandResult>> ExecuteAsync<TCommandResult>(ICommand<TCommandResult> command)
        {
            var handlerType = _commandHandlerResolver.Get(command.GetType());
            var instance = EngineContext.Current.Resolve(handlerType.GetInterfaces()[0]);

            var handler = DecorateCommand(command, instance);

            if (handler.Validate((dynamic)command))
            {
                return await handler.HandleAsync((dynamic)command);
            }
            return Result.Fail<TCommandResult>("Invalid Command");
        }

        public async Task<Result<TQueryResult>> QueryAsync<TQueryResult>(IQuery<TQueryResult> queryModel)
        {
            var handlerType = _queryHandlerResolver.Get(queryModel.GetType());
            var handlerInstance = EngineContext.Current.Resolve(handlerType.GetInterfaces()[0]);

            var handler = DecorateQuery(queryModel, handlerInstance);
            return await handler.HandleAsync((dynamic)queryModel);
        }

        public async Task<Result> HandleMessageAsync(IMessage message)
        {
            var handlers = _messageHandlerResolver.Get(message.GetType());
            foreach (var handlerType in handlers)
            {
                var handlerInstance = (dynamic)EngineContext.Current.Resolve(handlerType);
                //TODO log message and status
                await handlerInstance.HandleAsync((dynamic)message);
            }
            return Result.Ok();
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