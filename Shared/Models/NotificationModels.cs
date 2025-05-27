using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class NotificationDto
    {
        public Guid? Id { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? RepairmanFormId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
    }

    public class CreateNotificationRequest
    {
        public List<Guid>? UserIds { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }

    public class CreateOrderNotificationRequest : CreateNotificationRequest
    {
        public Guid? OrderId { get; set; }
    }

    public class CreateRepairmanFormNotificationRequest : CreateNotificationRequest
    {
        public Guid? RepairmanFormId { get; set; }
    }

    public class MarkNotificationAsReadRequest
    {
        public List<Guid>? NotificationIds { get; set; }
    }
}
