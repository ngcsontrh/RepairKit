using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class UserNotification
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public Guid UserId { get; set; }
        public Guid NotificationId { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? ReadAt { get; set; }

        public User? User { get; set; }
        public Notification? Notification { get; set; }
    }
}
