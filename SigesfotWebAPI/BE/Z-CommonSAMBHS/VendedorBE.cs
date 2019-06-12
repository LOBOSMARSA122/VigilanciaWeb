using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMBHSBE.Common
{
    [Table("vendedor")]
    public class VendedorBE
    {
        [Key]
        public string v_IdVendedor { get; set; }

        public int? i_SystemUser { get; set; }
        public string v_CodVendedor { get; set; }
        public string v_NombreCompleto { get; set; }
        public string v_Contacto { get; set; }
        public string v_Direccion { get; set; }
        public int? i_IdTipoPersona { get; set; }
        public int? i_IdTipoIdentificacion { get; set; }
        public string v_NroDocIdentificacion { get; set; }
        public string v_Telefono { get; set; }
        public string v_Fax { get; set; }
        public string v_Correo { get; set; }
        public int? i_IdPais { get; set; }
        public int? i_IdDepartamento { get; set; }
        public int? i_IdProvincia { get; set; }
        public int? i_IdDistrito { get; set; }
        public int? i_Eliminado { get; set; }
        public int? i_InsertaIdUsuario { get; set; }
        public DateTime? t_InsertaFecha { get; set; }
        public int? i_ActualizaIdUsuario { get; set; }
        public DateTime? t_ActualizaFecha { get; set; }
        public int? i_PermiteAnularVentas { get; set; }
        public int? i_PermiteEliminarVentas { get; set; }
        public int? i_IdAlmacen { get; set; }
        public int? i_EsActivo { get; set; }
    }
}
