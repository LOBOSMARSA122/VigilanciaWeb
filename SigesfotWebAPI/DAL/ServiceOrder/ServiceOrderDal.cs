using BE.Common;
using BE.Protocol;
using BE.ServiceOrder;
using BE.Sigesoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BE.Common.Enumeratores;

namespace DAL.ServiceOrder
{
    public class ServiceOrderDal
    {
        private static DatabaseContext ctx = new DatabaseContext();

        public static string AddServiceOrder(ServiceOrderBE pobjDtoEntity, List<ServiceOrderDetailBE> pobjDtoEntityDetail, int userId, int nodeId)
        {
            //mon.IsActive = true;
            string NewId = "(No generado)";
            try
            {
                DatabaseContext dbContext = new DatabaseContext();

                #region Service Order
                if (pobjDtoEntity.v_ServiceOrderId == null) //Add
                {
                    pobjDtoEntity.d_InsertDate = DateTime.Now;
                    pobjDtoEntity.i_InsertUserId = userId;
                    pobjDtoEntity.i_IsDeleted = 0;

                    NewId = new Common.Utils().GetPrimaryKey(nodeId, 101, "YY");
                    pobjDtoEntity.v_ServiceOrderId = NewId;

                    dbContext.ServiceOrder.Add(pobjDtoEntity);
                }
                else //update
                {
                    NewId = pobjDtoEntity.v_ServiceOrderId;
                    var objServiceOrder = ctx.ServiceOrder.Where(x => x.v_ServiceOrderId == pobjDtoEntity.v_ServiceOrderId).FirstOrDefault();
                    objServiceOrder.d_UpdateDate = DateTime.Now;
                    objServiceOrder.i_UpdateUserId = userId;
                    objServiceOrder.i_MostrarPrecio = pobjDtoEntity.i_MostrarPrecio;
                    objServiceOrder.i_LineaCreditoId = pobjDtoEntity.i_LineaCreditoId;
                    objServiceOrder.i_NumberOfWorker = pobjDtoEntity.i_NumberOfWorker;
                    objServiceOrder.i_ServiceOrderStatusId = pobjDtoEntity.i_ServiceOrderStatusId;
                    objServiceOrder.r_TotalCost = pobjDtoEntity.r_TotalCost;
                }
                
                dbContext.SaveChanges();
                #endregion

                #region Service Order Detail

                if (pobjDtoEntityDetail != null)
                {
                    foreach (var item in pobjDtoEntityDetail)
                    {
                        if (item.v_ServiceOrderDetailId == null) //add
                        {
                            item.d_InsertDate = DateTime.Now;
                            item.i_InsertUserId = userId;
                            item.i_IsDeleted = 0;
                            // Autogeneramos el Pk de la tabla
                            item.v_ServiceOrderId = NewId;
                            item.v_ServiceOrderDetailId = new Common.Utils().GetPrimaryKey(nodeId, 102, "YA");

                            dbContext.ServiceOrderDetail.Add(item);
                            dbContext.SaveChanges();
                        }
                        else //Update
                        {

                            var objServiceOrderDetail = dbContext.ServiceOrderDetail.Where(x => x.v_ServiceOrderDetailId == item.v_ServiceOrderDetailId).FirstOrDefault();
                            objServiceOrderDetail.d_UpdateDate = DateTime.Now;
                            objServiceOrderDetail.i_UpdateUserId = userId;
                            objServiceOrderDetail.v_ProtocolId = item.v_ProtocolId;
                            objServiceOrderDetail.r_Total = item.r_Total;
                            objServiceOrderDetail.r_ProtocolPrice = item.r_ProtocolPrice;
                            objServiceOrderDetail.i_NumberOfWorkerProtocol = item.i_NumberOfWorkerProtocol;                           
                        }                      
                    }
                    dbContext.SaveChanges();
                }

                #endregion

                return NewId;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<ServiceOrderDetailCustom> GetServicesOrderDetail(string serviceOrderId) {
            try
            {

                var ListSerOrdDet = (from sod in ctx.ServiceOrderDetail
                                     join pro in ctx.Protocol on sod.v_ProtocolId equals pro.v_ProtocolId
                                     join sys in ctx.SystemParameter on new { a = pro.i_EsoTypeId.Value, b = 118 } equals new { a = sys.i_ParameterId, b = sys.i_GroupId } into sys_join
                                     from sys in sys_join.DefaultIfEmpty()
                                     join grou in ctx.GroupOccupation on pro.v_GroupOccupationId equals grou.v_GroupOccupationId
                                     where sod.v_ServiceOrderId == serviceOrderId && sod.i_IsDeleted == 0 && pro.i_IsDeleted == 0
                                     select new ServiceOrderDetailCustom
                                     {
                                         
                                         v_ServiceOrderDetailId = sod.v_ServiceOrderDetailId,
                                         v_ProtocolId = sod.v_ProtocolId,
                                         i_NumberOfWorkerProtocol = sod.i_NumberOfWorkerProtocol,
                                         r_ProtocolPrice = sod.r_ProtocolPrice,
                                         r_Total = sod.r_Total,
                                         v_ProtocolName = pro.v_Name,
                                         v_ProtocolTypeName = sys.v_Value1,
                                         i_ProtocolType = pro.i_EsoTypeId,
                                         v_GroupOccupationId = pro.v_GroupOccupationId,
                                         v_GesoName = grou.v_Name,
                                         
                                     }).ToList();

                foreach (var obj in ListSerOrdDet)
                {
                    var listComponents = (from prot in ctx.ProtocolComponent
                                          join com in ctx.Component on prot.v_ComponentId equals com.v_ComponentId
                                          join sys in ctx.SystemParameter on new { a = prot.i_OperatorId.Value, b = 117 } equals new { a = sys.i_ParameterId, b = sys.i_GroupId } into sys_join from sys in sys_join.DefaultIfEmpty()
                                          join sys2 in ctx.SystemParameter on new { a = prot.i_GenderId.Value, b = 130 } equals new { a = sys2.i_ParameterId, b = sys2.i_GroupId } into sys2_join
                                          from sys2 in sys2_join.DefaultIfEmpty()
                                          join sys3 in ctx.SystemParameter on new { a = prot.i_IsConditionalId.Value, b = 111 } equals new { a = sys3.i_ParameterId, b = sys3.i_GroupId } into sys3_join
                                          from sys3 in sys3_join.DefaultIfEmpty()
                                          join sys4 in ctx.SystemParameter on new { a = com.i_ComponentTypeId.Value, b = 126 }  // Tipo Examen
                                                                  equals new { a = sys4.i_ParameterId, b = sys4.i_GroupId } into sys4_join
                                          from sys4 in sys4_join.DefaultIfEmpty()
                                          where prot.v_ProtocolId == obj.v_ProtocolId && prot.i_IsDeleted == 0
                                          select new ComponentCustom
                                          {
                                              v_ProtocolComponentId = prot.v_ProtocolComponentId,
                                              v_ComponentId = prot.v_ComponentId,
                                              v_Name = com.v_Name,
                                              r_Price = prot.r_Price,
                                              v_OperadorName = sys.v_Value1,
                                              i_Edad = prot.i_Age.Value,
                                              v_GenderName = sys2.v_Value1,
                                              v_IsConditional = sys3.v_Value1,
                                              v_ComponentTypeName = sys4.v_Value1,
                                              i_OperadorId = prot.i_OperatorId == null ? -1 : prot.i_OperatorId.Value,
                                              i_GenderId = prot.i_GenderId.Value,
                                              i_IsConditional = prot.i_IsConditionalId.Value,
                                              i_ComponentTypeId = com.i_ComponentTypeId.Value,
                                              i_GrupoEtario = prot.i_GrupoEtarioId.Value,
                                              i_IsIMC = prot.i_IsConditionalIMC.Value,
                                              IMC = prot.r_Imc.Value,
                                              i_IsAditional = prot.i_IsAdditional.Value,
                                              RecordStatus = 1,
                                              RecordType = 2,
                                          }).ToList().GroupBy(x => x.v_ProtocolComponentId).Select(z => z.First()).ToList();

                    obj.ProtocolComponents = listComponents;


                }

                return ListSerOrdDet;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool DeletedServiceOrderDetail(string serviceOrderDetail, int userId)
        {
            try
            {

                var objServiceDetail = ctx.ServiceOrderDetail.Where(x => x.v_ServiceOrderDetailId == serviceOrderDetail).FirstOrDefault();

                objServiceDetail.i_IsDeleted = (int)SiNo.Si;
                objServiceDetail.d_UpdateDate = DateTime.Now;
                objServiceDetail.i_UpdateUserId = userId;

                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static ServiceOrderCustom GetOrganizationByServiceOrderId(string serviceOrderId) {
            try
            {
                var objOrganization = (from org in ctx.Organization
                                       join pro in ctx.Protocol on org.v_OrganizationId equals pro.v_CustomerOrganizationId
                                       join sord in ctx.ServiceOrderDetail on pro.v_ProtocolId equals sord.v_ProtocolId
                                       join sor in ctx.ServiceOrder on serviceOrderId equals sor.v_ServiceOrderId

                                       join su in ctx.SystemUser on sor.i_InsertUserId.Value equals su.i_SystemUserId into su_join
                                       from su in su_join.DefaultIfEmpty()

                                       join pr in ctx.Professional on su.v_PersonId equals pr.v_PersonId into pr_join
                                       from pr in pr_join.DefaultIfEmpty()
                                       where sord.v_ServiceOrderId == serviceOrderId
                                       select new ServiceOrderCustom
                                       {
                                           v_OrganizationId = org.v_OrganizationId,
                                           v_OrganizationName = org.v_Name,
                                           v_OrganizationAdress = org.v_Address,
                                           v_ContacName = org.v_ContacName,
                                           d_InsertDate = sor.d_InsertDate,
                                           v_IdentificationNumber = org.v_IdentificationNumber,
                                           v_CustomServiceOrderId = sor.v_CustomServiceOrderId,
                                           b_SignatureImage = pr.b_SignatureImage,
                                       }).FirstOrDefault();

                return objOrganization;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
