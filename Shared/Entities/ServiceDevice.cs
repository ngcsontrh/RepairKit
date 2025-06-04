using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class ServiceDevice
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public Guid ServiceId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public Service? Service { get; set; }
        public List<DeviceDetail>? DeviceDetails { get; set; }
    }
}
