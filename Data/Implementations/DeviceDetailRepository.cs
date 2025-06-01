using Data.Config;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Shared.Filters;

namespace Data.Implementations
{
    public class DeviceDetailRepository : Repository<DeviceDetail>, IDeviceDetailRepository
    {
        public DeviceDetailRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<(List<DeviceDetail>, int)> GetListWithFilterAsync(DeviceDetailFilter filter)
        {
            var query = _context.DeviceDetails.AsQueryable();

            if (filter.ServiceDeviceId.HasValue)
            {
                query = query.Where(dd => dd.ServiceDeviceId == filter.ServiceDeviceId.Value);
            }

            var entities = await query
                .Skip(filter.Offset)
                .Take(filter.Limit)
                .ToListAsync();

            var totalCount = await query.CountAsync();
            return (entities, totalCount);
        }
    }
}
