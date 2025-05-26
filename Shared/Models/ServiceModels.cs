using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class ServiceDto
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public List<ServiceDeviceDto>? ServiceDevices { get; set; }
    }

    public class CreateServiceRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateServiceRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
