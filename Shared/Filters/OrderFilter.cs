using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Filters
{
    public class OrderFilter : FilterBase
    {
        public Guid? CustomerId { get; set; }
        public Guid? RepairmanId { get; set; }
        public string? Status { get; set; }
        public bool? PaymentStatus { get; set; }
    }
}
