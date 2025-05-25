using Data.Config;
using Data.Interfaces;
using Shared.Entities;

namespace Data.Implementations
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(AppDbContext context) : base(context)
        {
        }
    }
}
