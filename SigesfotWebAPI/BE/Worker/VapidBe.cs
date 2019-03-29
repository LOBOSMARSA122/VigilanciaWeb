using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Worker
{
    public class VapidBe
    {
        public string PersonId { get; set; }
        public string Subs { get; set; }
    }

    public class KeyNotification
    {
        public string EndPoint { get; set; }
        public string p256dh { get; set; }
        public string auth { get; set; }
    }
}
