using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IServiceDeviceRepository : IRepository<ServiceDevice>
    {
        Task<(List<ServiceDevice>, int)> GetPageByServiceIdAsync(Guid serviceId, int offset = 0, int limit = 10);
        Task<ServiceDevice?> GetDetailAsync(Guid id);
    }
}
