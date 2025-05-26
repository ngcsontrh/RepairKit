using Data.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implementations
{
    public class RepairmanFormDetailRepository : Repository<Shared.Entities.RepairmanFormDetail>, Data.Interfaces.IRepairmanFormDetailRepository
    {
        public RepairmanFormDetailRepository(AppDbContext context) : base(context)
        {
        }
    }
}
