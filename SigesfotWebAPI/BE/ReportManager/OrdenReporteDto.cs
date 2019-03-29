using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.ReportManager
{
    [Table("ordenreporte")]
   public class OrdenReporteDto
    {
        [Key]
        public string v_OrdenReporteId { get; set; }
        public string v_OrganizationId { get; set; }
        public string v_NombreReporte { get; set; }
        public string v_ComponenteId { get; set; }
        public int? i_Orden { get; set; }
        public string v_NombreCrystal { get; set; }
        public int? i_NombreCrystalId { get; set; }
    }
}
