using Data.Config;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Shared.Filters;

namespace Data.Implementations
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Order?> GetDetailAsync(Guid id)
        {
            var order = await _context.Orders
                .Where(o => o.Id == id)
                .Include(o => o.Customer!)
                .Include(o => o.Repairman!)
                .Include(o => o.Address!)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (order == null) return null;

            order.OrderDetails = await _context.OrderDetails
                .Where(od => od.OrderId == order.Id)
                .Include(od => od.DeviceDetail!)
                    .ThenInclude(dd => dd.ServiceDevice!)
                        .ThenInclude(sd => sd.Service!)
                .AsNoTracking()
                .ToListAsync();

            return order;
        }

        public async Task<(List<Order>, int)> GetPageByFilterAsync(OrderFilter filter, int offset = 0, int limit = 10)
        {
            var query = _context.Orders.AsQueryable();
            if (filter.CustomerId.HasValue)
            {
                query = query.Where(o => o.CustomerId == filter.CustomerId.Value);
            }
            if (filter.RepairmanId.HasValue)
            {
                query = query.Where(o => o.RepairmanId == filter.RepairmanId.Value);
            }
            if (filter.Status != null)
            {
                query = query.Where(o => o.Status == filter.Status);
            }
            if (filter.PaymentStatus.HasValue)
            {
                query = query.Where(o => o.PaymentStatus == filter.PaymentStatus.Value);
            }
            var entities = await query
                .OrderBy(o => o.Id)
                .Skip(offset)
                .Take(limit)
                .Include(o => o.Customer)                    
                .Include(o => o.Repairman)
                .Include(o => o.Address)
                .AsNoTracking()
                .ToListAsync();
            var total = await query.CountAsync();
            return (entities, total);
        }
    }
}
