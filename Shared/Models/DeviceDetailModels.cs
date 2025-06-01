using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class DeviceDetailDto
    {
        public Guid? Id { get; set; }
        public Guid? ServiceDeviceId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? MinPrice { get; set; }
    }

    public class CreateDeviceDetailRequest
    {
        public Guid? ServiceDeviceId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? MinPrice { get; set; }
    }

    public class UpdateDeviceDetailRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? MinPrice { get; set; }
    }
}
