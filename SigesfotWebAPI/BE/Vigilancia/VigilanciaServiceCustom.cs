using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Vigilancia
{
   public class VigilanciaServiceCustom
    {
        public string PersonId { get; set; }
        public string ServiceId { get; set; }
        public string Trabajador { get; set; }
        public string DateSchedule { get; set; }
        public int DoctoResponsibleId { get; set; }
        public string DoctoResponsibleName { get; set; }
    }
}
