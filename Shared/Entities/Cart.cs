using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class Cart
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public Guid UserId { get; set; }
        public int? Qty { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public User? User { get; set; }

        public List<CartDetail>? CartDetails { get; set; }
    }
}
