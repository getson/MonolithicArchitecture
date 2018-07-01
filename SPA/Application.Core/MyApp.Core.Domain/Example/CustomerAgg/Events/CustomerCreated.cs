using System;
using MyApp.Core.SharedKernel.Events;

namespace MyApp.Core.Domain.Example.CustomerAgg.Events
{
    public class CustomerCreatedEvent : IDomainEvent
    {

        public CustomerCreatedEvent(string customerName)
        {
            CustomerName = customerName;
            DateOfCreated=DateTime.Now;
        }
        public string CustomerName { get; }
        public DateTime DateOfCreated { get; }
    }
}
