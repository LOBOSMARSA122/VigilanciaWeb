using BE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using BE.Service;
using System.Transactions;
using DAL.Hospitalizacion;
using BE.Message;
using static BE.Common.Enumeratores;

namespace DAL.Service
{
    public class ServiceDal
    {
        private static DatabaseContext ctx = new DatabaseContext();

        public string AddService(ServiceDto oServiceDto, int nodeId, int systemUserId)
        {
            var serviceId = new Common.Utils().GetPrimaryKey(nodeId, 23, "SR");
            
            oServiceDto.v_ServiceId = serviceId;
            oServiceDto.i_IsDeleted = (int) Enumeratores.SiNo.No;
            oServiceDto.d_InsertDate = DateTime.UtcNow;
            oServiceDto.i_InsertUserId = systemUserId;

            ctx.Service.Add(oServiceDto);
            ctx.SaveChanges();
            
            return oServiceDto.v_ServiceId;
        }

        public string DarDeBaja(string personId)
        {
            var services = (from a in ctx.Service where a.v_PersonId == personId && a.i_IsDeleted == 0 select a).ToList();
            
            foreach (var service in services)
            {
                service.i_StatusVigilanciaId = 2;
            }

            ctx.SaveChanges();
            return personId;
        }

        public Enumeratores.ServiceStatus GetServiceStatus(string serviceId)
        {
            using (var contex = new DatabaseContext())
            {
                var serviceStatusId = (from a in contex.Service where a.v_ServiceId == serviceId select a).FirstOrDefault().i_ServiceStatusId;

                if (serviceStatusId == (int)Enumeratores.ServiceStatus.Culminado)
                {
                    return Enumeratores.ServiceStatus.Culminado;
                }
                else if (serviceStatusId == (int)Enumeratores.ServiceStatus.Incompleto)
                {
                    return Enumeratores.ServiceStatus.Incompleto;
                }
                else if (serviceStatusId == (int)Enumeratores.ServiceStatus.Iniciado || serviceStatusId == (int)Enumeratores.ServiceStatus.PorIniciar)
                {
                    return Enumeratores.ServiceStatus.Iniciado;
                }
                else 
                {
                    return Enumeratores.ServiceStatus.Cancelado;
                }
            }
        }

