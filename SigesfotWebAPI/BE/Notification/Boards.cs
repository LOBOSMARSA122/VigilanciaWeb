using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Notification
{
    public class Boards
    {
        public int TotalRecords { get; set; }
        public int Index { get; set; }
        public int Take { get; set; }
    }

    public class BoardNotification : Boards
    {
        public string NotificationDateStart { get; set; }
        public string NotificationDateEnd { get; set; }

        public int? TypeNotificationId { get; set; }
        public string OrganizationId { get; set; }
        public string Worker { get; set; }
        public string Title { get; set; }
        public int? StateNotificationId { get; set; }

        public List<NotificationsBE> list { get; set; }
    }

}
