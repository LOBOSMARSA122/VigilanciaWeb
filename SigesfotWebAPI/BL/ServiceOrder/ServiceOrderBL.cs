using BE.Common;
using BE.Message;
using BE.Organization;
using BE.Protocol;
using BE.ProtocolComponent;
using BE.ServiceOrder;
using BL.Protocol;
using BL.Service;
using DAL.Organizarion;
using DAL.Protocol;
using DAL.Security;
using DAL.ServiceOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using static BE.Common.Enumeratores;

namespace BL.ServiceOrder
{
    public class ServiceOrderBL
    {
        public static MessageCustom AddServiceOrder(BoardServiceOrder data, int userId, int nodeId)
        {
            MessageCustom msg = new MessageCustom();
            try
            {
                using (var ts = new TransactionScope())
                {

                    #region Entities
                    ServiceOrderBE oServiceOrderBE = new ServiceOrderBE();
                    oServiceOrderBE.v_ServiceOrderId = data.EntityserviceOrder.v_ServiceOrderId;
                    oServiceOrderBE.v_CustomServiceOrderId = data.EntityserviceOrder.v_CustomServiceOrderId;
                    oServiceOrderBE.v_Description = data.EntityserviceOrder.v_Description;
                    oServiceOrderBE.v_Comentary = data.EntityserviceOrder.v_Comentary;
                    oServiceOrderBE.i_NumberOfWorker = data.EntityserviceOrder.i_NumberOfWorker;
                    oServiceOrderBE.r_TotalCost = data.EntityserviceOrder.r_TotalCost;
                    oServiceOrderBE.d_DeliveryDate = data.EntityserviceOrder.d_DeliveryDate;
                    oServiceOrderBE.i_ServiceOrderStatusId = data.EntityserviceOrder.i_ServiceOrderStatusId;
                    oServiceOrderBE.i_LineaCreditoId = data.EntityserviceOrder.i_LineaCreditoId;
                    oServiceOrderBE.i_MostrarPrecio = data.EntityserviceOrder.i_MostrarPrecio;
                    oServiceOrderBE.i_EsProtocoloEspecial = data.EntityserviceOrder.i_EsProtocoloEspecial;


                    List<ServiceOrderDetailBE> ListServiceOrderDetailBE = new List<ServiceOrderDetailBE>();
                    var iter = 0;
                    foreach (var obj in data.ListEntityServiceOrder)
                    {
                        iter++;
                        ProtocolList _prot = new ProtocolList();
                        _prot.v_ProtocolId = obj.v_ProtocolId;

                        bool existName = true;

                        while (existName)
                        {
                            _prot.v_Name = data.EntityserviceOrder.v_OrganizationName + "-" + obj.v_ProtocolTypeName + "-" + iter + "-" + obj.v_GesoName;
                            MessageCustom resultVerify = ProtocolDal.VerifyExistsProtocol(_prot.v_Name);
                            if (resultVerify.Id == null)
                            {
                                existName = false;
                            }
                            else
                            {
                                iter++;
                            }
                        }

                        _prot.v_CustomerOrganizationId = data.EntityserviceOrder.v_OrganizationId + "|" + data.EntityserviceOrder.v_LocationId;
                        _prot.v_IntermediaryOrganization = data.EntityserviceOrder.v_OrganizationId + "|" + data.EntityserviceOrder.v_LocationId;
                        _prot.v_WorkingOrganizationId = data.EntityserviceOrder.v_OrganizationId + "|" + data.EntityserviceOrder.v_LocationId;
                        _prot.v_Geso = obj.v_GroupOccupationId;
                        _prot.i_IsActive = 1;
                        _prot.i_EsoTypeId = obj.i_ProtocolType;
                        _prot.i_ServiceTypeId = 1;//Empresarial
                        _prot.i_MasterServiceId = 2;//Examen de salud ocupacional

                        _prot.ListComponents = obj.ProtocolComponents;

                        var result = ProtocolBL.SaveProtocols(_prot, userId, nodeId);
                        if (result.Error)
                        {
                            throw new Exception(result.Message);
                        }
                        obj.v_ProtocolId = result.Id;

                        ServiceOrderDetailBE oServiceOrderDetailBE = new ServiceOrderDetailBE();
                        oServiceOrderDetailBE.v_ServiceOrderDetailId = obj.v_ServiceOrderDetailId;
                        oServiceOrderDetailBE.v_ServiceOrderId = obj.v_ServiceOrderId;
                        oServiceOrderDetailBE.v_ProtocolId = obj.v_ProtocolId;
                        oServiceOrderDetailBE.r_ProtocolPrice = obj.r_ProtocolPrice;
                        oServiceOrderDetailBE.i_NumberOfWorkerProtocol = obj.i_NumberOfWorkerProtocol;
                        oServiceOrderDetailBE.r_Total = obj.r_Total;

                        ListServiceOrderDetailBE.Add(oServiceOrderDetailBE);
                    }
                    #endregion

                    string ServiceOrderId = ServiceOrderDal.AddServiceOrder(oServiceOrderBE, ListServiceOrderDetailBE, userId, nodeId);

                    msg.Error = false;
                    msg.Id = ServiceOrderId ?? throw new Exception("Sucedió un error al grabar las ordenes, por favor actualice y vuelva a intentar.");
                    msg.Message = "Se guardó correctamente.";
                    msg.Status = (int)HttpStatusCode.Created;

                    ts.Complete();
                    return msg;
                }
            }
            catch (Exception ex)
            {
                msg.Message = ex.Message;
                msg.Error = true;
                msg.Status = (int)HttpStatusCode.Conflict;
                return msg;
            }

        }

