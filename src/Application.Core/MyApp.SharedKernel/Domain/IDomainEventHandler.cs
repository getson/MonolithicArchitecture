namespace MyApp.Core.SharedKernel.Events
{
    public interface IDomainEventHandler<in T> where T : IDomainEvent
    {
        void Handle(T domainEvent);
    }
}
