using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Filters
{
    public class UserFilter
    {
        public int? Offset { get; set; }
        public int? Limit { get; set; }
        public string? Role { get; set; }
        public string? Status { get; set; }
    }
}
