
using MyApp.Domain.Example.CustomerAgg;

namespace MyApp.Infrastructure.Data.Repositories
{
    public class CustomerRepository : EfRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(IDbContext context) : base(context)
        {
        }
    }
}
