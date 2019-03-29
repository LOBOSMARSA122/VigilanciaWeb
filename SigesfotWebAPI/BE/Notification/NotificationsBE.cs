using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Notification
{
   public class NotificationsBE
    {
        public string PersonId { get; set; }
        public string NotificationId { get; set; }

        public string Organization { get; set; }
        public int? TypeNotificationId { get; set; }
        public string TypeNotification { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime? NotificationDate { get; set; }
        public string NotificationDateString { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public string ScheduleDateString { get; set; }

        public int? SystemUserId { get; set; }
        public string SystemUser { get; set; }
        public int? IsRead { get; set; }
        public string Read { get; set; }
        public string Worker { get; set; }
        public string StateNotification { get; set; }
    }
}
