using Shared.Entities;
using Shared.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IServiceDeviceRepository : IRepository<ServiceDevice>
    {
        Task<(List<ServiceDevice> ServiceDevices, int TotalCount)> GetListWithFilterAsync(ServiceDeviceFilter filter);
        Task<ServiceDevice?> GetDetailAsync(Guid id);
    }
}
