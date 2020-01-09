using BinaryOrigin.SeedWork.Core.Exceptions;
using BinaryOrigin.SeedWork.Messages.Decorators;
using BinaryOrigin.SeedWork.Messages.Validation;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Messages
{
    public sealed class InMemoryBus : IBus
    {
        private readonly IHandlerResolver _handlerResolver;
        private readonly ICommandValidationProvider _validationProvider;

        public InMemoryBus(IHandlerResolver handlerResolver,
                           ICommandValidationProvider validationProvider)
        {
            _handlerResolver = handlerResolver;
            _validationProvider = validationProvider;
        }

        public async Task<TCommandResult> ExecuteAsync<TCommandResult>(ICommand<TCommandResult> command)
        {
            var handlerInstance = (dynamic)_handlerResolver.ResolveCommandHandler(command, typeof(ICommandHandler<,>));

            var validationResult = await _validationProvider.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new CommandValidationException(validationResult.Errors);
            }

            var result = await handlerInstance.HandleAsync((dynamic)command);
            if (result.IsSuccess)
            {
                return result.Value;
            }
            throw new GeneralException(result.Error);
        }

        public async Task<TQueryResult> QueryAsync<TQueryResult>(IQuery<TQueryResult> query)
        {
            var handlerInstance = (dynamic)_handlerResolver.ResolveQueryHandler(query, typeof(IQueryHandler<,>));

            var result = await handlerInstance.HandleAsync((dynamic)query);
            if (result.IsSuccess)
            {
                return result.Value;
            }
            throw new GeneralException(result.Error);
        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var handlers = _handlerResolver.ResolveEventHandlers<IEventHandler<TEvent>>();
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(@event);
            }
        }
    }

}