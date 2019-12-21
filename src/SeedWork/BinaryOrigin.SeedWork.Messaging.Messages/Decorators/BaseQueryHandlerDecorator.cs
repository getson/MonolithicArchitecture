using BinaryOrigin.SeedWork.Core.Domain;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Messages.Decorators
{
    public abstract class BaseQueryHandlerDecorator<TQuery, TQueryResult> : IQueryHandler<TQuery, TQueryResult>
        where TQuery : IQuery<TQueryResult>
    {
        protected readonly IQueryHandler<TQuery, TQueryResult> Handler;

        protected BaseQueryHandlerDecorator(IQueryHandler<TQuery, TQueryResult> handler)
        {
            Handler = handler;
        }

        public abstract Task<Result<TQueryResult>> HandleAsync(TQuery queryModel);
    }
}