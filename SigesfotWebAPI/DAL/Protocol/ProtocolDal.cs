using BE.Message;
using BE.Organization;
using BE.Protocol;
using BE.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                                         join org in ctx.Organization on pro.v_CustomerOrganizationId equals org.v_OrganizationId //Cliente
                                         
                                         join org2 in ctx.Organization on pro.v_EmployerOrganizationId equals org2.v_OrganizationId //Empleadora
                                         join loc2 in ctx.Location on pro.v_EmployerLocationId equals loc2.v_LocationId //Location Empleadora
                                         join org3 in ctx.Organization on pro.v_WorkingOrganizationId equals org3.v_OrganizationId //Trabajo
                                         where pro.v_ProtocolId == protocolId
                                         select new ProtocolCustom
                                         {
                                             Geso = gro.v_Name,
                                             TipoEso = sys.v_Value1,
                                             ProtocolName = pro.v_Name,
                                             i_EsoTypeId = pro.i_EsoTypeId.Value,
                                             EmpresaCliente = pro.v_CustomerOrganizationId + "|" + pro.v_CustomerLocationId,
                                             EmpresaEmpleadora = pro.v_EmployerOrganizationId + "|" + pro.v_EmployerLocationId,
                                             EmpresaTrabajo = pro.v_WorkingOrganizationId + "|" + pro.v_WorkingLocationId,
                                             v_EmployerOrganizationId = pro.v_EmployerOrganizationId,
                                             v_EmployerLocationId = pro.v_EmployerLocationId,
                                             v_GroupOccupationId = pro.v_GroupOccupationId,
                                             EmpresaEmpleadoraName = org2.v_Name + "/" + loc2.v_Name,
                                             EmpresaEmpleadoraAdress = org.v_Address,
                                             v_ContacName = org.v_ContacName,
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

        public static string AddProtocol(ProtocolBE protocolBE, List<ProtocolComponentDto> ListProtCompCreate, int nodeId, int userId )
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

                var result = ProtocolComponentDal.AddProtocolComponent(ListProtCompCreate, newId, userId, nodeId);
                if (!result) return null;
                return newId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string UpdateProtocol(ProtocolBE protocolBE, List<ProtocolComponentDto> ListProtCompCreate, List<ProtocolComponentDto> ListProtCompUpdate, int userId, int nodeId)
        {
            try
            {
                DatabaseContext cnx = new DatabaseContext();

                var objProtocol = cnx.Protocol.Where(x => x.v_ProtocolId == protocolBE.v_ProtocolId).FirstOrDefault();

                objProtocol.v_Name = protocolBE.v_Name;
                objProtocol.v_GroupOccupationId = protocolBE.v_GroupOccupationId;
                objProtocol.v_CustomerOrganizationId = protocolBE.v_CustomerOrganizationId;
                objProtocol.v_CustomerLocationId = protocolBE.v_CustomerLocationId;
                objProtocol.i_MasterServiceTypeId = protocolBE.i_MasterServiceTypeId;
                objProtocol.v_EmployerOrganizationId = protocolBE.v_EmployerOrganizationId;
                objProtocol.v_EmployerLocationId = protocolBE.v_EmployerLocationId;
                objProtocol.i_MasterServiceId = protocolBE.i_MasterServiceId;
                objProtocol.v_WorkingOrganizationId = protocolBE.v_WorkingOrganizationId;
                objProtocol.v_WorkingLocationId = protocolBE.v_WorkingLocationId;
                objProtocol.v_CostCenter = protocolBE.v_CostCenter;
                objProtocol.i_EsoTypeId = protocolBE.i_EsoTypeId;
                objProtocol.i_IsActive = protocolBE.i_IsActive;
                objProtocol.r_PriceFactor = protocolBE.r_PriceFactor;
                objProtocol.r_HospitalBedPrice = protocolBE.r_HospitalBedPrice;
                objProtocol.r_DiscountExam = protocolBE.r_DiscountExam;
                objProtocol.r_MedicineDiscount = protocolBE.r_MedicineDiscount;
                objProtocol.i_UpdateUserId = userId;
                objProtocol.d_UpdateDate = DateTime.Now;

                cnx.SaveChanges();
                if (ListProtCompCreate.Count > 0)
                {
                    var result = ProtocolComponentDal.AddProtocolComponent(ListProtCompCreate, objProtocol.v_ProtocolId, userId, nodeId);
                    if (!result) return null;
                }

                if (ListProtCompUpdate.Count > 0)
                {
                    var result = ProtocolComponentDal.UpdateProtocolComponent(ListProtCompUpdate, userId);
                    if (!result) return null;
                }


                return objProtocol.v_ProtocolId;
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

        public static BoardProtocol GetAllProtocols(BoardProtocol data)
        {
            try
            {
                string WorkingOrganization = data.EmpresaTrabajo == "-1" ? "-1" : data.EmpresaTrabajo.Split('|')[0];
                string EmployerOrganization = data.EmpresaEmpleadora == "-1" ? "-1" : data.EmpresaEmpleadora.Split('|')[0];
                string CustomerOrganization = data.EmpresaCliente == "-1" ? "-1" : data.EmpresaCliente.Split('|')[0];

                DatabaseContext dbContext = new DatabaseContext();
                int skip = (data.Index - 1) * data.Take;
                var query = from A in dbContext.Protocol
                            join B in dbContext.Organization on A.v_EmployerOrganizationId equals B.v_OrganizationId
                            join C in dbContext.Location on A.v_EmployerLocationId equals C.v_LocationId
                            join D in dbContext.GroupOccupation on A.v_GroupOccupationId equals D.v_GroupOccupationId
                            join E in dbContext.SystemParameter on new { a = A.i_EsoTypeId.Value, b = 118 }
                                                                equals new { a = E.i_ParameterId, b = E.i_GroupId } into J3_join
                            from E in J3_join.DefaultIfEmpty()

                            join F in dbContext.Organization on A.v_CustomerOrganizationId equals F.v_OrganizationId

                            join I in dbContext.Location on A.v_CustomerLocationId equals I.v_LocationId

                            join G in dbContext.Organization on A.v_WorkingOrganizationId equals G.v_OrganizationId into J4_join
                            from G in J4_join.DefaultIfEmpty()

                            join J in dbContext.Location on A.v_WorkingLocationId equals J.v_LocationId into J6_join
                            from J in J6_join.DefaultIfEmpty()

                            join H in dbContext.SystemParameter on new { a = A.i_MasterServiceId.Value, b = 119 }
                                                                equals new { a = H.i_ParameterId, b = H.i_GroupId } into J5_join
                            from H in J5_join.DefaultIfEmpty()

                            join J1 in dbContext.SystemUser on new { i_InsertUserId = A.i_InsertUserId.Value }
                                                          equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                            from J1 in J1_join.DefaultIfEmpty()

                            join J2 in dbContext.SystemUser on new { i_UpdateUserId = A.i_UpdateUserId.Value }
                                                            equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                            from J2 in J2_join.DefaultIfEmpty()

                            join L in dbContext.ProtocolComponent on A.v_ProtocolId equals L.v_ProtocolId
                            join K in dbContext.Component on L.v_ComponentId equals K.v_ComponentId

                            where A.i_IsDeleted == 0
                            && (A.v_Name.Contains(data.ProtocolName) || data.ProtocolName == null)
                            && (A.i_MasterServiceId == data.MasterService || data.MasterService == -1) && (B.v_OrganizationId == EmployerOrganization || EmployerOrganization == "-1")
                            && (F.v_OrganizationId == CustomerOrganization || CustomerOrganization == "-1") && (G.v_OrganizationId == WorkingOrganization || WorkingOrganization == "-1")
                            && (A.i_MasterServiceTypeId == data.MasterServiceType || data.MasterServiceType == -1) && (D.v_GroupOccupationId == data.GESO || data.GESO == "-1") && (A.i_EsoTypeId == data.EsoType || data.EsoType == -1)
                            && (K.v_Name.Contains(data.ComponentName) || data.ComponentName == null) && A.i_IsActive == data.IsActive
                            select new ProtocolList
                            {
                                v_ProtocolId = A.v_ProtocolId,
                                v_Protocol = A.v_Name,
                                v_Organization = B.v_Name + " / " + C.v_Name,
                                v_Location = C.v_Name,
                                v_EsoType = E.v_Value1,
                                v_GroupOccupation = D.v_Name,
                                v_OrganizationInvoice = F.v_Name + " / " + I.v_Name,
                                v_CostCenter = A.v_CostCenter,
                                v_IntermediaryOrganization = G.v_Name + " / " + J.v_Name,
                                i_ServiceTypeId = A.i_MasterServiceTypeId.Value,
                                v_MasterServiceName = H.v_Value1,
                                i_MasterServiceId = A.i_MasterServiceId.Value,
                                v_OrganizationId = B.v_OrganizationId + "|" + C.v_LocationId,
                                i_EsoTypeId = A.i_EsoTypeId,
                                v_WorkingOrganizationId = G.v_OrganizationId + "|" + J.v_LocationId,
                                v_OrganizationInvoiceId = F.v_OrganizationId + "|" + I.v_LocationId,
                                v_GroupOccupationId = D.v_GroupOccupationId,
                                v_Geso = D.v_Name,
                                v_CreationUser = J1.v_UserName,
                                v_UpdateUser = J2.v_UserName,
                                d_CreationDate = A.d_InsertDate,
                                d_UpdateDate = A.d_UpdateDate,
                                v_LocationId = A.v_EmployerLocationId,
                                v_CustomerLocationId = A.v_CustomerLocationId,
                                v_WorkingLocationId = A.v_WorkingLocationId,
                                i_IsActive = A.i_IsActive,
                                v_ComponenteNombre = K.v_Name,
                                r_PriceFactor = A.r_PriceFactor,
                                r_HospitalBedPrice = A.r_HospitalBedPrice,
                                r_DiscountExam = A.r_DiscountExam,
                                r_MedicineDiscount = A.r_MedicineDiscount,
                                AseguradoraId = A.v_AseguradoraOrganizationId
                            };
                var list = query.ToList();
                list = list.GroupBy(x => x.v_ProtocolId).Select(z => z.First()).OrderByDescending(x => x.v_ProtocolId).ToList(); 
                int totalRecords = list.Count;
                if (data.Take > 0)
                    list = list.Skip(skip).Take(data.Take).ToList();
                data.List = list;
                data.TotalRecords = totalRecords;
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static MessageCustom VerifyExistsProtocol(string protocolName)
        {
            MessageCustom ms = new MessageCustom();
            try
            {
                DatabaseContext ctx = new DatabaseContext();
                var objProtocol = ctx.Protocol.Where(x => x.v_Name == protocolName && x.i_IsDeleted == 0).FirstOrDefault();

                if (objProtocol == null)
                {
                    ms.Error = false;
                    ms.Status = (int)HttpStatusCode.Continue;
                }
                else
                {
                    ms.Error = false;
                    ms.Status = (int)HttpStatusCode.Forbidden;
                    ms.Id = objProtocol.v_Name;
                    ms.Message = "El nombre del protocolo ya existe en la Base de Datos, por favor elija otro.";
                }
                return ms;
            }
            catch (Exception ex)
            {
                ms.Error = false;
                ms.Status = (int)HttpStatusCode.Conflict;
                ms.Message = "Sucedió un error conectandode al servidor, por favor vuelva a intentar.";

                return ms;
            }
        }

        public static ProtocolList GetProtocolById(string pstrProtocolId)
        {
            try
            {
                DatabaseContext dbContext = new DatabaseContext();

                var objProtocol = (from A in dbContext.Protocol
                                   join B in dbContext.Organization on A.v_EmployerOrganizationId equals B.v_OrganizationId
                                   join C in dbContext.Location on A.v_EmployerLocationId equals C.v_LocationId
                                   join D in dbContext.GroupOccupation on A.v_GroupOccupationId equals D.v_GroupOccupationId
                                   join E in dbContext.SystemParameter on new { a = A.i_EsoTypeId.Value, b = 118 } equals new { a = E.i_ParameterId, b = E.i_GroupId }
                                   join F in dbContext.Organization on A.v_CustomerOrganizationId equals F.v_OrganizationId
                                   join H in dbContext.SystemParameter on new { a = A.i_MasterServiceId.Value, b = 119 } equals new { a = H.i_ParameterId, b = H.i_GroupId }
                                   join I in dbContext.Location on A.v_CustomerLocationId equals I.v_LocationId

                                   join J in dbContext.Location on A.v_EmployerLocationId equals J.v_LocationId
                                   join J1 in dbContext.SystemUser on new { i_InsertUserId = A.i_InsertUserId.Value }
                                                                 equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                                   from J1 in J1_join.DefaultIfEmpty()

                                   join J2 in dbContext.SystemUser on new { i_UpdateUserId = A.i_UpdateUserId.Value }
                                                                   equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                                   from J2 in J2_join.DefaultIfEmpty()

                                   join J3 in dbContext.Organization on new { a = A.v_WorkingOrganizationId }
                                           equals new { a = J3.v_OrganizationId } into J3_join
                                   from J3 in J3_join.DefaultIfEmpty()

                                   join J4 in dbContext.Area on new { a = C.v_LocationId }
                                           equals new { a = J4.v_LocationId } into J4_join
                                   from J4 in J4_join.DefaultIfEmpty()

                                   join J5 in dbContext.Ges on new { a = J4.v_AreaId }
                                       equals new { a = J5.v_AreaId } into J5_join
                                   from J5 in J5_join.DefaultIfEmpty()

                                   join J6 in dbContext.Occupation on new { a = J5.v_GesId, b = D.v_GroupOccupationId }
                                               equals new { a = J6.v_GesId, b = J6.v_GroupOccupationId } into J6_join
                                   from J6 in J6_join.DefaultIfEmpty()

                                       //join J7 in dbContext.groupoccupation on new { a = J4.v_AreaId }
                                       //    equals new { a = J7.v_AreaId } into J7_join
                                       //from J5 in J5_join.DefaultIfEmpty()

                                   where (A.v_ProtocolId == pstrProtocolId) && (A.i_IsDeleted == 0)
                                   select new ProtocolList
                                   {
                                       v_Name = A.v_Name,
                                       v_ProtocolId = A.v_ProtocolId, //id Protocolo
                                       v_Protocol = A.v_Name,// monbre protocolo
                                       v_Organization = B.v_Name + " / " + J.v_Name, // nombre organizacion
                                       v_Location = C.v_Name, // nombre de sede
                                       v_EsoType = E.v_Value1, // Esoa, Esor

                                       v_OrganizationInvoice = F.v_Name, // empresa que factura
                                       v_CostCenter = A.v_CostCenter, // centro de costo
                                       v_IntermediaryOrganization = J3.v_Name + " / " + I.v_Name, // empresa intermediaria

                                       v_MasterServiceName = H.v_Value1, // Eso o no Eo
                                       v_OrganizationId = B.v_OrganizationId,
                                       i_EsoTypeId = E.i_ParameterId, // Id de (Esoa, Esor, Espo)
                                       v_WorkingOrganizationId = J3.v_OrganizationId,
                                       v_OrganizationInvoiceId = F.v_OrganizationId,
                                       v_GroupOccupationId = D.v_GroupOccupationId,
                                       v_Ges = J5.v_Name,
                                       v_GroupOccupation = D.v_Name, // nombre GESO
                                       v_Occupation = J6.v_Name,
                                       i_ServiceTypeId = A.i_MasterServiceTypeId.Value,
                                       i_MasterServiceId = A.i_MasterServiceId.Value,
                                       v_CreationUser = J1.v_UserName,
                                       v_UpdateUser = J2.v_UserName,
                                       d_CreationDate = A.d_InsertDate,
                                       d_UpdateDate = A.d_UpdateDate,
                                       v_ContacName = F.v_ContacName,
                                       v_Address = F.v_Address,
                                       v_CustomerOrganizationId = A.v_CustomerOrganizationId
                                   }).FirstOrDefault();
                ProtocolList objData = objProtocol;

                return objData;
            }

            catch (Exception ex)
            {
                return null;
            }

        }

        public static List<OrganizationCustom> GetEmpresaByProtocoloId(string pstrProtocolId)
        {
            //mon.IsActive = true;
            try
            {
                DatabaseContext dbContext = new DatabaseContext();
                List<OrganizationCustom> objEntity = null;

                objEntity = (from a in dbContext.Protocol
                             join b in dbContext.Organization on a.v_CustomerOrganizationId equals b.v_OrganizationId
                             where a.v_ProtocolId == pstrProtocolId
                             select new OrganizationCustom
                             {
                                 v_Name = b.v_Name
                             }).ToList();


                return objEntity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



    }
}
