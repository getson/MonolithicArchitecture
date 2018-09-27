using System;

namespace MyApp.Domain.Example.CustomerAgg.Events
{
    public class CustomerCreatedEvent
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
