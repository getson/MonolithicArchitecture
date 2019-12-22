using BinaryOrigin.SeedWork.Messages.Decorators;
using BinaryOrigin.SeedWork.Messages.Validation;

namespace BinaryOrigin.SeedWork.Messages
{
    public sealed class InMemoryBus : BaseBus
    {
        public InMemoryBus(IHandlerResolver handlerResolver,
                           ICommandValidationProvider validationProvider)
            : base(handlerResolver,validationProvider)
        {
            RegisterQueryHandlerDecorator(typeof(ExceptionQueryHandlerDecorator<,>));
            RegisterCommandHandlerDecorator(typeof(ExceptionCommandHandlerDecorator<,>));
        }
    }
}