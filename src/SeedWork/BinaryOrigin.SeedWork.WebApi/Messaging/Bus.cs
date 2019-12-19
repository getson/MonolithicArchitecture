using BinaryOrigin.SeedWork.Commands.Decorators;
using BinaryOrigin.SeedWork.Queries.Decorators;

namespace BinaryOrigin.SeedWork.WebApi.Messaging
{
    public sealed class Bus : BaseBus
    {
        public Bus()
            : base(new CommandHandlerResolver(), new QueryHandlerResolver(), new MessageHandlerResolver())
        {
            RegisterQueryHandlerDecorator(typeof(ExceptionQueryHandlerDecorator<,>));
            RegisterCommandHandlerDecorator(typeof(ExceptionCommandHandlerDecorator<,>));
        }
    }
}