using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class CartDetail
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public Guid CartId { get; set; }
        public Guid ServiceDeviceId { get; set; }
        public int? Qty { get; set; }

        public Cart? Cart { get; set; }
        public ServiceDevice? ServiceDevice { get; set; }
    }
}
