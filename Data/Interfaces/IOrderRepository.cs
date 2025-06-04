using Shared;
using Shared.Entities;
using Shared.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<(List<Order>, int)> GetPageByFilterAsync(OrderFilter filter);
        Task<Order?> GetDetailAsync(Guid id);
        Task<(long today, long thisWeek, long thisMonth)> GetRevenueAsync();
        Task<(int today, int thisWeek, int thisMonth)> GetNewOrderCountAsync();
        Task<(int inProgress, int completed, int canceled)> GetOrderStatusCountAsync();
    }
}
