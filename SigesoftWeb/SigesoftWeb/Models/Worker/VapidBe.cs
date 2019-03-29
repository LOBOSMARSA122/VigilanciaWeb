using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Models.Worker
{
    public class VapidBe
    {
        public string PersonId { get; set; }
        public string Subs { get; set; }
    }

    public class KeyNotification
    {
        public string endpoint { get; set; }
        public ValueKey Keys { get; set; }
    }

    public class ValueKey
    {
        public string p256dh { get; set; }
        public string auth { get; set; }
    }
    
}