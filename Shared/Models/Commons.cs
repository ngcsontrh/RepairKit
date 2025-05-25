using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class PageData<T> where T : class
    {
        public List<T> Items { get; set; } = new List<T>();
        public int Total { get; set; }
    }
}
