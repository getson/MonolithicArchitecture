using BinaryOrigin.SeedWork.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Messages.Decorators
{
    [DecoratorOrder(2)]
    public class LocalizationCommandHandlerDecorator<TCommand, TCommandResult>
                : ICommandHandlerDecorator<TCommand, TCommandResult>
                        where TCommand : ICommand<TCommandResult>
    {
        private readonly ILocalizerService _localizerService;

        public LocalizationCommandHandlerDecorator(ICommandHandler<TCommand, TCommandResult> handler,
                                                   ILocalizerService localizerService)
        {
            Handler = handler;
            _localizerService = localizerService;
        }

        public ICommandHandler<TCommand, TCommandResult> Handler { get; }

        public async Task<Result<TCommandResult>> HandleAsync(TCommand command)
        {
            var result = await Handler.HandleAsync(command).ConfigureAwait(false);
            if (result.IsFailure)
            {
                return Result.Fail<TCommandResult>(
                    _localizerService.Localize(result.Error)
                    );
            }
            return result;
        }
    }
}
