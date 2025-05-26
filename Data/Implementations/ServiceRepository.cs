using Data.Config;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Data.Implementations
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Service?> GetDetailAsync(Guid id)
        {
            var service = await _context.Services
                .Where(s => s.Id == id)
                .Include(s => s.ServiceDevices!)
                    .ThenInclude(sd => sd.ServiceDetails!)
                .FirstOrDefaultAsync(s => s.Id == id);
            return service;
        }
    }
}
