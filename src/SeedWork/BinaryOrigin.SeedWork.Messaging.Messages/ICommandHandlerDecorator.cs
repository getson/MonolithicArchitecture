using BinaryOrigin.SeedWork.Core.Domain;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Messages.Decorators
{
    public interface ICommandHandlerDecorator<TCommand, TCommandResult>
        : ICommandHandler<TCommand, TCommandResult>
            where TCommand : ICommand<TCommandResult>
    {
        ICommandHandler<TCommand, TCommandResult> Handler { get; }
    }
}