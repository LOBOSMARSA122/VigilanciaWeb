using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Models.ReportManagerBe
{
    public class ReportManagerBe
    {
        public string ServiceId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentId { get; set; }
    }

    public class ComponentsServiceId
    {
        public string ComponentId { get; set; }
        public string ServiceId { get; set; }
    }
}