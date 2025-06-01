using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class ServiceDeviceDto
    {
        public Guid? Id { get; set; }
        public Guid? ServiceId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<DeviceDetailDto>? DeviceDetails { get; set; }
    }

    public class CreateServiceDeviceRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateServiceDeviceRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}