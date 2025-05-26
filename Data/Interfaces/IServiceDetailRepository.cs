using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;

namespace Data.Interfaces
{
    public interface IServiceDetailRepository : IRepository<ServiceDetail>
    {
        Task<(List<ServiceDetail>, int)> GetPageByServiceDeviceIdAsync(Guid serviceDeviceId, int offset = 0, int limit = 10);
    }
}
