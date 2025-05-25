using Data.Config;
using Data.Interfaces;
using Shared.Entities;

namespace Data.Implementations
{
    public class RepairmanFormRepository : Repository<RepairmanForm>, IRepairmanFormRepository
    {
        public RepairmanFormRepository(AppDbContext context) : base(context)
        {
        }
    }
}
