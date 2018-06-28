using System;
using System.Collections.Generic;
using System.Text;
using MyApp.Core.Domain.Example.OrderAgg;

namespace MyApp.Infrastructure.Data.Repositories
{
    public class OrderRepository:EfRepository<Order>,IOrderRepository
    {
        public OrderRepository(IDbContext context) : base(context)
        {
        }
    }
}
