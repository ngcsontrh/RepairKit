using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class AddressUserDto
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public bool? AddressMain { get; set; }
        public string? Street { get; set; }
        public string? Ward { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
    }

    public class CreateAddressUserRequest
    {
        public bool? AddressMain { get; set; }
        public string? Street { get; set; }
        public string? Ward { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
    }

    public class UpdateAddressUserRequest
    {
        public bool? AddressMain { get; set; }
        public string? Street { get; set; }
        public string? Ward { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
    }
}
