using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.ReportManager
{
    public class OrdenReportesBe
    {
        public string OrdenReporteId { get; set; }
        public string ComponenteId { get; set; }
        public string NombreReporte { get; set; }
        public int? Orden { get; set; }
        public string NombreCrystal { get; set; }
        public int? NombreCrystalId { get; set; }
    }
}
