﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Filters
{
    public class FilterBase
    {
        public int? Offset { get; set; }
        public int? Limit { get; set; }
    }
}
