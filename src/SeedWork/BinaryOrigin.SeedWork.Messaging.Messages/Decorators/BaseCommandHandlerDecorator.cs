using BinaryOrigin.SeedWork.Core.Domain;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Messages.Decorators
{
    public abstract class BaseCommandHandlerDecorator<TCommand, TCommandResult> : ICommandHandler<TCommand, TCommandResult>
        where TCommand : ICommand<TCommandResult>
    {
        protected readonly ICommandHandler<TCommand, TCommandResult> Handler;

        protected BaseCommandHandlerDecorator(ICommandHandler<TCommand, TCommandResult> handler)
        {
            Handler = handler;
        }
        public abstract Task<Result<TCommandResult>> HandleAsync(TCommand command);

    }
}