        public static MessageCustom GenerateServiceOrderReport(BoardServiceOrder data, int userId, string FechaEmision)
        {
            MessageCustom msg = new MessageCustom();
            try
            {
                
                var MedicalCenter = ServiceBl.GetInfoMedicalCenterSede();
                var pEmpresaCliente = data.EntityserviceOrder.v_OrganizationName;
                //var _DataService = ProtocolBL.GetProtocolById(ProtocolId);
                List<ProtocolComponentCustom> ListaComponentes = new List<ProtocolComponentCustom>();
                List<ServiceOrderPdf> Lista = new List<ServiceOrderPdf>();
                foreach (var objServiceOrder in data.ListEntityServiceOrder)
                {
                    ServiceOrderPdf objSerOrdPdf = new ServiceOrderPdf();

                    var oProtocolo = ProtocolBL.GetProtocolById(objServiceOrder.v_ProtocolId);
                    objSerOrdPdf.v_ServiceOrderId = data.EntityserviceOrder.v_ServiceOrderId;
                    objSerOrdPdf.EmpresaCliente = oProtocolo.v_OrganizationInvoice + " / " + oProtocolo.v_GroupOccupation + " / " + oProtocolo.v_EsoType;
                    var board = ProtocolBL.GetProtocolComponentByProtocolId(objServiceOrder.v_ProtocolId);
                    ListaComponentes = board.ListProtocolComponents;
                    List<ServiceOrderDetailPdf> ListaServiceOrderDetailPdf = new List<ServiceOrderDetailPdf>();
                    foreach (var Componente in ListaComponentes)
                    {
                        ServiceOrderDetailPdf oServiceOrderDetailPdf = new ServiceOrderDetailPdf();
                        oServiceOrderDetailPdf.v_ServiceOrderDetailId = data.ListEntityServiceOrder.Find(p => p.v_ProtocolId == oProtocolo.v_ProtocolId).v_ServiceOrderDetailId;
                        oServiceOrderDetailPdf.v_ServiceOrderId = data.EntityserviceOrder.v_ServiceOrderId;
                        oServiceOrderDetailPdf.v_ComponentId = Componente.ComponentId;
                        oServiceOrderDetailPdf.Componente = Componente.ComponentName;
                        oServiceOrderDetailPdf.v_Precio = Componente.Price;
                        ListaServiceOrderDetailPdf.Add(oServiceOrderDetailPdf);
                    }
                    objSerOrdPdf.DetalleServiceOrder = ListaServiceOrderDetailPdf;
                    objSerOrdPdf.TotalProtocolo = ListaServiceOrderDetailPdf.Sum(s => s.v_Precio);
                    Lista.Add(objSerOrdPdf);
                }

                var oSystemUserList = new SecurityDal().GetSystemUserAndProfesional(userId);

                string ruta = HttpContext.Current.Server.MapPath("~/" + System.Configuration.ConfigurationManager.AppSettings["rutaCotizacion"]);
                string path = ruta + "/" + data.EntityserviceOrder.v_ServiceOrderId + ".pdf";
                if (data.EntityserviceOrder.i_EsProtocoloEspecial == (int)SiNo.Si)
                {
                    OrdenServicioPromocion.CrearOrdenServicio(data.EntityserviceOrder.i_MostrarPrecio == (int)SiNo.Si ? true : false, Lista, MedicalCenter, pEmpresaCliente, DateTime.Parse(FechaEmision).ToString("dd 'd'e MMMM 'd'e yyyy"), oSystemUserList == null ? "" : oSystemUserList.Profesion + ". " + oSystemUserList.v_PersonName, path);

                }
                else
                {
                    OrdenServicio.CrearOrdenServicio(data.EntityserviceOrder.i_MostrarPrecio == (int)SiNo.Si ? true : false, Lista, MedicalCenter, pEmpresaCliente, data.EntityserviceOrder.v_ServiceOrderId, DateTime.Parse(FechaEmision).ToString("dd 'd'e MMMM 'd'e yyyy"), oSystemUserList == null ? "" : oSystemUserList.Profesion  + ". " + oSystemUserList.v_PersonName, path);
                }
                msg.Error = false;
                msg.Id = data.EntityserviceOrder.v_ServiceOrderId + ".pdf";
                msg.Status = (int)HttpStatusCode.Accepted;
                return msg;
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Status = (int)HttpStatusCode.BadRequest;
                msg.Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return msg; 
            }
            
        }

        public static List<ServiceOrderDetailCustom> GetServicesOrderDetail(string serviceOrderId)
        {
            return ServiceOrderDal.GetServicesOrderDetail(serviceOrderId);
        }

        public static MessageCustom DeletedServiceOrderDetail(string serviceOrderDetail, int userId)
        {
            MessageCustom msg = new MessageCustom();
            var ok = ServiceOrderDal.DeletedServiceOrderDetail(serviceOrderDetail, userId);
            if (!ok)
            {
                msg.Error = true;
                msg.Message = "Sucedió un error al eliminar el detalle, por favor vuelva intentar.";
                msg.Status = (int)HttpStatusCode.Conflict;
            }
            else
            {
                msg.Error = false;
                msg.Message = "Se eliminó correctamente.";
                msg.Status = (int)HttpStatusCode.OK;
            }

            return msg;
        }
    }
}
