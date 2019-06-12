using BE.Protocol;
using BE.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BE.Common.Enumeratores;

namespace DAL.Protocol
{
    public class ProtocolDal
    {
        private DatabaseContext ctx = new DatabaseContext();
        public ProtocolCustom GetDatosProtocolo(string protocolId)
        {
            try
            {
                ProtocolCustom result = (from pro in ctx.Protocol
                                         join gro in ctx.GroupOccupation on pro.v_GroupOccupationId equals gro.v_GroupOccupationId
                                         join sys in ctx.SystemParameter on new { a = pro.i_EsoTypeId.Value, b = 118 } equals new { a = sys.i_ParameterId, b = sys.i_GroupId }
                                         join org in ctx.Organization on pro.v_CustomerOrganizationId equals org.v_OrganizationId
                                         join org2 in ctx.Organization on pro.v_EmployerOrganizationId equals org2.v_OrganizationId
                                         join org3 in ctx.Organization on pro.v_WorkingOrganizationId equals org3.v_OrganizationId
                                         where pro.v_ProtocolId == protocolId
                                         select new ProtocolCustom
                                         {
                                             Geso = gro.v_Name,
                                             TipoEso = sys.v_Value1,
                                             i_EsoTypeId = pro.i_EsoTypeId.Value,
                                             EmpresaCliente = pro.v_CustomerOrganizationId + "|" + pro.v_CustomerLocationId,
                                             EmpresaEmpleadora = pro.v_EmployerOrganizationId + "|" + pro.v_EmployerLocationId,
                                             EmpresaTrabajo = pro.v_WorkingOrganizationId + "|" + pro.v_WorkingLocationId,
                                             v_EmployerOrganizationId = pro.v_EmployerOrganizationId,
                                             v_EmployerLocationId = pro.v_EmployerLocationId,
                                             v_GroupOccupationId = pro.v_GroupOccupationId
                                         }).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public static bool ExisteProtocoloPropuestoSegunLaEmpresa(string organizationEmployerId, int masterServiceTypeId, int masterServiceId, string groupOccupationName, int esoTypeId)
        {
            try
            {
                DatabaseContext cnx = new DatabaseContext();
                var query = (from pro in cnx.Protocol
                             join gro in cnx.GroupOccupation on pro.v_EmployerLocationId equals gro.v_LocationId
                             where pro.v_EmployerOrganizationId == organizationEmployerId && pro.i_MasterServiceTypeId == masterServiceTypeId
                             && gro.v_Name == groupOccupationName && pro.i_MasterServiceId == masterServiceId && pro.i_EsoTypeId == esoTypeId
                             select pro).ToList();
                return query.Count > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static int ObtenerTipoEmpresaByProtocol(string protocolId)
        {
            try
            {
                DatabaseContext cnx = new DatabaseContext();
                var query = (from pro in cnx.Protocol
                             join org in cnx.Organization on pro.v_EmployerOrganizationId equals org.v_OrganizationId
                             where pro.v_ProtocolId == protocolId select org).FirstOrDefault();

                if (query != null)
                {
                    return query.i_OrganizationTypeId.Value;
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static string AddProtocol(ProtocolBE protocolBE, List<ProtocolComponentDto> ListProtComp, int nodeId, int userId )
        {
            try
            {
                DatabaseContext cnx = new DatabaseContext();
                var newId = new Common.Utils().GetPrimaryKey(nodeId, 20, "PR");
                protocolBE.v_ProtocolId = newId;
                protocolBE.i_IsDeleted = (int)SiNo.No;
                protocolBE.d_InsertDate = DateTime.Now;
                protocolBE.i_InsertUserId = userId;
                cnx.Protocol.Add(protocolBE);
                cnx.SaveChanges();

                var result = ProtocolComponentDal.AddProtocolComponent(ListProtComp, newId, userId, nodeId);
                if (!result) return null;
                return newId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<SystemUserDto> GetSystemUserSigesoft()
        {
            try
            {
                DatabaseContext cnx = new DatabaseContext();
                var List = cnx.SystemUser.Where(x => x.i_SystemUserTypeId == 2).ToList();

                return List;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<ProtocolSystemUserBE> GetProtocolSystemUser(int userId)
        {
            try
            {
                DatabaseContext cnx = new DatabaseContext();
                var List = cnx.ProtocolSystemUser.Where(x => x.i_SystemUserId == userId).GroupBy(y => y.i_ApplicationHierarchyId).Select(z => z.First()).ToList();
                return List;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static bool AddProtocolSystemUser(List<ProtocolSystemUserBE> lista, int userId, int nodeId)
        {
            try
            {
                DatabaseContext cnx = new DatabaseContext();
                foreach (var obj in lista)
                {
                    var newId = new Common.Utils().GetPrimaryKey(nodeId, 44, "PU");
                    obj.i_IsDeleted = (int)SiNo.No;
                    obj.i_InsertUserId = userId;
                    obj.d_InsertDate = DateTime.Now;
                    obj.v_ProtocolSystemUserId = newId;
                    cnx.ProtocolSystemUser.Add(obj);
                }
                return cnx.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
