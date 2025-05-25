using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class ServiceDetail
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public Guid ServiceDeviceId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? MinPrice { get; set; }

        public ServiceDevice? ServiceDevice { get; set; }
    }
}
