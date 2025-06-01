using Shared.Entities;
using Shared.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IDeviceDetailRepository : IRepository<Shared.Entities.DeviceDetail>
    {
        Task<(List<DeviceDetail>, int)> GetListWithFilterAsync(DeviceDetailFilter filter);
    }
}
