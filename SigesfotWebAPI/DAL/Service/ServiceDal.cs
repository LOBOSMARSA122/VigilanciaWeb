using BE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using BE.Service;
using System.Transactions;
using DAL.Hospitalizacion;
using BE.Message;
using static BE.Common.Enumeratores;
using BE.Pacient;
using BE.RoleNodeComponentProfile;
using BE.Sigesoft;
using BE.Piso;
using BE.Categoria;
using BE.Component;
using BE.Receta;
using DAL.z_ProductsSAMBHS;
using BE.Organization;

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

        public List<SaldoPaciente> GetListSaldosPaciente(string serviceId)
        {
            try
            {
                var list = (from ser in ctx.Service
                            join src in ctx.ServiceComponent on ser.v_ServiceId equals src.v_ServiceId
                            join com in ctx.Component on src.v_ComponentId equals com.v_ComponentId
                            where ser.v_ServiceId == serviceId && src.d_SaldoPaciente > 0
                            select new SaldoPaciente
                            {
                                v_Name = com.v_Name,
                                d_SaldoPaciente = src.d_SaldoPaciente,
                            }).ToList();

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<RoleNodeComponentProfileCustom> GetRoleNodeComponentProfileByRoleNodeId(int pintNodeId, int pintRoleId)
        {
            //mon.IsActive = true;

            try
            {
                DatabaseContext ctx = new DatabaseContext();

                var query = (from a in ctx.RoleNodeComponentProfile
                             where (a.i_NodeId == pintNodeId) &&
                                   (a.i_RoleId == pintRoleId) &&
                                   (a.i_IsDeleted == (int)SiNo.No)
                             select new RoleNodeComponentProfileCustom
                             {
                                 v_ComponentId = a.v_ComponentId,
                                 v_RoleNodeComponentId = a.v_RoleNodeComponentId,
                                 i_Read = a.i_Read,
                             }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw;

            }

        }

        public List<ServiceComponentList> GetServiceComponentsCulminados(string pstrServiceId)
        {
            //mon.IsActive = true;
            try
            {

                var query = (from A in ctx.ServiceComponent
                             join B in ctx.SystemParameter on new { a = A.i_ServiceComponentStatusId.Value, b = 127 }
                                    equals new { a = B.i_ParameterId, b = B.i_GroupId }
                             join C in ctx.Component on A.v_ComponentId equals C.v_ComponentId
                             where (A.v_ServiceId == pstrServiceId) &&
                                   (A.i_IsDeleted == 0) &&
                                   (A.i_IsRequiredId == (int?)SiNo.Si) &&
                                   (A.i_ServiceComponentStatusId != (int)ServiceComponentStatus.Evaluado)

                             select new ServiceComponentList
                             {
                                 v_ComponentId = A.v_ComponentId,
                                 v_ComponentName = C.v_Name,
                                 i_ServiceComponentStatusId = A.i_ServiceComponentStatusId.Value,
                                 v_ServiceComponentStatusName = B.v_Value1,
                                 i_CategoryId = C.i_CategoryId
                             }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ServiceComponentList> GetServiceComponents(string pstrServiceId)
        {


            int isDeleted = (int)SiNo.No;
            int isRequired = (int)SiNo.Si;

            try
            {

                var query = (from A in ctx.ServiceComponent
                             join B in ctx.SystemParameter on new { a = A.i_ServiceComponentStatusId.Value, b = 127 }
                                      equals new { a = B.i_ParameterId, b = B.i_GroupId }
                             join C in ctx.Component on A.v_ComponentId equals C.v_ComponentId
                             join D in ctx.SystemParameter on new { a = A.i_QueueStatusId.Value, b = 128 }
                                      equals new { a = D.i_ParameterId, b = D.i_GroupId }
                             join E in ctx.Service on A.v_ServiceId equals E.v_ServiceId
                             join F in ctx.SystemParameter on new { a = C.i_CategoryId, b = 116 }
                                      equals new { a = F.i_ParameterId, b = F.i_GroupId } into F_join
                             from F in F_join.DefaultIfEmpty()

                             where A.v_ServiceId == pstrServiceId &&
                                   A.i_IsDeleted == isDeleted &&
                                   A.i_IsRequiredId == isRequired

                             select new ServiceComponentList
                             {
                                 v_ComponentId = A.v_ComponentId,
                                 v_ComponentName = C.v_Name,
                                 i_ServiceComponentStatusId = A.i_ServiceComponentStatusId.Value,
                                 v_ServiceComponentStatusName = B.v_Value1,
                                 d_StartDate = A.d_StartDate.Value,
                                 d_EndDate = A.d_EndDate.Value,
                                 i_QueueStatusId = A.i_QueueStatusId.Value,
                                 v_QueueStatusName = D.v_Value1,
                                 ServiceStatusId = E.i_ServiceStatusId.Value,
                                 v_Motive = E.v_Motive,
                                 i_CategoryId = C.i_CategoryId,
                                 v_CategoryName = C.i_CategoryId == -1 ? C.v_Name : F.v_Value1,
                                 v_ServiceId = E.v_ServiceId,
                                 v_ServiceComponentId = A.v_ServiceComponentId,
                             });

                var objData = query.AsEnumerable()
                             .Where(s => s.i_CategoryId != -1)
                             .GroupBy(x => x.i_CategoryId)
                             .Select(group => group.First());

                List<ServiceComponentList> obj = objData.ToList();

                obj.AddRange(query.Where(p => p.i_CategoryId == -1));

                return obj;
            }
            catch (Exception ex)
            { 
                return null;
            }
        }

        public bool PermitirLlamar(string pstrServiceId, int pintPiso)
        {
            //mon.IsActive = true;


            try
            {
                bool Respuesta = true;

                var query = (from s in ctx.ServiceComponent
                             join c in ctx.Component on s.v_ComponentId equals c.v_ComponentId
                             join P in ctx.SystemParameter on new { a = 116, b = c.i_CategoryId }
                                   equals new { a = P.i_GroupId, b = P.i_ParameterId } //into P_join
                                                                                       //from P in P_join.DefaultIfEmpty()

                             join P1 in ctx.SystemParameter on new { a = 127, b = s.i_ServiceComponentStatusId.Value }
                            equals new { a = P1.i_GroupId, b = P1.i_ParameterId } //into P1_join
                                                                                  //from P1 in P1_join.DefaultIfEmpty()

                             where s.v_ServiceId == pstrServiceId
                             select new PisoCustom
                             {
                                 v_Categoria = P.v_Value1,
                                 ValorPiso = P.v_Value2,
                                 i_CategoriaId = c.i_CategoryId,
                                 i_EstadoComponente = s.i_ServiceComponentStatusId.Value,
                                 v_EstadoComponente = P1.v_Value1
                             });


                var objData = query.AsEnumerable()
                           .GroupBy(x => x.i_CategoriaId)
                           .Select(group => group.First())
                           .OrderBy(o => o.ValorPiso);


                foreach (var item in objData)
                {
                    if (int.Parse(item.ValorPiso.ToString()) < pintPiso && item.i_EstadoComponente != (int)ServiceComponentStatus.Evaluado)
                    {
                        Respuesta = false;
                    }
                }

                return Respuesta;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<ServiceComponentList> GetServiceComponentByCategoryId(int pstrCategoryId, string pstrServiceId)
        {
            //mon.IsActive = true;

            try
            {
                var objEntity = (from a in ctx.ServiceComponent
                                 join b in ctx.Component on a.v_ComponentId equals b.v_ComponentId
                                 where b.i_CategoryId == pstrCategoryId && a.v_ServiceId == pstrServiceId && a.i_IsRequiredId == (int)SiNo.Si
                                 select new ServiceComponentList
                                 {
                                     v_ServiceComponentId = a.v_ServiceComponentId,
                                     v_ServiceId = a.v_ServiceId,
                                     v_ComponentId = a.v_ComponentId,
                                     v_ComponentName = b.v_Name,
                                     i_ServiceComponentStatusId = a.i_ServiceComponentStatusId
                                 }).ToList();

                List<ServiceComponentList> objDataList = objEntity.ToList();
                return objDataList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool UpdateAdditionalExam(List<ServiceComponentList> pobjDtoEntity, string serviceId, int? isRequiredId, int userId)
        {
            //mon.IsActive = true;

            try
            {

                var serviceComponentId = pobjDtoEntity.Select(p => p.v_ServiceComponentId).ToArray();

                // Obtener la entidad fuente
                var objEntitySource = (from sc in ctx.ServiceComponent
                                       where sc.v_ServiceId == serviceId && serviceComponentId.Contains(sc.v_ServiceComponentId)
                                       select sc).ToList();


                foreach (var item in objEntitySource)
                {
                    item.d_UpdateDate = DateTime.Now;
                    item.i_UpdateUserId = userId;
                    item.i_IsRequiredId = isRequiredId;
                }

                // Guardar los cambios
                

                return ctx.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Categoria> GetAllComponents(int? filterType, string name)
        {

            int isDeleted = (int)SiNo.No;
            string codigoSegus = "";
            string nameCategory = "";
            string nameComponent = "";
            string nameSubCategory = "";
            string componentId = "";
            if (filterType == (int)TipoBusqueda.CodigoSegus)
            {
                codigoSegus = name;

            }
            else if (filterType == (int)TipoBusqueda.NombreCategoria)
            {
                nameCategory = name;
            }
            else if (filterType == (int)TipoBusqueda.NombreComponent)
            {
                nameComponent = name;
            }
            else if (filterType == (int)TipoBusqueda.NombreSubCategoria)
            {
                nameSubCategory = name;
            }
            else if (filterType == (int)TipoBusqueda.ComponentId)
            {
                componentId = name;
            }


            try
            {
                System.Linq.IQueryable<Categoria> query;
                if (name == "")
                {
                    query = (from C in ctx.Component
                             join F in ctx.SystemParameter on new { a = C.i_CategoryId, b = 116 }
                                 equals new { a = F.i_ParameterId, b = F.i_GroupId } into F_join
                             from F in F_join.DefaultIfEmpty()

                             where C.i_IsDeleted == 0
                             select new Categoria
                             {
                                 v_ComponentId = C.v_ComponentId,
                                 v_ComponentName = C.v_Name,

                                 v_CodigoSegus = C.v_CodigoSegus,
                                 i_CategoryId = C.i_CategoryId,
                                 v_CategoryName = C.i_CategoryId == -1 ? C.v_Name : F.v_Value1,

                             });

                }
                else if (filterType == (int)TipoBusqueda.ComponentId)
                {
                    query = (from C in ctx.Component
                             join F in ctx.SystemParameter on new { a = C.i_CategoryId, b = 116 }
                                 equals new { a = F.i_ParameterId, b = F.i_GroupId } into F_join

                             from F in F_join.DefaultIfEmpty()

                             where C.i_IsDeleted == 0 && C.v_ComponentId == componentId
                             select new Categoria
                             {
                                 v_ComponentId = C.v_ComponentId,
                                 v_ComponentName = C.v_Name,
                                 v_CodigoSegus = C.v_CodigoSegus,
                                 i_CategoryId = C.i_CategoryId,
                                 v_CategoryName = C.i_CategoryId == -1 ? C.v_Name : F.v_Value1,

                             });
                    var query2 = query
                        .GroupBy(x => new { x.v_CodigoSegus, x.v_ComponentName, x.v_ComponentId, x.i_CategoryId, x.v_CategoryName })
                        .Select(g => new { g.Key.v_CodigoSegus, g.Key.v_ComponentName, g.Key.v_ComponentId, g.Key.i_CategoryId, g.Key.v_CategoryName });

                }
                else
                {
                    query = (from C in ctx.Component
                             join F in ctx.SystemParameter on new { a = C.i_CategoryId, b = 116 }
                                 equals new { a = F.i_ParameterId, b = F.i_GroupId } into F_join
                             from F in F_join.DefaultIfEmpty()

                             join G in ctx.SystemParameter on new { a = F.i_ParameterId, b = 116 }
                                 equals new { a = G.i_ParentParameterId.Value, b = G.i_GroupId } into G_join
                             from G in G_join.DefaultIfEmpty()

                             where C.i_IsDeleted == 0 && (G.v_Value1.Contains(nameSubCategory) && C.v_Name.Contains(nameComponent) && F.v_Value1.Contains(nameCategory) && C.v_CodigoSegus.Contains(codigoSegus))
                             select new Categoria
                             {
                                 v_ComponentId = C.v_ComponentId,
                                 v_ComponentName = C.v_Name,
                                 v_CodigoSegus = C.v_CodigoSegus,
                                 i_CategoryId = C.i_CategoryId,
                                 v_CategoryName = C.i_CategoryId == -1 ? C.v_Name : F.v_Value1,

                             });
                    var query2 = query
                        .GroupBy(x => new
                        { x.v_CodigoSegus, x.v_ComponentName, x.v_ComponentId, x.i_CategoryId, x.v_CategoryName })
                        .Select(g => new { g.Key.v_CodigoSegus, g.Key.v_ComponentName, g.Key.v_ComponentId, g.Key.i_CategoryId, g.Key.v_CategoryName });

                }


                var objData = query.AsEnumerable()
                    .Where(s => s.i_CategoryId != -1)
                    .GroupBy(x => x.i_CategoryId)
                    .Select(group => group.First());

                List<Categoria> obj = objData.ToList();

                Categoria objCategoriaList;
                List<Categoria> Lista = new List<Categoria>();

                //int CategoriaId_Old = 0;
                for (int i = 0; i < obj.Count(); i++)
                {
                    objCategoriaList = new Categoria();

                    objCategoriaList.i_CategoryId = obj[i].i_CategoryId.Value;
                    objCategoriaList.v_CategoryName = obj[i].v_CategoryName;

                    var x = query.ToList().FindAll(p => p.i_CategoryId == obj[i].i_CategoryId.Value);

                    x.Sort((z, y) => z.v_ComponentName.CompareTo(y.v_ComponentName));
                    ComponentDetailList objComponentDetailList;
                    List<ComponentDetailList> ListaComponentes = new List<ComponentDetailList>();
                    foreach (var item in x)
                    {
                        objComponentDetailList = new ComponentDetailList();
                        objComponentDetailList.v_ComponentId = item.v_ComponentId;
                        objComponentDetailList.v_ComponentName = item.v_ComponentName;
                        //objComponentDetailList.v_ServiceComponentId = item.v_ServiceComponentId;
                        var list = ListaComponentes.Find(z => z.v_ComponentId == item.v_ComponentId);
                        if (list == null)
                        {
                            ListaComponentes.Add(objComponentDetailList);
                        }

                    }

                    objCategoriaList.Componentes = ListaComponentes;

                    Lista.Add(objCategoriaList);

                }
                return Lista;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool AddAdditionalExam(List<AdditionalExamCustom> listAdditionalExam, int userId, int nodeId)
        {
            try
            {
                foreach (var exam in listAdditionalExam)
                {
                    var addtionalExamId = new Common.Utils().GetPrimaryKey(nodeId, 49, "AE");

                    AdditionalExamBE objAdditionalExam = new AdditionalExamBE();
                    objAdditionalExam.v_AdditionalExamId = addtionalExamId;
                    objAdditionalExam.v_ServiceId = exam.ServiceId;
                    objAdditionalExam.v_PersonId = exam.PersonId;
                    objAdditionalExam.v_ProtocolId = exam.ProtocolId;
                    objAdditionalExam.v_Commentary = exam.Commentary;
                    objAdditionalExam.v_ComponentId = exam.ComponentId;
                    objAdditionalExam.i_IsNewService = exam.IsNewService;
                    objAdditionalExam.i_IsProcessed = exam.IsProcessed;
                    objAdditionalExam.i_IsDeleted = (int)SiNo.No;
                    objAdditionalExam.d_InsertDate = DateTime.Now;
                    objAdditionalExam.i_InsertUserId = userId;

                    ctx.AdditionalExam.Add(objAdditionalExam);
                }

                return ctx.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public bool LiberarPaciente(List<string> list)
        {
            try
            {
                foreach (var servicecomponentId in list)
                {
                    // Obtener la entidad fuente

                    var objEntitySource = ctx.ServiceComponent.SingleOrDefault(p => p.v_ServiceComponentId == servicecomponentId);
                    objEntitySource.i_QueueStatusId = (int)QueueStatusId.Libre;
                    objEntitySource.i_Iscalling = (int)SiNo.No;
                    objEntitySource.i_Iscalling_1 = (int)SiNo.No;
                    objEntitySource.d_EndDate = DateTime.Now;

                }

                // Guardar los cambios
                ctx.SaveChanges();


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateServiceComponentOfficeLlamando(List<string> servicescomponent, string oficina)
        {
            //mon.IsActive = true;

            try
            {
                foreach (var id in servicescomponent)
                {
                    // Obtener la entidad fuente
                    var objEntitySource = (from a in ctx.ServiceComponent
                                           where a.v_ServiceComponentId == id
                                           select a).FirstOrDefault();

                    // Crear la entidad con los datos actualizados
                    objEntitySource.v_NameOfice = oficina;
                    objEntitySource.i_QueueStatusId = (int)QueueStatusId.llamando;
                    ctx.SaveChanges();
                }
                // Guardar los cambios
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public UsuarioGrabo DevolverDatosUsuarioFirma(int systemuserId)
        {
            var objEntity = (from me in ctx.SystemUser

                             join pme in ctx.Professional on me.v_PersonId equals pme.v_PersonId into pme_join
                             from pme in pme_join.DefaultIfEmpty()

                             join B in ctx.Person on pme.v_PersonId equals B.v_PersonId

                             where me.i_SystemUserId == systemuserId
                             select new UsuarioGrabo
                             {
                                 Firma = pme.b_SignatureImage,
                                 Nombre = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                 CMP = pme.v_ProfessionalCode
                             }).FirstOrDefault();

            return objEntity;

        }

        public bool LiberarPacientelaboratorio(List<string> list, int i_ServiceComponentStatusId_Antiguo)
        {
            try
            {
                int status = i_ServiceComponentStatusId_Antiguo == 1 ? (int)ServiceComponentStatus.PorAprobacion : i_ServiceComponentStatusId_Antiguo;
                foreach (var servicecomponentId in list)
                {
                    // Obtener la entidad fuente

                    var objEntitySource = ctx.ServiceComponent.SingleOrDefault(p => p.v_ServiceComponentId == servicecomponentId);
                    objEntitySource.i_QueueStatusId = (int)QueueStatusId.Libre;
                    objEntitySource.i_Iscalling = (int)SiNo.No;
                    objEntitySource.i_Iscalling_1 = (int)SiNo.No;
                    objEntitySource.d_EndDate = DateTime.Now;
                    objEntitySource.i_ServiceComponentStatusId = status;

                }
                // Guardar los cambios
                ctx.SaveChanges();


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public OrganizationDto GetInfoMedicalCenter()
        {
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                OrganizationDto objDtoEntity = null;
                var objEntity = (from o in dbContext.Organization
                                 where o.v_OrganizationId == Constants.OWNER_ORGNIZATION_ID
                                 select new OrganizationDto
                                 {
                                     v_OrganizationId = o.v_OrganizationId,
                                     b_Image = o.b_Image,
                                     v_Name = o.v_Name,
                                     v_Address = o.v_Address,

                                 }).SingleOrDefault();

                var other = (from o in dbContext.Location
                             where o.v_OrganizationId == Constants.OWNER_ORGNIZATION_ID
                             select new OrganizationDto
                             {
                                 v_OrganizationId = o.v_OrganizationId,
                                 v_Name = o.v_Name,
                             }).SingleOrDefault();
                objEntity.v_SectorName = other == null ? "" : other.v_Name;

                if (objEntity != null)
                    objDtoEntity = objEntity;

                return objDtoEntity;
            }
        }

        public static OrganizationCustom GetInfoMedicalCenterSede()
        {
            using (DatabaseContext dbContext = new DatabaseContext())
            {

                var sql = (from o in dbContext.Organization
                           join s in dbContext.Location on o.v_OrganizationId equals s.v_OrganizationId
                           where o.v_OrganizationId == Constants.OWNER_ORGNIZATION_ID
                           select new OrganizationCustom
                           {
                               v_Name = o.v_Name,
                               v_Address = o.v_Address,
                               b_Image = o.b_Image,
                               v_PhoneNumber = o.v_PhoneNumber,
                               v_Mail = o.v_Mail,
                               v_Sede = s.v_Name,
                               v_EmailContacto = o.v_EmailContacto
                           }).SingleOrDefault();


                return sql;
            }
        }

        public List<AdditionalExamCustom> GetAdditionalExamByServiceId_all(string serviceId, int userId)
        {
            DatabaseContext dbcontext = new DatabaseContext();

            var list = (from ade in dbcontext.AdditionalExam
                        where ade.v_ServiceId == serviceId && ade.i_IsDeleted == 0 && ade.i_InsertUserId == userId
                        select new AdditionalExamCustom
                        {
                            ComponentId = ade.v_ComponentId,
                            ServiceId = ade.v_ServiceId,
                            IsProcessed = ade.i_IsProcessed.Value,
                            IsNewService = ade.i_IsNewService.Value
                        }).ToList();

            return list;
        }

        public bool UpdateServiceForCalendar(DateTime InicioCircuito, int userId, string serviceId)
        {
            //mon.IsActive = true;

            try
            {
                DatabaseContext dbContext = new DatabaseContext();

                // Obtener la entidad fuente
                var objEntitySource = (from a in dbContext.Service
                                       where a.v_ServiceId == serviceId
                                       select a).FirstOrDefault();

                // Crear la entidad con los datos actualizados
                objEntitySource.i_ServiceStatusId = (int)ServiceStatus.Iniciado;
                objEntitySource.d_UpdateDate = DateTime.Now;
                objEntitySource.i_UpdateUserId = userId;

                // Guardar los cambios
                return dbContext.SaveChanges() > 0;

                
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public BoardServiceCustomList GetServicesPagedAndFiltered(BoardServiceCustomList data)
        {
            //mon.IsActive = true;

            try
            {
                DatabaseContext dbContext = new DatabaseContext();

                string pacient = data.Pacient == null ? "" : data.Pacient;
                string serviceId = data.ServiceId == null ? "" : data.ServiceId;
                string protocolId = data.ProtocolId == "-1" ? "" : data.ProtocolId;
                DateTime Desde = data.FechaDesde.Date;
                DateTime Hasta = data.FechaHasta.Date.AddHours(24);
                int skip = (data.Index - 1) * data.Take;
                var query = from A in dbContext.Service
                            join B in dbContext.Person on A.v_PersonId equals B.v_PersonId
                            join C in dbContext.Calendar on A.v_ServiceId equals C.v_ServiceId
                            join I in dbContext.Protocol on A.v_ProtocolId equals I.v_ProtocolId into I_join
                            from I in I_join.DefaultIfEmpty()
                            join M in dbContext.ServiceComponent on A.v_ServiceId equals M.v_ServiceId into M_join
                            from M in M_join.DefaultIfEmpty()
                            join L in dbContext.Calendar on A.v_ServiceId equals L.v_ServiceId into L_join
                            from L in L_join.DefaultIfEmpty()
                            join J in dbContext.Organization on I.v_EmployerOrganizationId equals J.v_OrganizationId into J_join
                            from J in J_join.DefaultIfEmpty()
                            join J11 in dbContext.Organization on I.v_CustomerOrganizationId equals J11.v_OrganizationId into J11_join
                            from J11 in J11_join.DefaultIfEmpty()
                            join J22 in dbContext.Organization on I.v_WorkingOrganizationId equals J22.v_OrganizationId into J22_join
                            from J22 in J22_join.DefaultIfEmpty()
                            join K in dbContext.SystemParameter on new { a = A.i_AptitudeStatusId.Value, b = 124 } equals new { a = K.i_ParameterId, b = K.i_GroupId } into K_join
                            from K in K_join.DefaultIfEmpty()
                            join N in dbContext.SystemUser on C.i_InsertUserId equals N.i_SystemUserId
                            join O in dbContext.Person on N.v_PersonId equals O.v_PersonId
                            join P in dbContext.SystemParameter on new { a = A.i_MasterServiceId.Value, b = 119 } equals new { a = P.i_ParameterId, b = P.i_GroupId } into P_join
                            from P in P_join.DefaultIfEmpty()

                            join E in dbContext.ServiceComponent on A.v_ServiceId equals E.v_ServiceId

                            join F in dbContext.SystemUser on E.i_MedicoTratanteId equals F.i_SystemUserId into F_join
                            from F in F_join.DefaultIfEmpty()

                            join G in dbContext.Person on F.v_PersonId equals G.v_PersonId into G_join
                            from G in G_join.DefaultIfEmpty()

                            join Q in dbContext.SystemParameter on new { a = I.i_EsoTypeId.Value, b = 118 } equals new { a = Q.i_ParameterId, b = Q.i_GroupId } into Q_join
                            from Q in Q_join.DefaultIfEmpty()

                            where A.i_IsDeleted == 0
                            && L.i_LineStatusId == (int)LineStatus.EnCircuito && (A.v_ProtocolId == protocolId || protocolId == "")
                            && A.d_ServiceDate > Desde && A.d_ServiceDate < Hasta && (B.v_FirstName.Contains(pacient) || B.v_FirstLastName.Contains(pacient) || B.v_SecondLastName.Contains(pacient))
                            && (A.v_ServiceId.Contains(serviceId))  && (A.i_MasterServiceId == data.Servicio || data.Servicio == -1)
                             && (C.i_ServiceTypeId == data.TipoServicio || data.TipoServicio == -1) && (A.i_ServiceStatusId == data.EstadoServicio || data.EstadoServicio == -1) && (A.i_InsertUserId == data.UsuarioMedico || data.UsuarioMedico == -1)
                            select new ServiceCustomList
                            {
                                b_FechaEntrega = false,
                                v_PersonId = B.v_PersonId,
                                v_ServiceId = A.v_ServiceId,
                                v_Pacient = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                v_PacientDocument = B.v_DocNumber + " " + B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                d_ServiceDate = A.d_ServiceDate,
                                i_ServiceStatusId = A.i_ServiceStatusId,
                                i_StatusLiquidation = A.i_StatusLiquidation,
                                v_CustomerOrganizationId = I.v_CustomerOrganizationId,
                                v_CustomerLocationId = I.v_CustomerLocationId,
                                i_MasterServiceId = A.i_MasterServiceId,
                                i_ServiceTypeId = I.i_MasterServiceTypeId,
                                i_EsoTypeId = I.i_EsoTypeId,
                                v_ProtocolId = A.v_ProtocolId,
                                v_ProtocolName = I.v_Name,
                                i_AptitudeStatusId = A.i_AptitudeStatusId,
                                i_ApprovedUpdateUserId = M.i_ApprovedUpdateUserId.Value,
                                CompMinera = J11.v_Name,
                                Tercero = J22.v_Name,
                                v_OrganizationName = J.v_Name,
                                i_ServiceId = C.i_ServiceId,
                                v_AptitudeStatusName = K.v_Value1,
                                v_DocNumber = B.v_DocNumber,
                                UsuarioCrea = O.v_FirstLastName + " " + O.v_SecondLastName + ", " + O.v_FirstName,
                                TipoServicioMaster = P.v_Value1,
                                TipoServicioESO = Q.v_Value1,
                                v_MedicoTratante = G.v_FirstLastName + " " + G.v_SecondLastName + ", " + G.v_FirstName
                            };


                List<ServiceCustomList> objData = query.ToList();

                var datos = (from a in objData
                             select new ServiceCustomList
                             {
                                 b_FechaEntrega = a.b_FechaEntrega,
                                 v_PersonId = a.v_PersonId,
                                 v_ServiceId = a.v_ServiceId,
                                 v_Pacient = a.v_Pacient,
                                 v_PacientDocument = a.v_PacientDocument,
                                 i_ServiceStatusId = a.i_ServiceStatusId,
                                 i_StatusLiquidation = a.i_StatusLiquidation,
                                 v_CustomerOrganizationId = a.v_CustomerOrganizationId,
                                 v_CustomerLocationId = a.v_CustomerLocationId,
                                 i_MasterServiceId = a.i_MasterServiceId,
                                 i_ServiceTypeId = a.i_ServiceTypeId,
                                 i_EsoTypeId = a.i_EsoTypeId,
                                 v_ProtocolId = a.v_ProtocolId,
                                 v_ProtocolName = a.v_ProtocolName,
                                 i_AptitudeStatusId = a.i_AptitudeStatusId,
                                 i_ApprovedUpdateUserId = a.i_ApprovedUpdateUserId,
                                 CompMinera = a.CompMinera,
                                 Tercero = a.Tercero,
                                 v_OrganizationName = a.v_OrganizationName,
                                 i_ServiceId = a.i_ServiceId,
                                 v_AptitudeStatusName = a.v_AptitudeStatusName,
                                 v_DocNumber = a.v_DocNumber,
                                 UsuarioCrea = a.UsuarioCrea,
                                 TipoServicio = a.TipoServicioMaster == "EXAMEN SALUD OCUPACIONAL" ? "EMO" + " - " + a.TipoServicioESO : a.TipoServicioMaster + " - " + a.TipoServicioESO,
                                 v_MedicoTratante = a.v_MedicoTratante == "-1" ? " - - -" : a.v_MedicoTratante == null ? " - - - " : a.v_MedicoTratante == "SAN LORENZO, CLINICA" ? "CLINICA SAN LORENZO" : a.v_MedicoTratante,
                                 Fecha = a.d_ServiceDate.ToString()
                             }).ToList();

                var result = datos.GroupBy(g => g.v_ServiceId).Select(s => s.First()).ToList();
                var List = new List<ServiceCustomList>(result);
                int totalRecords = List.Count;
                if (data.Take > 0)
                    List = List.Skip(skip).Take(data.Take).ToList();

                data.TotalRecords = totalRecords;
                data.List = List;

                return data;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public UsuarioGrabo DevolverDatosUsuarioGraboExamen(int categoriaId, string pstrserviceId)
        {
            DatabaseContext dbContext = new DatabaseContext();
            var objEntity = (from E in dbContext.ServiceComponent

                             join me in dbContext.SystemUser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                             from me in me_join.DefaultIfEmpty()

                             join pme in dbContext.Professional on me.v_PersonId equals pme.v_PersonId into pme_join
                             from pme in pme_join.DefaultIfEmpty()

                             join B in dbContext.Person on pme.v_PersonId equals B.v_PersonId

                             join C in dbContext.Component on E.v_ComponentId equals C.v_ComponentId

                             where E.v_ServiceId == pstrserviceId && C.i_CategoryId == categoriaId
                             select new UsuarioGrabo
                             {
                                 Firma = pme.b_SignatureImage,
                                 Nombre = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                 CMP = pme.v_ProfessionalCode
                             }).FirstOrDefault();

            return objEntity;

        }


        public List<DiagnosticRepositoryList> GetServiceComponentConclusionesDxServiceIdReport(string pstrServiceId)
        {
            //mon.IsActive = true;

            try
            {
                DatabaseContext dbContext = new DatabaseContext();
                var query = (from ccc in dbContext.DiagnosticRepository
                             join bbb in dbContext.Component on ccc.v_ComponentId equals bbb.v_ComponentId into J7_join
                             from bbb in J7_join.DefaultIfEmpty()

                             join ddd in dbContext.Diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId  // Diagnosticos                       

                             join fff in dbContext.SystemParameter on new { a = ccc.i_PreQualificationId.Value, b = 137 } // PRE-CALIFICACION
                                                                 equals new { a = fff.i_ParameterId, b = fff.i_GroupId } into J5_join
                             from fff in J5_join.DefaultIfEmpty()

                             join ggg in dbContext.SystemParameter on new { a = ccc.i_FinalQualificationId.Value, b = 138 } //CALIFICACION FINAL
                                                                 equals new { a = ggg.i_ParameterId, b = ggg.i_GroupId } into J4_join
                             from ggg in J4_join.DefaultIfEmpty()

                             join hhh in dbContext.SystemParameter on new { a = ccc.i_DiagnosticTypeId.Value, b = 139 } // TIPO DE DX [Enfermedad comun, etc]
                                                                     equals new { a = hhh.i_ParameterId, b = hhh.i_GroupId } into J3_join
                             from hhh in J3_join.DefaultIfEmpty()

                             join cat in dbContext.SystemParameter on new { a = bbb.i_CategoryId, b = 116 }
                                                                   equals new { a = cat.i_ParameterId, b = cat.i_GroupId } into cat_join
                             from cat in cat_join.DefaultIfEmpty()

                             where (ccc.v_ServiceId == pstrServiceId) &&
                             (ccc.i_IsDeleted == 0) &&
                             (ccc.i_FinalQualificationId == (int)FinalQualification.Definitivo ||
                             ccc.i_FinalQualificationId == (int)FinalQualification.Presuntivo ||
                             ccc.i_FinalQualificationId == (int)FinalQualification.Descartado)

                             orderby bbb.v_Name

                             select new DiagnosticRepositoryList
                             {
                                 v_DiagnosticRepositoryId = ccc.v_DiagnosticRepositoryId,
                                 v_ServiceId = ccc.v_ServiceId,
                                 v_ComponentId = ccc.v_ComponentId,
                                 v_DiseasesId = ccc.v_DiseasesId,
                                 v_DiseasesName = ddd.v_Name,
                                 v_ComponentName = bbb.v_Name,
                                 v_PreQualificationName = fff.v_Value1,
                                 v_FinalQualificationName = ggg.v_Value1,
                                 v_DiagnosticTypeName = hhh.v_Value1,
                                 v_ComponentFieldsId = ccc.v_ComponentFieldId,
                                 v_Dx_CIE10 = ddd.v_CIE10Id,
                                 i_DiagnosticTypeId = ccc.i_DiagnosticTypeId,
                                 i_FinalQualificationId = ccc.i_FinalQualificationId,
                                 i_CategoryId = bbb.i_CategoryId,
                                 Categoria = cat.v_Value1
                             }).ToList();

                // add the sequence number on the fly
                var finalQuery = query.Select((a, index) => new DiagnosticRepositoryList
                {
                    i_Item = index + 1,
                    v_DiagnosticRepositoryId = a.v_DiagnosticRepositoryId,
                    v_ServiceId = a.v_ServiceId,
                    v_ComponentId = a.v_ComponentId,
                    v_DiseasesId = a.v_DiseasesId,
                    v_DiseasesName = a.v_DiseasesName,
                    v_ComponentName = a.v_ComponentName,
                    v_DiagnosticTypeName = a.v_DiagnosticTypeName,
                    Recomendations = GetServiceRecommendationByDiagnosticRepositoryIdReport(a.v_DiagnosticRepositoryId),
                    Restrictions = GetServiceRestrictionByDiagnosticRepositoryIdReport(a.v_DiagnosticRepositoryId),
                    v_ComponentFieldsId = a.v_ComponentFieldsId,
                    v_Dx_CIE10 = a.v_Dx_CIE10,
                    i_DiagnosticTypeId = a.i_DiagnosticTypeId,
                    i_FinalQualificationId = a.i_FinalQualificationId,
                    i_CategoryId = a.i_CategoryId,
                    Categoria = a.Categoria
                }).ToList();

                return finalQuery;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // Beto
        public List<RestrictionList> GetServiceRestrictionByDiagnosticRepositoryIdReport(string pstrDiagnosticRepositoryId)
        {
            //mon.IsActive = true;

            try
            {
                DatabaseContext dbContext = new DatabaseContext();

                List<RestrictionList> query = (from ddd in dbContext.Restriction
                                               join eee in dbContext.MasterRecommendationRestricction on ddd.v_MasterRestrictionId
                                                                       equals eee.v_MasterRecommendationRestricctionId //                                                                                                                                                                              
                                               where (ddd.v_DiagnosticRepositoryId == pstrDiagnosticRepositoryId) &&
                                                     (ddd.i_IsDeleted == 0)
                                               select new RestrictionList
                                               {
                                                   v_RestrictionId = ddd.v_RestrictionId,
                                                   v_DiagnosticRepositoryId = ddd.v_DiagnosticRepositoryId,
                                                   v_ServiceId = ddd.v_ServiceId,
                                                   v_ComponentId = ddd.v_ComponentId,
                                                   v_MasterRestrictionId = ddd.v_MasterRestrictionId,
                                                   v_RestrictionName = eee.v_Name,

                                               }).ToList();

                // add the sequence number on the fly
                var finalQuery = query.Select((a, index) => new RestrictionList
                {
                    i_Item = index + 1,
                    v_RestrictionId = a.v_RestrictionId,
                    v_DiagnosticRepositoryId = a.v_DiagnosticRepositoryId,
                    v_ServiceId = a.v_ServiceId,
                    v_ComponentId = a.v_ComponentId,
                    v_MasterRestrictionId = a.v_MasterRestrictionId,
                    v_RestrictionName = a.v_RestrictionName,
                }).ToList();

                return finalQuery;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        // Alejandro
        public List<RecomendationList> GetServiceRecommendationByDiagnosticRepositoryIdReport(string pstrDiagnosticRepositoryId)
        {
            //mon.IsActive = true;
            try
            {
                DatabaseContext dbContext = new DatabaseContext();

                List<RecomendationList> query = (from ddd in dbContext.Recommendation
                                                 join eee in dbContext.MasterRecommendationRestricction on ddd.v_MasterRecommendationId
                                                                         equals eee.v_MasterRecommendationRestricctionId //                                                                                                                                                                              
                                                 where (ddd.v_DiagnosticRepositoryId == pstrDiagnosticRepositoryId) &&
                                                       (ddd.i_IsDeleted == 0)
                                                 select new RecomendationList
                                                 {
                                                     v_RecommendationId = ddd.v_RecommendationId,
                                                     v_DiagnosticRepositoryId = ddd.v_DiagnosticRepositoryId,
                                                     v_ServiceId = ddd.v_ServiceId,
                                                     v_ComponentId = ddd.v_ComponentId,
                                                     v_MasterRecommendationId = ddd.v_MasterRecommendationId,
                                                     v_RecommendationName = eee.v_Name,

                                                 }).ToList();

                var objData = query.AsEnumerable()
                     .GroupBy(x => x.v_RecommendationId)
                     .Select(group => group.First());


                // add the sequence number on the fly
                var finalQuery = objData.Select((a, index) => new RecomendationList
                {
                    i_Item = index + 1,
                    v_RecommendationId = a.v_RecommendationId,
                    v_DiagnosticRepositoryId = a.v_DiagnosticRepositoryId,
                    v_ServiceId = a.v_ServiceId,
                    v_ComponentId = a.v_ComponentId,
                    v_MasterRecommendationId = a.v_MasterRecommendationId,
                    v_RecommendationName = a.v_RecommendationName,
                }).ToList();

                return finalQuery;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public List<RecetaDespachoCustom> GetReceta(string serviceId)
        {
            using (var dbContext = new DatabaseContext())
            {
                var medicamentos = ProductSAMBHSDal.ObtenerContasolMedicamentos();
                var consulta = (from r in dbContext.Receta
                                join d in dbContext.DiagnosticRepository on r.v_DiagnosticRepositoryId equals d.v_DiagnosticRepositoryId into dJoin
                                from d in dJoin.DefaultIfEmpty()
                                join s in dbContext.Service on d.v_ServiceId equals s.v_ServiceId into sJoin
                                from s in sJoin.DefaultIfEmpty()

                                where s.v_ServiceId.Equals(serviceId)

                                select new RecetaDespachoCustom
                                {
                                    RecetaId = r.i_IdReceta,
                                    CantidadRecetada = r.d_Cantidad ?? 0,
                                    FechaFin = r.t_FechaFin ?? DateTime.Now,
                                    Duracion = r.v_Duracion,
                                    Dosis = r.v_Posologia,

                                    Despacho = (r.i_Lleva ?? 0) == 1,
                                    MedicinaId = r.v_IdProductoDetalle
                                }).ToList();

                foreach (var item in consulta)
                {
                    var prod = medicamentos.FirstOrDefault(p => p.IdProductoDetalle.Equals(item.MedicinaId));
                    if (prod == null) continue;
                    item.Medicamento = prod.NombreCompleto;
                    item.Presentacion = prod.Presentacion;
                    item.Ubicacion = prod.Ubicacion;
                }

                return consulta;
            }

        }


        public MedicoTratanteAtencionesCustom GetMedicoTratante(string pstrServiceId)
        {
            //mon.IsActive = true;  

            try
            {
                DatabaseContext dbContext = new DatabaseContext();

                var objEntity = (from A in dbContext.Service
                                 join B in dbContext.ServiceComponent on A.v_ServiceId equals B.v_ServiceId
                                 join C in dbContext.SystemUser on B.i_MedicoTratanteId equals C.i_SystemUserId
                                 join D in dbContext.Person on C.v_PersonId equals D.v_PersonId
                                 join E in dbContext.Professional on D.v_PersonId equals E.v_PersonId
                                 where A.v_ServiceId == pstrServiceId && B.i_IsRequiredId == 1

                                 select new MedicoTratanteAtencionesCustom
                                 {
                                     Nombre = D.v_FirstLastName + " " + D.v_SecondLastName + ", " + D.v_FirstName,
                                     Colegiatura = E.v_ProfessionalCode,
                                     Direccion = D.v_AdressLocation
                                 }).FirstOrDefault();

                return objEntity;
            }
            catch (Exception e)
            {
                return null;
            }

        }


        public ServiceList GetServicePersonData( string pstrServiceId)
        {
            //mon.IsActive = true;
            var isDeleted = 0;

            try
            {
                DatabaseContext dbContext = new DatabaseContext();

                var query = from A in dbContext.Service
                            join B in dbContext.SystemParameter on new { a = A.i_ServiceStatusId.Value, b = 125 }
                                     equals new { a = B.i_ParameterId, b = B.i_GroupId } into B_join  // ESTADO SERVICIO
                            from B in B_join.DefaultIfEmpty()

                            join G in dbContext.SystemParameter on new { a = A.i_AptitudeStatusId.Value, b = 124 } // ESTADO APTITUD ESO 
                                    equals new { a = G.i_ParameterId, b = G.i_GroupId } into J4_join
                            from G in J4_join.DefaultIfEmpty()

                            join J1 in dbContext.SystemParameter on new { a = 119, b = A.i_MasterServiceId.Value }  // DESCRIPCION DEL SERVICIO
                                                       equals new { a = J1.i_GroupId, b = J1.i_ParameterId } into J1_join
                            from J1 in J1_join.DefaultIfEmpty()

                            join J2 in dbContext.SystemParameter on new { a = 119, b = J1.i_ParentParameterId.Value } // TIPO DE SERVICIO
                                                      equals new { a = J2.i_GroupId, b = J2.i_ParameterId } into J2_join
                            from J2 in J2_join.DefaultIfEmpty()

                            join D in dbContext.Person on A.v_PersonId equals D.v_PersonId

                            join J in dbContext.SystemParameter on new { a = D.i_SexTypeId.Value, b = 100 }
                                               equals new { a = J.i_ParameterId, b = J.i_GroupId } into J_join // GENERO
                            from J in J_join.DefaultIfEmpty()

                            join E in dbContext.Protocol on A.v_ProtocolId equals E.v_ProtocolId

                            join F in dbContext.GroupOccupation on E.v_GroupOccupationId equals F.v_GroupOccupationId

                            join H in dbContext.SystemParameter on new { a = E.i_EsoTypeId.Value, b = 118 }
                                                equals new { a = H.i_ParameterId, b = H.i_GroupId } into J3_join  // TIPO ESO [ESOA,ESOR,ETC]
                            from H in J3_join.DefaultIfEmpty()

                            where (A.v_ServiceId == pstrServiceId) &&
                                  (A.i_IsDeleted == isDeleted)

                            select new ServiceList
                            {
                                v_ServiceId = A.v_ServiceId,
                                v_ProtocolId = A.v_ProtocolId,
                                v_ProtocolName = E.v_Name,
                                v_PersonId = D.v_PersonId,
                                v_FirstName = D.v_FirstName,
                                v_FirstLastName = D.v_FirstLastName,
                                v_SecondLastName = D.v_SecondLastName,
                                d_BirthDate = D.d_Birthdate,
                                i_SexTypeId = D.i_SexTypeId.Value,
                                i_DocTypeId = D.i_DocTypeId.Value,
                                v_Mail = D.v_Mail,
                                v_ServiceStatusName = B.v_Value1,
                                i_AptitudeStatusId = A.i_AptitudeStatusId.Value,
                                d_GlobalExpirationDate = A.d_GlobalExpirationDate.Value,
                                d_ObsExpirationDate = A.d_ObsExpirationDate,
                                d_ServiceDate = A.d_ServiceDate,
                                v_MasterServiceName = J1.v_Value1,
                                v_ServiceTypeName = J2.v_Value1,
                                i_MasterServiceId = A.i_MasterServiceId.Value,
                                v_GroupOcupationName = F.v_Name,
                                v_EsoTypeName = H.v_Value1,
                                i_ServiceTypeId = E.i_MasterServiceTypeId.Value,
                                v_GenderName = J.v_Value1,
                                v_AdressLocation = D.v_AdressLocation,
                                v_BirthPlace = D.v_BirthPlace,
                                v_TelephoneNumber = D.v_TelephoneNumber,
                                i_HasSymptomId = A.i_HasSymptomId.Value,
                                v_MainSymptom = A.v_MainSymptom,
                                i_TimeOfDisease = A.i_TimeOfDisease.Value,
                                i_TimeOfDiseaseTypeId = A.i_TimeOfDiseaseTypeId.Value,
                                v_Story = A.v_Story,
                                i_DreamId = A.i_DreamId.Value,
                                i_UrineId = A.i_UrineId.Value,
                                i_DepositionId = A.i_DepositionId.Value,
                                i_AppetiteId = A.i_AppetiteId.Value,
                                i_ThirstId = A.i_ThirstId.Value,
                                d_Fur = A.d_Fur.Value,
                                v_CatemenialRegime = A.v_CatemenialRegime,
                                i_MacId = A.i_MacId.Value,
                                i_DestinationMedicationId = A.i_DestinationMedicationId.Value,
                                i_TransportMedicationId = A.i_TransportMedicationId.Value,
                                i_HasMedicalBreakId = A.i_HasMedicalBreakId.Value,
                                i_HasRestrictionId = A.i_HasRestrictionId.Value,
                                d_MedicalBreakStartDate = A.d_MedicalBreakStartDate,
                                d_MedicalBreakEndDate = A.d_MedicalBreakEndDate,
                                d_StartDateRestriction = A.d_StartDateRestriction,
                                d_EndDateRestriction = A.d_EndDateRestriction,
                                v_GeneralRecomendations = A.v_GeneralRecomendations,
                                i_IsNewControl = A.i_IsNewControl.Value,
                                b_PersonImage = D.b_PersonImage,
                                //i_HazInterconsultationId = A.i_HazInterconsultationId.Value,
                                d_NextAppointment = A.d_NextAppointment,
                                i_SendToTracking = A.i_SendToTracking.Value,
                                v_CurrentOccupation = D.v_CurrentOccupation,
                                d_PAP = A.d_PAP.Value,
                                d_Mamografia = A.d_Mamografia.Value,
                                v_CiruGine = A.v_CiruGine,
                                v_Gestapara = A.v_Gestapara,
                                v_Menarquia = A.v_Menarquia,
                                v_Findings = A.v_Findings,
                                i_InicioEnf = A.i_InicioEnf.Value,
                                i_CursoEnf = A.i_CursoEnf.Value,
                                i_Evolucion = A.i_Evolucion.Value,
                                v_ExaAuxResult = A.v_ExaAuxResult,
                                v_ObsStatusService = A.v_ObsStatusService,
                                v_FechaUltimoPAP = A.v_FechaUltimoPAP,
                                v_ResultadosPAP = A.v_ResultadosPAP,
                                v_FechaUltimaMamo = A.v_FechaUltimaMamo,
                                v_ResultadoMamo = A.v_ResultadoMamo,
                                v_DocNumber = D.v_DocNumber,
                                v_InicioVidaSexaul = A.v_InicioVidaSexaul,
                                v_NroParejasActuales = A.v_NroParejasActuales,
                                v_NroAbortos = A.v_NroAbortos,
                                v_PrecisarCausas = A.v_PrecisarCausas,
                                i_BloodFactorId = D.i_BloodGroupId.Value,
                                i_BloodGroupId = D.i_BloodFactorId.Value,
                                v_Procedencia = D.v_Procedencia,
                                v_CentroEducativo = D.v_CentroEducativo,
                                i_LevelOfId = D.i_LevelOfId.Value,
                                i_MaritalStatusId = D.i_MaritalStatusId.Value,
                                v_CustomerOrganizationId = E.v_CustomerOrganizationId,
                                v_EmployerOrganizationId = E.v_EmployerOrganizationId
                            };

                ServiceList objData = query.FirstOrDefault();

                return objData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<string> GetServicesId()
        {
            try
            {
                List<string> servicesId = (from ser in ctx.Service
                                           select ser.v_ServiceId).ToList();

                return servicesId;

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
