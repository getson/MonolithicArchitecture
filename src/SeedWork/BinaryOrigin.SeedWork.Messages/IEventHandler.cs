using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Messages
{
    /// <summary>
    /// This type should be used when we want to create handlers for events
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IEventHandler<in TMessage> where TMessage : IEvent
    {
        Task HandleAsync(TMessage message);
    }
}