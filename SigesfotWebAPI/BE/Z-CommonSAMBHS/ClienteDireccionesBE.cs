using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Z_CommonSAMBHS
{
    [Table("clientedirecciones")]
    public class ClienteDireccionesBE
    {
        [Key]
        public int i_IdDireccionCliente { get; set; }

        public string v_Direccion { get; set; }
        public string v_IdCliente { get; set; }
        public int? i_IdDepartamento { get; set; }
        public int? i_IdProvincia { get; set; }
        public int? i_IdZona { get; set; }
        public int? i_IdDistrito { get; set; }
        public int? i_Eliminado { get; set; }
        public int? i_EsDireccionPredeterminada { get; set; }
        public int? i_InsertaIdUsuario { get; set; }
        public DateTime? t_InsertaFecha { get; set; }
        public int? i_ActualizaIdUsuario { get; set; }
        public DateTime? t_ActualizaFecha { get; set; }
    }
}
