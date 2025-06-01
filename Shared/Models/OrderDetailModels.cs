using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class OrderDetailDto
    {
        public Guid? Id { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? DeviceDetailId { get; set; }
        public string? Description { get; set; }
        public string? Video { get; set; }
        public string? Image { get; set; }
        public int? MinPrice { get; set; }

        public DeviceDetailDto? DeviceDetail { get; set; }
    }

    public class CreateOrderDetailRequest
    {
        public Guid? DeviceDetailId { get; set; }
        public string? Description { get; set; }
        public IFormFile? VideoFile { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int? MinPrice { get; set; }
    }
}
