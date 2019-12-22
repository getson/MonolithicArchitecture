using BinaryOrigin.SeedWork.Core.Domain;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Messages.Validation
{
    public interface ICommandValidationProvider
    {
        Task<ValidationResponse> ValidateAsync<TCommandResult>(ICommand<TCommandResult> command);
    }
}