using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Models.Notification
{
    public class SendNotificationBE
    {
        public string Subs { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }

    public class Message
    {
        public string title { get; set; }
        public string message { get; set; }

    }
}