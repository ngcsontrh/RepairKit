using Data.Config;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Shared.Filters;

namespace Data.Implementations
{
    public class ServiceDeviceRepository : Repository<ServiceDevice>, IServiceDeviceRepository
    {
        public ServiceDeviceRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<(List<ServiceDevice> ServiceDevices, int TotalCount)> GetListWithFilterAsync(ServiceDeviceFilter filter)
        {
            var query = _context.ServiceDevices.AsQueryable();

            if (filter.ServiceId.HasValue)
            {
                query = query.Where(x => x.ServiceId == filter.ServiceId.Value);
            }

            var totalCount = await query.CountAsync();

            var entities = await query                
                .Skip(filter.Offset)
                .Take(filter.Limit)
                .OrderBy(x => x.Id)
                .ToListAsync();

            return (entities, totalCount);
        }

    }
}
