using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Models.Schedule
{
    public class ScheduleCustom
    {
        public string PersonId { get; set; }
        public string VigilanciId { get; set; }
        public int TypeId { get; set; }
        public string Commentary { get; set; }
        public DateTime? ScheduleDate { get; set; }
    }
}