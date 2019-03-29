using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Models.Notification
{
    public class NotificationAlert
    {
        public string personId { get; set; }
        public string organizationId { get; set; }
        public string planVigilancia { get; set; }
        public int nodeId { get; set; }
        public int systemUserId { get; set; }
    }
}