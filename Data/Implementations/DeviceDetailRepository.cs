using Data.Config;
using Data.Interfaces;
using Shared.Entities;

namespace Data.Implementations
{
    public class DeviceDetailRepository : Repository<DeviceDetail>, IDeviceDetailRepository
    {
        public DeviceDetailRepository(AppDbContext context) : base(context)
        {
        }
    }
}
