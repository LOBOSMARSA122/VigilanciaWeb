using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Z_CommonSAMBHS
{
    [Table("productodetalle")]
    public class ProductoDetalleBE
    {
        [Key]
        public string v_IdProductoDetalle { get; set; }

        public string v_IdProducto { get; set; }
        public string v_IdColor { get; set; }
        public string v_IdTalla { get; set; }
        public int? i_Eliminado { get; set; }
        public int? i_InsertaIdUsuario { get; set; }
        public DateTime? t_InsertaFecha { get; set; }
        public int? i_ActualizaIdUsuario { get; set; }
        public DateTime? t_ActualizaFecha { get; set; }
    }
}
