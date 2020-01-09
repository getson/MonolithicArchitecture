using BinaryOrigin.SeedWork.Core.Domain;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Messages.Decorators
{
    public interface IQueryHandlerDecorator<TQuery, TQueryResult>
         : IQueryHandler<TQuery, TQueryResult>
            where TQuery : IQuery<TQueryResult>
    {
        IQueryHandler<TQuery, TQueryResult> Handler { get; }
    }
}