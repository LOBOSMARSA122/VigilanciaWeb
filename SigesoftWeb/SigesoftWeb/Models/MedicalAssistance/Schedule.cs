using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Models.MedicalAssistance
{
    public class Schedule
    {
        public string Pacient { get; set; }
        public DateTime ServiceDate{ get; set; }
        public string Color { get; set; }
        public string EsoType { get; set; }
    }
}