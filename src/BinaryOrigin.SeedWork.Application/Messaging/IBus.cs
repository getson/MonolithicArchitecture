using System;
using BinaryOrigin.SeedWork.Commands;
using BinaryOrigin.SeedWork.Queries;

namespace BinaryOrigin.SeedWork.Application.Messaging
{
    public interface IBus
    {
        CommandResponse<TCommandResult> Execute<TCommandResult>(ICommand<TCommandResult> command);

        void RegisterCommandHandlerDecorator(Type decorator);

        QueryResponse<TQueryResult> Query<TQueryResult>(IQuery<TQueryResult> queryObject);

        void RegisterQueryHandlerDecorator(Type decorator);
    }
}