using Data.Config;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Data.Implementations
{
    public class ServiceDetailRepository : Repository<ServiceDetail>, IServiceDetailRepository
    {
        public ServiceDetailRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<(List<ServiceDetail>, int)> GetPageByServiceDeviceIdAsync(Guid serviceDeviceId, int offset = 0, int limit = 10)
        {
            var entities = await _context.ServiceDetails
                .Where(x => x.ServiceDeviceId == serviceDeviceId)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
            var total = await _context.ServiceDetails.CountAsync(x => x.ServiceDeviceId == serviceDeviceId);
            return (entities, total);
        }
    }
}
