using Data.Config;
using Data.Interfaces;
using Shared.Entities;

namespace Data.Implementations
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }
    }
}
