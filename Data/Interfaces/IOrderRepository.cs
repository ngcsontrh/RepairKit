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
    }
}
