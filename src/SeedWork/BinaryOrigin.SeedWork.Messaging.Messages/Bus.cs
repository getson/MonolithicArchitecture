using BinaryOrigin.SeedWork.Messages.Decorators;
using BinaryOrigin.SeedWork.Messages.Validation;

namespace BinaryOrigin.SeedWork.Messages
{
    public sealed class Bus : BaseBus
    {
        public Bus(IValidationProvider validationProvider)
            : base(new CommandHandlerResolver(),
                   new QueryHandlerResolver(),
                   new EventHandlerResolver(),
                   validationProvider)
        {
            RegisterQueryHandlerDecorator(typeof(ExceptionQueryHandlerDecorator<,>));
            RegisterCommandHandlerDecorator(typeof(ExceptionCommandHandlerDecorator<,>));
        }
    }
}