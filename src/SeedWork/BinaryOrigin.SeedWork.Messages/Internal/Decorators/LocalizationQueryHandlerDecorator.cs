using BinaryOrigin.SeedWork.Core.Domain;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.Messages.Decorators
{
    [DecoratorOrder(1)]
    public sealed class LocalizationQueryHandlerDecorator<TQuery, TQueryResult>
            : IQueryHandlerDecorator<TQuery, TQueryResult>
            where TQuery : IQuery<TQueryResult>
    {
        private readonly ILocalizerService _localizerService;

        public LocalizationQueryHandlerDecorator(IQueryHandler<TQuery, TQueryResult> handler,
                                                 ILocalizerService localizerService)
        {
            Handler = handler;
            _localizerService = localizerService;
        }

        public IQueryHandler<TQuery, TQueryResult> Handler { get; }

        public async Task<Result<TQueryResult>> HandleAsync(TQuery queryModel)
        {
            var result = await Handler.HandleAsync(queryModel).ConfigureAwait(false);
            if (result.IsFailure)
            {
                return Result.Fail<TQueryResult>(
                    _localizerService.Localize(result.Error)
                    );
            }
            return result;
        }
    }
}