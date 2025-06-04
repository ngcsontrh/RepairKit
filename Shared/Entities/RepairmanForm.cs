using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class RepairmanForm
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public Guid UserId { get; set; }
        public string? Status { get; set; } = RepairmanFormStatus.Processing.ToString();
        public string? Areas { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public User? User { get; set; }
        public RepairmanFormDetail? Detail { get; set; }
    }
}
