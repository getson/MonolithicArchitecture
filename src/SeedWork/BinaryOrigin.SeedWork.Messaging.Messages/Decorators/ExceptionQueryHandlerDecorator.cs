using BinaryOrigin.SeedWork.Core.Domain;
using System;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Messages.Decorators
{
    public sealed class ExceptionQueryHandlerDecorator<TQuery, TQueryResult> : BaseQueryHandlerDecorator<TQuery, TQueryResult>
        where TQuery : IQuery<TQueryResult>
    {
        public ExceptionQueryHandlerDecorator(IQueryHandler<TQuery, TQueryResult> handler) : base(handler)
        {
        }

        public async override Task<Result<TQueryResult>> HandleAsync(TQuery queryModel)
        {
            try
            {
                return await Handler.HandleAsync(queryModel);
            }
            catch (Exception exception)
            {
                return Result.Fail<TQueryResult>(exception.ToString());
            }
        }
    }
}