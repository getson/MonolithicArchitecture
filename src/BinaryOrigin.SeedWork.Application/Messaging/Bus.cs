using BinaryOrigin.SeedWork.Commands.Decorators;
using BinaryOrigin.SeedWork.Queries.Decorators;

namespace BinaryOrigin.SeedWork.Application.Messaging
{
    public sealed class Bus : BaseBus
    {
        public Bus() : base(new CommandHandlerResolver(), new QueryHandlerResolver())
        {
            RegisterQueryHandlerDecorator(typeof(ExceptionQueryHandlerDecorator<,>));
            RegisterCommandHandlerDecorator(typeof(ExceptionCommandHandlerDecorator<,>));
        }
    }
}