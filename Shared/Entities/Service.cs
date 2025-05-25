using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class Service
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
