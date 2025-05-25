using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class Notification
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public Guid? OrderId { get; set; }
        public Guid? RepairmanFormId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }

        public Order? Order { get; set; }
        public RepairmanForm? RepairmanForm { get; set; }
    }
}
