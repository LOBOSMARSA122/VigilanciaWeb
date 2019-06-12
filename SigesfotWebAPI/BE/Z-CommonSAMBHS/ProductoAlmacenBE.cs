using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Z_CommonSAMBHS
{
    [Table("productoalmacen")]
    public class ProductoAlmacenBE
    {
        [Key]
        public string v_IdProductoAlmacen { get; set; }

        public int i_IdAlmacen { get; set; }
        public string v_Periodo { get; set; }
        public string v_ProductoDetalleId { get; set; }
        public decimal? d_StockMinimo { get; set; }
        public decimal? d_StockMaximo { get; set; }
        public decimal? d_StockActual { get; set; }
        public decimal? d_SeparacionTotal { get; set; }
        public int? i_Eliminado { get; set; }
        public int? i_InsertaIdUsuario { get; set; }
        public DateTime? t_InsertaFecha { get; set; }
        public int? i_ActualizaIdUsuario { get; set; }
        public DateTime? t_ActualizaFecha { get; set; }
        public string v_NroPedido { get; set; }
        public DateTime? t_FechaCaducidad { get; set; }
        public string v_NroSerie { get; set; }
        public string v_NroLote { get; set; }
    }
}
