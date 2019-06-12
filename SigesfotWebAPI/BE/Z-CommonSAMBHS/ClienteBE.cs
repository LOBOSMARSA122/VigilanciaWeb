using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Z_CommonSAMBHS
{
    [Table("cliente")]
    public class ClienteBE
    {
        [Key]
        public string v_IdCliente { get; set; }

        public string v_IdVendedor { get; set; }
        public string v_CodCliente { get; set; }
        public string v_FlagPantalla { get; set; }
        public int? i_IdTipoIdentificacion { get; set; }
        public string v_NroDocIdentificacion { get; set; }
        public int? i_IdTipoPersona { get; set; }
        public string v_PrimerNombre { get; set; }
        public string v_SegundoNombre { get; set; }
        public string v_ApePaterno { get; set; }
        public string v_ApeMaterno { get; set; }
        public string v_RazonSocial { get; set; }
        public int? i_IdSexo { get; set; }
        public string v_DirecPrincipal { get; set; }
        public int? i_IdPais { get; set; }
        public int? i_IdDepartamento { get; set; }
        public int? i_IdProvincia { get; set; }
        public int? i_IdDistrito { get; set; }
        public string v_DirecSecundaria { get; set; }
        public string v_TelefonoFijo { get; set; }
        public string v_TelefonoFax { get; set; }
        public string v_TelefonoMovil { get; set; }
        public string v_Correo { get; set; }
        public string v_PaginaWeb { get; set; }
        public int? i_Activo { get; set; }
        public DateTime? t_FechaNacimiento { get; set; }
        public int? i_Nacionalidad { get; set; }
        public string v_NombreContacto { get; set; }
        public int? i_IdGrupoCliente { get; set; }
        public int? i_IdListaPrecios { get; set; }
        public int? i_IdZona { get; set; }
        public int? i_EsPrestadorServicios { get; set; }
        public string v_Servicio { get; set; }
        public int? i_IdConvenioDobleTributacion { get; set; }
        public int? i_AfectoDetraccion { get; set; }
        public int? i_UsaLineaCredito { get; set; }
        public int? i_Eliminado { get; set; }
        public int? i_InsertaIdUsuario { get; set; }
        public string v_NroCuentaDetraccion { get; set; }
        public DateTime? t_InsertaFecha { get; set; }
        public int? i_ActualizaIdUsuario { get; set; }
        public DateTime? t_ActualizaFecha { get; set; }
        public string v_Password { get; set; }
        public string v_Alias { get; set; }
        public string v_MotivoEliminacion { get; set; }
        public int? i_IdTipoAccionesSocio { get; set; }
        public int? i_NumeroAccionesSuscritas { get; set; }
        public int? i_NumeroAccionesPagadas { get; set; }
    }
}
