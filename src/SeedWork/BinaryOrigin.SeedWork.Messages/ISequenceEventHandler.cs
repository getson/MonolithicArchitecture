using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Messages
{
    /// <summary>
    /// This type is neccessary to be used when we have more than one EventHandler for an event,
    /// and we want to process that in a specific order
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface ISequenceEventHandler<in TMessage> where TMessage : IEvent
    {
        int Order { get; }
        Task HandleAsync(TMessage message);
    }
}