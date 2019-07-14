namespace MyApp.Core.SharedKernel.Events
{
    public interface IHandler<in T> where T : IDomainEvent
    {
        void Handle(T domainEvent);
    }
}
