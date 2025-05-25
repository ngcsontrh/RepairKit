using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Filters
{
    public class UserFilter : FilterBase
    {        
        public string? Role { get; set; }
        public string? Status { get; set; }
    }
}
