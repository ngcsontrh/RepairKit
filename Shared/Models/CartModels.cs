using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class CartDto
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public int? Qty { get; set; }
        public List<CartDetailDto>? CartDetails { get; set; }
    }
}
