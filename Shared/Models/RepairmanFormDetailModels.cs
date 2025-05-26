using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class RepairmanFormDetailDto
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public Guid RepairmanFormId { get; set; }
        public Guid ServiceDeviceId { get; set; }
        public int? YearsOfExperience { get; set; }
        public string? Description { get; set; }
        public string? Degree { get; set; }
    }
}
