using MyApp.Domain.Example.BankAccountAgg;

namespace MyApp.Infrastructure.Data.Repositories
{
   public class BankAccountRepository:EfRepository<BankAccount>,IBankAccountRepository
    {
        public BankAccountRepository(IDbContext context) : base(context)
        {
        }
    }
}
