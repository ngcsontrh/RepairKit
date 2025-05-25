using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class AddressUser
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public bool? AddressMain { get; set; }
        public string? Street { get; set; }
        public string? Ward { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        
        public User? User { get; set; }
    }
}
