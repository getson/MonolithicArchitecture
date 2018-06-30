using System.Diagnostics;
using MyApp.Core.Domain.Example.CustomerAgg;
using MyApp.Core.SharedKernel.Events;

namespace MyApp.Core.Domain.Services.Banking
{
  public  class CustomerServiceHandler:IHandler<CustomerCreatedEvent>
    {
         
        public void Handle(CustomerCreatedEvent domainEvent)
        {
            Debug.WriteLine(domainEvent.CustomerName);
        }
    }
}
