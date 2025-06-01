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
        Task<(List<Order>, int)> GetPageByFilterAsync(OrderFilter filter, int offset = 0, int limit = 10);
        Task<Order?> GetDetailAsync(Guid id);
    }
}
