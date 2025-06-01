using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class CartDetailDto
    {
        public Guid? Id { get; set; }
        public Guid? CartId { get; set; }
        public Guid? ServiceDeviceId { get; set; }
        public int? Qty { get; set; }
    }

    public class AddToCartRequest
    {
        public Guid? ServiceDeviceId { get; set; }
        public int? Qty { get; set; }
    }

    public class UpdateCartDetailRequest
    {
        public int? Qty { get; set; }
    }
}
