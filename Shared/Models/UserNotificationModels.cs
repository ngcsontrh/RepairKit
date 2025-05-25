using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class UserNotificationDto
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? NotificationId { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? ReadAt { get; set; }

        public NotificationDto? Notification { get; set; }
    }
}
