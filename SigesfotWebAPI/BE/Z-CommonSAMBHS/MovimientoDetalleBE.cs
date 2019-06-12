using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Z_CommonSAMBHS
{
    [Table("movimientodetalle")]
    public class MovimientoDetalleBE
    {
        [Key]
        public string v_IdMovimientoDetalle { get; set; }

        public string v_IdMovimiento { get; set; }
        public string v_IdProductoDetalle { get; set; }
        public string v_IdMovimientoDetalleTransferencia { get; set; }
        public string v_NroGuiaRemision { get; set; }
        public int? i_IdTipoDocumento { get; set; }
        public string v_NumeroDocumento { get; set; }
        public decimal? d_Cantidad { get; set; }
        public decimal? d_CantidadEmpaque { get; set; }
        public decimal? d_CantidadAdministrativa { get; set; }
        public decimal? d_CantidadEmpaqueAdministrativa { get; set; }
        public int? i_IdUnidad { get; set; }
        public decimal? d_Precio { get; set; }
        public decimal? d_Total { get; set; }
        public string v_NroPedido { get; set; }
        public int? i_Eliminado { get; set; }
        public int? i_InsertaIdUsuario { get; set; }
        public DateTime? t_InsertaFecha { get; set; }
        public int? i_ActualizaIdUsuario { get; set; }
        public DateTime? t_ActualizaFecha { get; set; }
        public string v_IdRecetaFinal { get; set; }
        public int? i_EsProductoFinal { get; set; }
        public decimal? d_PrecioCambio { get; set; }
        public decimal? d_TotalCambio { get; set; }
        public int? i_IdCentroCosto { get; set; }
        public DateTime? t_FechaCaducidad { get; set; }
        public DateTime? t_FechaFabricacion { get; set; }
        public string v_NroSerie { get; set; }
        public string v_NroLote { get; set; }
        public string v_NroOrdenProduccion { get; set; }
    }
}
