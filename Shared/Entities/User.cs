using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Avatar { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Status { get; set; } = UserStatus.Active.ToString();
        public string? Role { get; set; } = UserRole.Customer.ToString();
        public int? Average { get; set; }
        public int? ReviewCount { get; set; }
        public string? Bio { get; set; }
        public string? WorkingStatus { get; set; }
    }
}
