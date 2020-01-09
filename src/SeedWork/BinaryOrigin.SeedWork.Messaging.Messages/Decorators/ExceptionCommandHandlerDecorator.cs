using BinaryOrigin.SeedWork.Core.Domain;
using System;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Messages.Decorators
{
    public sealed class ExceptionCommandHandlerDecorator<TCommand, TCommandResult>
                : BaseCommandHandlerDecorator<TCommand, TCommandResult>
                        where TCommand : ICommand<TCommandResult>
    {
        public ExceptionCommandHandlerDecorator(ICommandHandler<TCommand, TCommandResult> handler)
            : base(handler)
        {
        }

        public async override Task<Result<TCommandResult>> HandleAsync(TCommand command)
        {
            try
            {
                return await Handler.HandleAsync(command);
            }
            catch (Exception exception)
            {
                return Result.Fail<TCommandResult>(exception.ToString());
            }
        }
    }
}