using MyApp.Core.Domain.Events;

namespace MyApp.Core.Domain.Common
{
    public interface IHandler<T>
        where T : IDomainEvent
    {
        void Handle(T domainEvent);
    }
}
