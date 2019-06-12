using BE.Z_SAMBHSCUSTOM.Clientes;
using SAMBHSDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ClientesSAMBHS
{
    public class ClientesDal
    {
        public BoardCliente GetClients(BoardCliente data)
        {
            try
            {
                string filtValue = data.Name == "" ? null : data.Name;
                DatabaseSAMBHSContext cnx = new DatabaseSAMBHSContext();
                var query = (from A in cnx.Cliente
                            join J1 in cnx.SystemUser on new { i_InsertUserId = A.i_InsertaIdUsuario.Value }
                                                        equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                            from J1 in J1_join.DefaultIfEmpty()

                            join J2 in cnx.SystemUser on new { i_UpdateUserId = A.i_ActualizaIdUsuario.Value }
                                                        equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                            from J2 in J2_join.DefaultIfEmpty()

                            join J3 in cnx.SystemParameter on new { a = A.i_IdTipoIdentificacion.Value, b = 150 }
                                                             equals new { a = J3.i_ParameterId, b = J3.i_GroupId } into J3_join
                            from J3 in J3_join.DefaultIfEmpty()

                            join J4 in cnx.ClienteDirecciones on new { IdDireccion = A.v_IdCliente, eliminado = 0, predeterminado = 1 }
                                                    equals new { IdDireccion = J4.v_IdCliente, eliminado = J4.i_Eliminado.Value, predeterminado = J4.i_EsDireccionPredeterminada.Value } into J4_join
                            from J4 in J4_join.DefaultIfEmpty()

                            where A.i_Eliminado == 0 && (A.v_RazonSocial.Contains(filtValue) || A.v_NroDocIdentificacion.Contains(filtValue) || filtValue == null)

                            select new ClienteCustom
                            {
                                NombreRazonSocial = (A.v_ApePaterno + " " + A.v_ApeMaterno + " " + A.v_PrimerNombre + " " + A.v_SegundoNombre + " " + A.v_RazonSocial).Trim(),
                                v_IdCliente = A.v_IdCliente,
                                v_ApeMaterno = A.v_ApeMaterno,
                                v_ApePaterno = A.v_ApePaterno,
                                v_CodCliente = A.v_CodCliente,
                                i_IdLista = A.i_IdListaPrecios,
                                v_NroDocIdentificacion = A.v_NroDocIdentificacion,
                                v_PrimerNombre = A.v_PrimerNombre,
                                v_RazonSocial = (A.v_ApePaterno + " " + A.v_ApeMaterno + " " + A.v_PrimerNombre + " " + A.v_SegundoNombre + " " + A.v_RazonSocial).Trim(),
                                v_SegundoNombre = A.v_SegundoNombre,
                                i_IdTipoIdentificacion = A.i_IdTipoIdentificacion,
                                i_IdTipoPersona = A.i_IdTipoPersona,
                                t_ActualizaFecha = A.t_ActualizaFecha.Value,
                                t_InsertaFecha = A.t_InsertaFecha.Value,
                                v_UsuarioCreacion = J1.v_UserName,
                                v_UsuarioModificacion = J2.v_UserName,
                                TipoDocumento = J3.v_Value1,
                                v_FlagPantalla = A.v_FlagPantalla,
                                v_Direccion = J4 == null ? A.v_DirecPrincipal : J4.v_Direccion,
                                i_ParameterId = J3.i_ParameterId,
                                i_IdDireccionCliente = J4 == null ? -1 : J4.i_IdDireccionCliente
                            }).ToList();

                int skip = (data.Index - 1) * data.Take;
                var ListClients = query.GroupBy(g => g.v_IdCliente).Select(s => s.First()).ToList();
                data.TotalRecords = ListClients.Count;

                if (data.Take > 0)
                    ListClients = ListClients.Skip(skip).Take(data.Take).ToList();

                data.List = ListClients.OrderBy(x => x.v_NroDocIdentificacion).ToList();
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
