using MyApp.SharedKernel.Domain;

namespace MyApp.Domain.Example.OrderAgg
{
    public interface IOrderRepository : IRepository<Order>
    {
    }
}
