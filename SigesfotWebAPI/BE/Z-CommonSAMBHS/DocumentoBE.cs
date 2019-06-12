using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMBHSBE.Documento
{
    [Table("documento")]
    public class DocumentoBE
    {
        [Key]
        public int i_CodigoDocumento { get; set; }

        public string v_Nombre { get; set; }
        public string v_Siglas { get; set; }
        public int? i_UsadoDocumentoContable { get; set; }
        public int? i_UsadoDocumentoInterno { get; set; }
        public int? i_UsadoDocumentoInverso { get; set; }
        public int? i_UsadoCompras { get; set; }
        public int? i_UsadoContabilidad { get; set; }
        public int? i_UsadoLibroDiario { get; set; }
        public int? i_UsadoTesoreria { get; set; }
        public int? i_UsadoVentas { get; set; }
        public int? i_RequiereSerieNumero { get; set; }
        public int? i_UsadoRendicionCuentas { get; set; }
        public int? i_Naturaleza { get; set; }
        public int? i_IdFormaPago { get; set; }
        public string v_NroCuenta { get; set; }
        public string v_provimp_3i { get; set; }
        public int? i_Destino { get; set; }
        public int? i_UsadoPedidoCotizacion { get; set; }
        public int? i_Eliminado { get; set; }
        public int? i_InsertaIdUsuario { get; set; }
        public DateTime? t_InsertaFecha { get; set; }
        public int? i_ActualizaIdUsuario { get; set; }
        public DateTime? t_ActualizaFecha { get; set; }
        public int? i_DescontarStock { get; set; }
        public int? i_OperacionTransitoria { get; set; }
        public string v_NroContraCuenta { get; set; }
        public string v_MotivoEliminacion { get; set; }
        public int? i_BancoDetraccion { get; set; }
    }
}
