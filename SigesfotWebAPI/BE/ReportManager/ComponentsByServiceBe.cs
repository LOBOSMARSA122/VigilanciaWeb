using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.ReportManager
{
    public class ComponentsByServiceBe
    {
        public string ComponentId { get; set; }
        public string ComponentName { get; set; }
        public string ServiceComponentId { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
