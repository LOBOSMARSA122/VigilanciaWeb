using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    [Table("receta")]
    public class RecetaBE
    {
        [Key]
        public int i_IdReceta { get; set; }

        public string v_DiagnosticRepositoryId { get; set; }
        public decimal? d_Cantidad { get; set; }
        public string v_Posologia { get; set; }
        public string v_Duracion { get; set; }
        public DateTime? t_FechaFin { get; set; }
        public string v_IdProductoDetalle { get; set; }
        public string v_Lote { get; set; }
        public int? i_IdAlmacen { get; set; }
        public int? i_Lleva { get; set; }
        public int? i_NoLleva { get; set; }
        public string v_IdVentaPaciente { get; set; }
        public string v_IdVentaAseguradora { get; set; }
        public string v_IdUnidadProductiva { get; set; }
        public decimal? d_SaldoPaciente { get; set; }
        public decimal? d_SaldoAseguradora { get; set; }
        public string v_ServiceId { get; set; }
        public string v_ComentaryUpdate { get; set; }
    }
}
