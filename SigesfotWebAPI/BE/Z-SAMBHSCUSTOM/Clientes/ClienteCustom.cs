using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Z_SAMBHSCUSTOM.Clientes
{
    public class Boards
    {
        public int TotalRecords { get; set; }
        public int Index { get; set; }
        public int Take { get; set; }
    }

    public class BoardCliente : Boards
    {
        public string Name { get; set; }
        public List<ClienteCustom> List { get; set; }
    }

    public class ClienteCustom
    {
        public string NombreRazonSocial { get; set; }
        public string TipoDocumento { get; set; }
        public string v_UsuarioCreacion { get; set; }
        public string v_UsuarioModificacion { get; set; }
        public string v_IdCliente { get; set; }
        public string v_CodCliente { get; set; }
        public string v_NroDocIdentificacion { get; set; }
        public int? i_IdTipoIdentificacion { get; set; }
        public int? i_IdTipoPersona { get; set; }
        public int? i_IdLista { get; set; }
        public int? i_IdSexo { get; set; }
        public int? i_IdPais { get; set; }
        public int? i_IdProvincia { get; set; }
        public int? i_IdDistrito { get; set; }
        public int? i_IdDepartamento { get; set; }
        public int? i_Nacionalidad { get; set; }
        public string v_FlagPantalla { get; set; }
        public string v_RazonSocial { get; set; }
        public string v_Correo { get; set; }
        public string v_TelefonoFijo { get; set; }
        public string v_TelefonoMovil { get; set; }
        public DateTime t_FechaNacimiento { get; set; }
        public DateTime? t_ActualizaFecha { get; set; }
        public DateTime? t_InsertaFecha { get; set; }
        public string v_PrimerNombre { get; set; }
        public string v_SegundoNombre { get; set; }
        public string v_ApePaterno { get; set; }
        public string v_ApeMaterno { get; set; }
        public string v_Direccion { get; set; }
        public string TipoDocumentoTrabajadores { get; set; }
        public int? i_ParameterId { get; set; }
        public int? i_Activo { get; set; }
        public int? i_EsProveedorServicios { get; set; }
        public int? i_IdDireccionCliente { get; set; }
    }
}
