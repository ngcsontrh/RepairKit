using Data.Config;
using Data.Interfaces;
using Shared.Entities;

namespace Data.Implementations
{
    public class ServiceDeviceRepository : Repository<ServiceDevice>, IServiceDeviceRepository
    {
        public ServiceDeviceRepository(AppDbContext context) : base(context)
        {
        }
    }
}
