using Data.Config;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared;
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
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (order == null) return null;

            order.OrderDetails = await _context.OrderDetails
                .Where(od => od.OrderId == order.Id)
                .Include(od => od.DeviceDetail!)                    
                .AsNoTracking()
                .ToListAsync();

            return order;
        }

        public async Task<(int today, int thisWeek, int thisMonth)> GetNewOrderCountAsync()
        {
            var todayStart = DateTime.UtcNow.Date;
            var thisWeekStart = DateTime.UtcNow.Date.AddDays(-((7 + (int)DateTime.UtcNow.Date.DayOfWeek - 1) % 7));
            var thisMonthStart = new DateTime(DateTime.UtcNow.Date.Year, DateTime.UtcNow.Date.Month, 1);

            var todayEnd = todayStart.AddDays(1).AddTicks(-1);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddTicks(-1);
            var thisMonthEnd = new DateTime(DateTime.UtcNow.Date.Year, DateTime.UtcNow.Date.Month, 1).AddMonths(1).AddTicks(-1);

            var todayCount = await _context.Orders
                .CountAsync(o => o.CreatedAt >= todayStart && o.CreatedAt <= todayEnd);
            var thisWeekCount = await _context.Orders
                .CountAsync(o => o.CreatedAt >= thisWeekStart && o.CreatedAt <= thisWeekEnd);
            var thisMonthCount = await _context.Orders
                .CountAsync(o => o.CreatedAt >= thisMonthStart && o.CreatedAt <= thisMonthEnd);

            return (todayCount, thisWeekCount, thisMonthCount);
        }

        public async Task<(int inProgress, int completed, int canceled)> GetOrderStatusCountAsync()
        {
            var inProgressCount = await _context.Orders.CountAsync(o => o.Status == OrderStatus.InProgress.ToString());
            var completedCount = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Completed.ToString());
            var canceledCount = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Canceled.ToString());

            return (inProgressCount, completedCount, canceledCount);
        }

        public async Task<(List<Order>, int)> GetPageByFilterAsync(OrderFilter filter)
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
                .Skip(filter.Offset)
                .Take(filter.Limit)
                .AsNoTracking()
                .ToListAsync();
            var total = await query.CountAsync();
            return (entities, total);
        }

        public async Task<(long today, long thisWeek, long thisMonth)> GetRevenueAsync()
        {
            var todayStart = DateTime.UtcNow.Date;
            var thisWeekStart = DateTime.UtcNow.Date.AddDays(-((7 + (int)DateTime.UtcNow.Date.DayOfWeek - 1) % 7));
            var thisMonthStart = new DateTime(DateTime.UtcNow.Date.Year, DateTime.UtcNow.Date.Month, 1);

            var todayEnd = todayStart.AddDays(1).AddTicks(-1);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddTicks(-1);
            var thisMonthEnd = new DateTime(DateTime.UtcNow.Date.Year, DateTime.UtcNow.Date.Month, 1).AddMonths(1).AddTicks(-1);

            var todayRevenue = await _context.Orders
                .Where(o => o.CreatedAt >= todayStart && o.CreatedAt <= todayEnd)
                .SumAsync(o => (long?)o.Total) ?? 0;

            var thisWeekRevenue = await _context.Orders
                .Where(o => o.CreatedAt >= thisWeekStart && o.CreatedAt <= thisWeekEnd)
                .SumAsync(o => (long?)o.Total) ?? 0;

            var thisMonthRevenue = await _context.Orders
                .Where(o => o.CreatedAt >= thisMonthStart && o.CreatedAt <= thisMonthEnd)
                .SumAsync(o => (long?)o.Total) ?? 0;

            return (todayRevenue, thisWeekRevenue, thisMonthRevenue);
        }


    }
}
