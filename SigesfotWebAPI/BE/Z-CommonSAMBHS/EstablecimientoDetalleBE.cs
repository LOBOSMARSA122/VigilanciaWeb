using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMBHSBE.EstablecimientoDetalle
{
    [Table("establecimientodetalle")]
    public class EstablecimientoDetalleBE
    {
        [Key]
        public int i_IdEstablecimientoDetalle { get; set; }

        public int? i_IdEstablecimiento { get; set; }
        public int? i_IdTipoDocumento { get; set; }
        public string v_Serie { get; set; }
        public int? i_Correlativo { get; set; }
        public int? i_Almacen { get; set; }
        public int? i_ImpresionVistaPrevia { get; set; }
        public string v_NombreImpresora { get; set; }
        public int? i_DocumentoPredeterminado { get; set; }
        public int? i_Eliminado { get; set; }
        public int? i_InsertaIdUsuario { get; set; }
        public DateTime? t_InsertaFecha { get; set; }
        public int? i_ActualizaIdUsuario { get; set; }
        public DateTime? t_ActualizaFecha { get; set; }
        public int? i_NumeroItems { get; set; }
    }
}
