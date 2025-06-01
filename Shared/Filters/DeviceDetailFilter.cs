using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Filters
{
    public class DeviceDetailFilter : FilterBase
    {
        public Guid? ServiceDeviceId { get; set; }
    }
}
