using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.MedicalAssistance
{
    public class Schedule
    {
        public string PacientId { get; set; }
        public string Pacient { get; set; }
        public DateTime ServiceDate { get; set; }
        public string Color { get; set; }
        public string EsoType { get; set; }
    }
}
