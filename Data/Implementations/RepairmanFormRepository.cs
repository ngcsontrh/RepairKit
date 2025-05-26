using Data.Config;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Data.Implementations
{
    public class RepairmanFormRepository : Repository<RepairmanForm>, IRepairmanFormRepository
    {
        public RepairmanFormRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<RepairmanForm?> GetDetailAsync(Guid id)
        {
            var entity = await _context.RepairmanForms
                .Where(x => x.Id == id)
                .Include(x => x.Detail)
                .FirstOrDefaultAsync();
            return entity;
        }
    }
}
