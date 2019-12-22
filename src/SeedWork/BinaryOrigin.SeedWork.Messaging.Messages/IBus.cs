using BinaryOrigin.SeedWork.Core.Domain;
using System;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Messages
{
    public interface IBus
    {
        Task<Result<TCommandResult>> ExecuteAsync<TCommandResult>(ICommand<TCommandResult> command);

        Task<Result<TQueryResult>> QueryAsync<TQueryResult>(IQuery<TQueryResult> queryModel);

        Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;

        void RegisterCommandHandlerDecorator(Type decorator);

        void RegisterQueryHandlerDecorator(Type decorator);
    }
}