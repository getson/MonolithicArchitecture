using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Messages
{
    public interface IBus
    {
        Task<TCommandResult> ExecuteAsync<TCommandResult>(ICommand<TCommandResult> command);

        Task<TQueryResult> QueryAsync<TQueryResult>(IQuery<TQueryResult> queryModel);

        Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}