        public string CreateService(ServiceCustom data, int nodeId, int userId)
        {
            try
            {
                var newId = new Common.Utils().GetPrimaryKey(nodeId, 23, "SR");
                ServiceDto oServiceDto = new ServiceDto();
                oServiceDto.v_ServiceId = newId;
                oServiceDto.v_ProtocolId = data.ProtocolId;
                oServiceDto.v_PersonId = data.PersonId;
                oServiceDto.i_MasterServiceId = data.MasterServiceId;
                oServiceDto.i_ServiceStatusId = data.ServiceStatusId;
                oServiceDto.i_AptitudeStatusId = data.AptitudeStatusId;
                oServiceDto.d_ServiceDate = DateTime.Now;
                oServiceDto.d_GlobalExpirationDate = null;
                oServiceDto.d_ObsExpirationDate = null;
                oServiceDto.v_OrganizationId = data.OrganizationId;
                oServiceDto.i_FlagAgentId = data.FlagAgentId;
                oServiceDto.v_Motive = data.Motive;
                oServiceDto.i_IsFac = 1;
                oServiceDto.i_StatusLiquidation = 1;
                oServiceDto.i_IsFacMedico = 0;
                oServiceDto.i_IsDeleted = 0;
                oServiceDto.v_centrocosto = data.CentroCosto;
                oServiceDto.i_MedicoPagado = 0;
                oServiceDto.i_InsertUserId = userId;
                oServiceDto.d_InsertDate = DateTime.Now;

                ctx.Service.Add(oServiceDto);
                ctx.SaveChanges();
                return newId;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ServiceDto GetServiceByServiceId(string ServiceId)
        {
            return ctx.Service.Where(x => x.v_ServiceId == ServiceId).FirstOrDefault();
        }

        public bool UpdateServiceForProtocolo(ServiceCustom data, int userId)
        {
            try
            {
                var objService = ctx.Service.Where(x => x.v_ServiceId == data.ServiceId).FirstOrDefault();
                objService.v_ProtocolId = data.ProtocolId;
                objService.v_centrocosto = data.CentroCosto;
                objService.i_MasterServiceId = data.MasterServiceId;

                objService.d_UpdateDate = DateTime.Now;
                objService.i_UpdateUserId = userId;

                return ctx.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RegistrarCarta(MultiDataModel data)
        {
            try
            {
                var objservice = ctx.Service.Where(x => x.v_ServiceId == data.String1).FirstOrDefault();
                objservice.v_NroCartaSolicitud = data.String2;
                objservice.d_UpdateDate = DateTime.Now;
                objservice.i_UpdateUserId = data.Int1;

                return ctx.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public MessageCustom FusionarServicios(List<string> ServicesId, int userId, int nodeId)
        {
            MessageCustom _MessageCustom = new MessageCustom();
            try
            {
                List<HospitalizacionServiceBE> ListHospServices = new List<HospitalizacionServiceBE>();
                List<HospitalizacionServiceBE> ListHospServicesDistintos = new List<HospitalizacionServiceBE>();
                List<string> ServicesNoEncontrados = new List<string>();
                using (var ts = new TransactionScope())
                {
                    #region FindListHospServices
                    foreach (var serviceId in ServicesId)
                    {
                        var query = (from hser in ctx.HospitalizacionService
                                     where hser.v_ServiceId == serviceId && hser.i_IsDeleted == 0
                                     select hser).FirstOrDefault();

                        if (query != null)
                        {
                            ListHospServices.Add(query);
                        }
                        else
                        {
                            ServicesNoEncontrados.Add(serviceId);
                        }

                    }
                    #endregion

                    string HospitalizacionId = "";
                    var objHospitlizacion = ListHospServices.FindAll(x => x.v_HopitalizacionId != null).FirstOrDefault();
                    if (objHospitlizacion != null)
                    {
                        HospitalizacionId = objHospitlizacion.v_HopitalizacionId;
                    }
                    if (HospitalizacionId != "" && HospitalizacionId != null)
                    {
                        //Actualizo la HospitalizacionService con los mismos HospitalizacionId
                        foreach (var HospService in ListHospServices)
                        {
                            if (HospitalizacionId != HospService.v_HopitalizacionId)
                            {
                                var query = (from hser in ctx.HospitalizacionService
                                             where hser.v_HospitalizacionServiceId == HospService.v_HospitalizacionServiceId
                                             select hser).FirstOrDefault();
                                query.v_HopitalizacionId = HospitalizacionId;
                                query.d_UpdateDate = DateTime.Now;
                                query.i_UpdateUserId = userId;
                                ctx.SaveChanges();
                            }

                        }
                    }
                    else
                    {
                        if (ListHospServices.Count > 0)
                        {
                            HospitalizacionId = new HospitalizacionDal().AddHospitalizacion(ListHospServices[0].v_ServiceId, nodeId, userId);
                            foreach (var HospService in ListHospServices)
                            {
                                var query = (from hser in ctx.HospitalizacionService
                                             where hser.v_HospitalizacionServiceId == HospService.v_HospitalizacionServiceId
                                             select hser).FirstOrDefault();
                                query.v_HopitalizacionId = HospitalizacionId;
                                query.d_UpdateDate = DateTime.Now;
                                query.i_UpdateUserId = userId;
                                ctx.SaveChanges();
                            }
                        }

                    }

                    if (ServicesNoEncontrados.Count > 0)
                    {
                        //Agrego una nueva HospitalizacionService
                        if (HospitalizacionId != "" && HospitalizacionId != null)
                        {
                            foreach (var _serviceId in ServicesNoEncontrados)
                            {
                                bool result = new HospitalizacionDal().AddHospitalizacionService(HospitalizacionId, _serviceId, nodeId, userId);
                                if (!result) throw new Exception("Sucedió un error al generar las nuevas hospitalizaciones services");
                            }
                        }
                        else //Agrego una nueva Hospitalizacion
                        {
                            string _HospitalizacionId = new HospitalizacionDal().AddHospitalizacion(ServicesNoEncontrados[0], nodeId, userId);
                            foreach (var serviceId in ServicesNoEncontrados)
                            {
                                if (_HospitalizacionId != null)
                                {
                                    //Agrego la hospitalizacionService
                                    bool result = new HospitalizacionDal().AddHospitalizacionService(_HospitalizacionId, serviceId, nodeId, userId);
                                    if (!result) throw new Exception("Sucedió un error al generar las nuevas hospitalizaciones services");
                                }
                                else
                                {
                                    throw new Exception("Sucedió un error al generar las nuevas hospitalizaciones");
                                }
                            }
                        }
                    }
                    ts.Complete();                  
                }
                
                _MessageCustom.Error = false;
                _MessageCustom.Status = (int)StatusHttp.Ok;
                _MessageCustom.Message = "Los servicios se fusionaron correctamente";
                return _MessageCustom;
            }
            catch (Exception ex)
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = (int)StatusHttp.BadRequest;
                _MessageCustom.Message = ex.Message;
                return _MessageCustom;
            }
        }
    }
}
