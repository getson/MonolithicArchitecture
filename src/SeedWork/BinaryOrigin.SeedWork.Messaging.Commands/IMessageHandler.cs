using BinaryOrigin.SeedWork.Core.Domain;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Commands
{
    public interface IMessageHandler<in TMessage> where TMessage : IMessage
    {
        Task<Result> HandleAsync(TMessage message);
    }
}
