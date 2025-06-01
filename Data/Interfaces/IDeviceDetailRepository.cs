using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IDeviceDetailRepository : IRepository<Shared.Entities.DeviceDetail>
    {
        Task<(List<DeviceDetail>, int)> GetPageByServiceDeviceIdAsync(Guid serviceDeviceId, int offset = 0, int limit = 10);
    }
}
