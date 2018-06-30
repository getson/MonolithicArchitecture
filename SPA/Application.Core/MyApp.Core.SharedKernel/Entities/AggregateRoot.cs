using System.Collections.Generic;
using MyApp.Core.SharedKernel.Events;

namespace MyApp.Core.SharedKernel.Entities
{
    public abstract class AggregateRoot : BaseEntity
    {
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        public virtual IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

        protected virtual void AddDomainEvent(IDomainEvent newEvent)
        {
            _domainEvents.Add(newEvent);
        }

        public virtual void ClearEvents()
        {
            _domainEvents.Clear();
        }
    }
}
