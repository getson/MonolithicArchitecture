using BinaryOrigin.SeedWork.Core.Domain;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Messages
{
    public interface ICommandHandler<in TCommand, TCommandResult>
        where TCommand : ICommand<TCommandResult>
    {
        Task<Result<TCommandResult>> HandleAsync(TCommand command);
    }
}