using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Schedule
{
   public class ScheduleCustom
    {
        public string PersonId { get; set; }
        public int TypeId { get; set; }
        public string Commentary { get; set; }
        public DateTime? ScheduleDate{ get; set; }
        public string VigilanciId { get; set; }
    }
}
