using Data.Config;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Data.Implementations
{
    public class DeviceDetailRepository : Repository<DeviceDetail>, IDeviceDetailRepository
    {
        public DeviceDetailRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<(List<DeviceDetail>, int)> GetPageByServiceDeviceIdAsync(Guid serviceDeviceId, int offset = 0, int limit = 10)
        {
            var entities = await _context.DeviceDetails
                .Where(x => x.ServiceDeviceId == serviceDeviceId)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
            var total = await _context.DeviceDetails.CountAsync(x => x.ServiceDeviceId == serviceDeviceId);
            return (entities, total);
        }
    }
}
