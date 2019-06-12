using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Z_CommonSAMBHS
{
    [Table("movimiento")]
    public class MovimientoBE
    {
        [Key]
        public string v_IdMovimiento { get; set; }

        public string v_Periodo { get; set; }
        public string v_Mes { get; set; }
        public string v_Correlativo { get; set; }
        public int? i_IdEstablecimiento { get; set; }
        public int? i_IdAlmacenOrigen { get; set; }
        public int? i_IdAlmacenDestino { get; set; }
        public string v_IdCliente { get; set; }
        public int? i_IdTipoMovimiento { get; set; }
        public DateTime? t_Fecha { get; set; }
        public decimal? d_TipoCambio { get; set; }
        public int? i_IdTipoMotivo { get; set; }
        public int? i_IdMoneda { get; set; }
        public string v_Glosa { get; set; }
        public decimal? d_TotalPrecio { get; set; }
        public decimal? d_TotalCantidad { get; set; }
        public string v_OrigenTipo { get; set; }
        public string v_OrigenRegPeriodo { get; set; }
        public string v_OrigenRegMes { get; set; }
        public string v_OrigenRegCorrelativo { get; set; }
        public int? i_EsDevolucion { get; set; }
        public int? i_Eliminado { get; set; }
        public int? i_InsertaIdUsuario { get; set; }
        public DateTime? t_InsertaFecha { get; set; }
        public int? i_ActualizaIdUsuario { get; set; }
        public DateTime? t_ActualizaFecha { get; set; }
        public string v_IdMovimientoOrigen { get; set; }
        public int? i_GenerarGuia { get; set; }
        public string v_NroOrdenCompra { get; set; }
        public int? i_IdTipoDocumento { get; set; }
        public string v_SerieDocumento { get; set; }
        public string v_CorrelativoDocumento { get; set; }
        public string v_NroGuiaVenta { get; set; }
        public int? i_IdDireccionCliente { get; set; }
        public string v_MotivoEliminacion { get; set; }
    }
}
