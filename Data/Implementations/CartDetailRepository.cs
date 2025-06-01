using Data.Config;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Data.Implementations
{
    public class CartDetailRepository : Repository<CartDetail>, ICartDetailRepository
    {
        public CartDetailRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<CartDetail?> GetByCartAndServiceDeviceAsync(Guid cartId, Guid serviceDeviceId)
        {
            var entity = await _context.CartDetails
                .FirstOrDefaultAsync(cd => cd.CartId == cartId && cd.ServiceDeviceId == serviceDeviceId);
            return entity;
        }
    }
}
