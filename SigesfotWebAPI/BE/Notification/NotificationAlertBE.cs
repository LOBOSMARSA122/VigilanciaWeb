using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Notification
{
   public class NotificationAlertBE
    {
        public string NotificationId { get; set; }
        public string personId { get; set; }
        public string organizationId { get; set; }
        public string planVigilancia { get; set; }
        public int nodeId { get; set; }
        public int systemUserId { get; set; }
    }
}
