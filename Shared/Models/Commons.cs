using Microsoft.AspNetCore.Http;
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

    public class ConvertToBase64Request
    {
        public IFormFile? File { get; set; }
    }

    public class ConvertToBase64Response
    {
        public string? Base64String { get; set; }
    }
}
