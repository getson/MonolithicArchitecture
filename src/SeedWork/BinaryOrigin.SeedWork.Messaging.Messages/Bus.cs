using BinaryOrigin.SeedWork.Messages.Decorators;

namespace BinaryOrigin.SeedWork.Messages
{
    public sealed class Bus : BaseBus
    {
        public Bus()
            : base(new CommandHandlerResolver(),
                   new QueryHandlerResolver(),
                   new EventHandlerResolver())
        {
            RegisterQueryHandlerDecorator(typeof(ExceptionQueryHandlerDecorator<,>));
            RegisterCommandHandlerDecorator(typeof(ExceptionCommandHandlerDecorator<,>));
        }
    }
}