using Data.Config;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Data.Implementations
{
    public class ServiceDeviceRepository : Repository<ServiceDevice>, IServiceDeviceRepository
    {
        public ServiceDeviceRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<ServiceDevice?> GetDetailAsync(Guid id)
        {
            var entity = await _context.ServiceDevices
                .Where(sd => sd.Id == id)
                .Include(sd => sd.ServiceDetails)
                .FirstOrDefaultAsync();
            return entity;
        }

        public async Task<(List<ServiceDevice>, int)> GetPageByServiceIdAsync(Guid serviceId, int offset = 0, int limit = 10)
        {
            var entities = await _context.ServiceDevices
                .Where(sd => sd.ServiceId == serviceId)
                .OrderBy(sd => sd.Id)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
            var total = await _context.ServiceDevices.CountAsync(sd => sd.ServiceId == serviceId);
            return (entities, total);
        }
    }
}
