using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class RepairmanFormDto
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public string? Status { get; set; }
        public string? Areas { get; set; }

        public RepairmanFormDetailDto? Detail { get; set; }
    }

    public class CreateRepairmanFormRequest
    {
        public Guid? UserId { get; set; }
        public string? Areas { get; set; }
        public Guid? ServiceDeviceId { get; set; }
        public int? YearsOfExperience { get; set; }
        public string? Description { get; set; }
        public IFormFile? DegreeFile { get; set; }
    }

    public class UpdateRepairmanFormStatusRequest
    {
        public string? Status { get; set; }
    }
}
