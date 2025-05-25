using Data.Config;
using Data.Interfaces;
using Shared.Entities;

namespace Data.Implementations
{
    public class ServiceDetailRepository : Repository<ServiceDetail>, IServiceDetailRepository
    {
        public ServiceDetailRepository(AppDbContext context) : base(context)
        {
        }
    }
}
