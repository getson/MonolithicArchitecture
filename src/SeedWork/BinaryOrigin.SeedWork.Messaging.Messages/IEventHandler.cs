using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Messages
{
    public interface IEventHandler<in TMessage> where TMessage : IEvent
    {
        Task HandleAsync(TMessage message);
    }
}