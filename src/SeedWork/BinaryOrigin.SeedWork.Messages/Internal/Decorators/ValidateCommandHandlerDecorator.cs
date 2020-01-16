using BinaryOrigin.SeedWork.Core.Domain;
using BinaryOrigin.SeedWork.Core.Exceptions;
using BinaryOrigin.SeedWork.Messages.Validation;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Messages.Decorators
{
    [DecoratorOrder(1)]
    public class ValidateCommandHandlerDecorator<TCommand, TCommandResult>
            : ICommandHandlerDecorator<TCommand, TCommandResult>
                    where TCommand : ICommand<TCommandResult>
    {
        private readonly ICommandValidationProvider _validationProvider;

        public ValidateCommandHandlerDecorator(ICommandHandler<TCommand, TCommandResult> handler,
                                               ICommandValidationProvider validationProvider)
        {
            Handler = handler;
            _validationProvider = validationProvider;
        }

        public ICommandHandler<TCommand, TCommandResult> Handler { get; }

        public async Task<Result<TCommandResult>> HandleAsync(TCommand command)
        {
            var validationResult = await _validationProvider.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new CommandValidationException(validationResult.Errors);
            }
            return await Handler.HandleAsync(command).ConfigureAwait(false);
        }
    }
}
