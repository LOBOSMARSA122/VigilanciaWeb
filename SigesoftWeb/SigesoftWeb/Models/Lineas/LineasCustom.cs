using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Models.Lineas
{
    public class LineasCustom
    {
        public string v_IdLinea { get; set; }

        public string v_Periodo { get; set; }
        public string v_CodLinea { get; set; }
        public string v_Nombre { get; set; }
        public string v_NroCuentaVenta { get; set; }
        public string v_NroCuentaCompra { get; set; }
        public string v_NroCuentaDConsumo { get; set; }
        public string v_NroCuentaHConsumo { get; set; }
        public int? i_Eliminado { get; set; }
        public int? i_InsertaIdUsuario { get; set; }
        public DateTime? t_InsertaFecha { get; set; }
        public int? i_ActualizaIdUsuario { get; set; }
        public DateTime? t_ActualizaFecha { get; set; }
        public byte[] b_Foto { get; set; }
        public int? i_Header { get; set; }

    }
}