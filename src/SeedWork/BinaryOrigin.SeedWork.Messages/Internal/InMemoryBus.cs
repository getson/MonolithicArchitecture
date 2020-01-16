using BinaryOrigin.SeedWork.Core.Exceptions;
using BinaryOrigin.SeedWork.Core.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Messages
{
    public sealed class InMemoryBus : IBus
    {
        private readonly IHandlerResolver _handlerResolver;

        public InMemoryBus(IHandlerResolver handlerResolver)
        {
            _handlerResolver = handlerResolver;
        }

        public async Task<TCommandResult> ExecuteAsync<TCommandResult>(ICommand<TCommandResult> command)
        {
            var handlerInstance = (dynamic)_handlerResolver.ResolveCommandHandler(command, typeof(ICommandHandler<,>));
          
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
            var orderedHandlers = _handlerResolver.ResolveEventHandlers<ISequenceEventHandler<TEvent>>()
                                                  .OrderBy(x=>x.Order);

            foreach(var handler in orderedHandlers)
            {
                await handler.HandleAsync(@event);
            }
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(@event);
            }
        }
    }

}