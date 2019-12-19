using BinaryOrigin.SeedWork.Core.Domain;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Queries
{
    public interface IQueryHandler<in TQuery, TQueryResult>
        where TQuery : IQuery<TQueryResult>
    {
        Task<Result<TQueryResult>> HandleAsync(TQuery queryModel);
    }
}