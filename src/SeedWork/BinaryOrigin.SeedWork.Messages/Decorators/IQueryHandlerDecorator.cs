namespace BinaryOrigin.SeedWork.Messages
{
    public interface IQueryHandlerDecorator<TQuery, TQueryResult>
         : IQueryHandler<TQuery, TQueryResult>
            where TQuery : IQuery<TQueryResult>
    {
        IQueryHandler<TQuery, TQueryResult> Handler { get; }
    }
}