using BinaryOrigin.SeedWork.Commands;
using BinaryOrigin.SeedWork.Core.Domain;
using BinaryOrigin.SeedWork.Queries;
using System;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.WebApi.Messaging
{
    public interface IBus
    {
        Task<Result<TCommandResult>> ExecuteAsync<TCommandResult>(ICommand<TCommandResult> command);

        Task<Result<TQueryResult>> QueryAsync<TQueryResult>(IQuery<TQueryResult> queryModel);
        Task<Result> HandleMessageAsync(IMessage message);
        void RegisterCommandHandlerDecorator(Type decorator);
        void RegisterQueryHandlerDecorator(Type decorator);
    }
}