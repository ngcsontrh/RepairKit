using Data.Config;
using Data.Interfaces;
using Shared.Entities;

namespace Data.Implementations
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context)
        {
        }
    }
}
