using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using BE.Protocol;
using BE.Service;
using DAL.Common;
using DAL.Plan;
using static BE.Common.Enumeratores;

namespace DAL.Service
{
   public class ServiceComponentDal
    {
        private static DatabaseContext ctx = new DatabaseContext();

        public void AddServiceComponentInBlock(List<ServiceComponentDto> list, int nodeId, int systemUserId)
        {
            using (var oCtx = new DatabaseContext())
            {
                foreach (var item in list)
                {
                    var serviceComponentId = new Common.Utils().GetPrimaryKey(nodeId, 24, "SC");

                    item.v_ServiceComponentId = serviceComponentId;
                    item.i_IsDeleted = (int)Enumeratores.SiNo.No;
                    item.d_InsertDate = DateTime.UtcNow;
                    item.i_InsertUserId = systemUserId;

                    oCtx.ServiceComponent.Add(item);
                }
                oCtx.SaveChanges();
            }
            
        }

        public List<ServiceComponentTemp> AddServiceComponentInBlockTemp(List<ServiceComponentDto> list, int nodeId, int systemUserId)
        {
            using (var oCtx = new DatabaseContext())
            {
                var outList = new List<ServiceComponentTemp>();
                foreach (var item in list)
                {
                    var serviceComponentId = new Common.Utils().GetPrimaryKey(nodeId, 24, "SC");

                    item.v_ServiceComponentId = serviceComponentId;
                    item.i_IsDeleted = (int)Enumeratores.SiNo.No;
                    item.d_InsertDate = DateTime.UtcNow;
                    item.i_InsertUserId = systemUserId;

                    oCtx.ServiceComponent.Add(item);

                    var oClassCustom = new ServiceComponentTemp
                    {
                        v_ComponentId = item.v_ComponentId,
                        v_ServiceComponentId = serviceComponentId
                    };

                    outList.Add(oClassCustom);
                }
                oCtx.SaveChanges();

                return outList;
            }
            
        }

        public List<ServiceComponentDto> GetServiceComponentDtos(string serviceId)
        {
            using (var oCtx = new DatabaseContext())
            {
                var list = (from A in oCtx.ServiceComponent where A.v_ServiceId == serviceId && A.i_IsDeleted == (int)SiNo.No select A).ToList();

                return list;
            }
        }

        public ServiceComponentDto UpdateServiceComponentId(ServiceComponentDto oDataServiceComponent, int nodeId, int systemUserId)
        {
            using (var oCtx = new DatabaseContext())
            {
                var obj = (from a in oCtx.ServiceComponent where a.v_ServiceComponentId == oDataServiceComponent.v_ServiceComponentId select a).FirstOrDefault();
                var componentId = obj.v_ComponentId;
                var serviceId = obj.v_ServiceId;
                var categoryId = (from a in oCtx.Component where a.v_ComponentId == componentId select a).FirstOrDefault().i_CategoryId;

                var components = (from a in oCtx.Component where a.i_CategoryId == categoryId select a.v_ComponentId).ToList();
                
                var oEntidadFuerte =
                    (from A in oCtx.ServiceComponent
                        where A.v_ServiceId == serviceId  && components.Contains(A.v_ComponentId)
                        //where A.v_ServiceComponentId == oDataServiceComponent.v_ServiceComponentId
                        select A).ToList();

                foreach (var item in oEntidadFuerte)
                {
                    item.v_Comment = oDataServiceComponent.v_Comment;
                    item.i_ServiceComponentStatusId = oDataServiceComponent.i_ServiceComponentStatusId;
                    //item.i_ExternalInternalId = oDataServiceComponent.i_ExternalInternalId;
                    item.i_IsApprovedId = oDataServiceComponent.i_IsApprovedId;
                    item.i_ApprovedUpdateUserId = systemUserId;
                    item.i_UpdateUserId = systemUserId;
                    item.d_UpdateDate = DateTime.Now;
                }
                
                oCtx.SaveChanges();

                return oEntidadFuerte[0];
            }
        }

        public List<ServiceComponentBe> ServiceComponentByServiceId(string serviceId)
        {
            using (var contex = new DatabaseContext())
            {
                var query = (from a in contex.ServiceComponent
                    where a.v_ServiceId == serviceId
                    select new ServiceComponentBe
                    {
                        ServiceId = a.v_ServiceId,
                        ServiceComponentId = a.v_ServiceComponentId,
                        ServiceComponentStatusId = a.i_ServiceComponentStatusId.Value
                    }).ToList();

                return query;
            }
        }

        public bool AddServiceComponent(List<ProtocolComponentCustom> dataProtComponent, ServiceCustom dataService, int nodeId, int userId)
        {
            try
            {
                foreach (var obj in dataProtComponent)
                {
                    var serviceComponentId = new Common.Utils().GetPrimaryKey(nodeId, 24, "SC");
                    ServiceComponentDto oServiceComponentDto = new ServiceComponentDto();
                    oServiceComponentDto.v_ServiceComponentId = serviceComponentId;
                    oServiceComponentDto.v_ComponentId = obj.ComponentId;
                    oServiceComponentDto.i_MedicoTratanteId = dataService.MedicoTratanteId;
                    oServiceComponentDto.v_ServiceId = dataService.ServiceId;
                    oServiceComponentDto.i_ExternalInternalId = (int)ComponenteProcedencia.Interno;
                    oServiceComponentDto.i_ServiceComponentTypeId = obj.ComponentTypeId;
                    oServiceComponentDto.i_IsVisibleId = obj.UIIsVisibleId;
                    oServiceComponentDto.i_IsInheritedId = (int)SiNo.No;
                    oServiceComponentDto.d_StartDate = null;
                    oServiceComponentDto.d_EndDate = null;
                    oServiceComponentDto.i_index = obj.UIIndex;
                    oServiceComponentDto.i_IsDeleted = (int)SiNo.No;
                    oServiceComponentDto.d_InsertDate = DateTime.Now;
                    oServiceComponentDto.i_InsertUserId = userId;
                    var porcentajes = obj.Porcentajes.Split('-');

                    float p1 = porcentajes[0] == null || porcentajes[0] == "" ? 0 : float.Parse(porcentajes[0]);
                    float p2 = porcentajes[1] == null || porcentajes[1] == "" ? 0 : float.Parse(porcentajes[1]);

                    var pb = obj.Price;
                    oServiceComponentDto.r_Price = pb + (pb * p1 / 100) + (pb * p2 / 100);
                    oServiceComponentDto.r_Price = SetNewPrice(oServiceComponentDto.r_Price, obj.ComponentId);
                    oServiceComponentDto.i_IsInvoicedId = (int)SiNo.No;
                    oServiceComponentDto.i_ServiceComponentStatusId = (int)ServiceStatus.PorIniciar;
                    oServiceComponentDto.i_QueueStatusId = (int)QueueStatusId.Libre;
                    oServiceComponentDto.i_Iscalling = (int)FlagCall.NoseLlamo;
                    oServiceComponentDto.i_Iscalling_1 = (int)FlagCall.NoseLlamo;
                    oServiceComponentDto.v_IdUnidadProductiva = obj.IdUnidadProductiva;

                    var resultplan = new PlanDal().GetPlans(dataService.ProtocolId, obj.IdUnidadProductiva);
                    var tienePlan = false;
                    if (resultplan.Count > 0) tienePlan = true;
                    else tienePlan = false;
                    if (tienePlan)
                    {
                        if (resultplan[0].i_EsCoaseguro == 1)
                        {
                            oServiceComponentDto.d_SaldoPaciente = resultplan[0].d_Importe * decimal.Parse(oServiceComponentDto.r_Price.ToString()) / 100;
                            oServiceComponentDto.d_SaldoAseguradora = decimal.Parse(oServiceComponentDto.r_Price.ToString()) - oServiceComponentDto.d_SaldoPaciente;
                        }
                        if (resultplan[0].i_EsDeducible == 1)
                        {
                            oServiceComponentDto.d_SaldoPaciente = resultplan[0].d_Importe;
                            oServiceComponentDto.d_SaldoAseguradora = decimal.Parse(oServiceComponentDto.r_Price.ToString()) - resultplan[0].d_Importe;

                        }
                    }

                    //Condicionales
                    var conditional = obj.IsConditionalId;
                    if (conditional == (int)SiNo.Si)
                    {
                        var fechaNacimiento = dataService.FechaNacimiento;
                        //Datos del paciente

                        if (fechaNacimiento != null)
                        {
                            var pacientAge = DateTime.Today.AddTicks(-fechaNacimiento.Ticks).Year - 1;

                            var pacientGender = dataService.GeneroId;

                            //Datos del protocolo
                            int analyzeAge = obj.Age.Value;
                            int analyzeGender = obj.GenderId.Value;
                            var @operator = (Operator2Values)obj.OperatorId;
                            GrupoEtario oGrupoEtario = (GrupoEtario)obj.GrupoEtarioId;
                            if (analyzeAge >= 0)//condicional edad (SI)
                            {
                                if (analyzeGender != (int)GenderConditional.Ambos)//condicional genero (SI)
                                {
                                    if (@operator == Operator2Values.X_esIgualque_A)
                                    {
                                        if (pacientAge == analyzeAge && pacientGender == analyzeGender) { oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si; }
                                        else { oServiceComponentDto.i_IsRequiredId = (int)SiNo.No; }
                                    }
                                    if (@operator == Operator2Values.X_esMayorIgualque_A)
                                    {
                                        if (pacientAge >= analyzeAge && pacientGender == analyzeGender) { oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si; }
                                        else { oServiceComponentDto.i_IsRequiredId = (int)SiNo.No; }
                                    }
                                    if (@operator == Operator2Values.X_esMayorque_A)
                                    {
                                        if (pacientAge > analyzeAge && pacientGender == analyzeGender) { oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si; }
                                        else { oServiceComponentDto.i_IsRequiredId = (int)SiNo.No; }
                                    }
                                    if (@operator == Operator2Values.X_esMenorIgualque_A)
                                    {
                                        if (pacientAge <= analyzeAge && pacientGender == analyzeGender) { oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si; }
                                        else { oServiceComponentDto.i_IsRequiredId = (int)SiNo.No; }
                                    }
                                }
                                else//condicional genero (NO)
                                {
                                    if (@operator == Operator2Values.X_esIgualque_A)
                                    {
                                        if (pacientAge == analyzeAge) { oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si; }
                                        else { oServiceComponentDto.i_IsRequiredId = (int)SiNo.No; }
                                    }
                                    if (@operator == Operator2Values.X_esMayorIgualque_A)
                                    {
                                        if (pacientAge >= analyzeAge) { oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si; }
                                        else { oServiceComponentDto.i_IsRequiredId = (int)SiNo.No; }
                                    }
                                    if (@operator == Operator2Values.X_esMayorque_A)
                                    {
                                        if (pacientAge > analyzeAge) { oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si; }
                                        else { oServiceComponentDto.i_IsRequiredId = (int)SiNo.No; }
                                    }
                                    if (@operator == Operator2Values.X_esMenorIgualque_A)
                                    {
                                        if (pacientAge <= analyzeAge) { oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si; }
                                        else { oServiceComponentDto.i_IsRequiredId = (int)SiNo.No; }
                                    }
                                }

                            }

                        }
                    }
                    else
                    {
                        oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                        if (obj.IsAdditional == null) continue;
                        var adicional = obj.IsAdditional;
                        if (adicional == 1)
                        {
                            oServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                        }
                    }

                    oServiceComponentDto.i_ConCargoA = 0;
                    oServiceComponentDto.i_IsManuallyAddedId = (int)SiNo.No;
                    ctx.ServiceComponent.Add(oServiceComponentDto);
                }
                return ctx.SaveChanges() > 0; 
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateServiceComponent(string serviceId, int userId) {
            try
            {
                var ListServiceComponent = ctx.ServiceComponent.Where(X => X.v_ServiceId == serviceId && X.i_ConCargoA == 0).ToList();

                foreach (var obj in ListServiceComponent)
                {
                    obj.i_MedicoTratanteId = -1;
                    obj.d_UpdateDate = DateTime.Now;
                    obj.i_UpdateUserId = userId;
                }

                return ctx.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AddServiceComponentAtx(List<ProtocolComponentCustom> dataProtComponent, ServiceCustom dataService, int nodeId, int userId)
        {
            try
            {
                foreach (var obj in dataProtComponent)
                {
                    var componentId = obj.ComponentId;
                    var serviceComponentId = new Common.Utils().GetPrimaryKey(nodeId, 24, "SC");
                    ServiceComponentDto oServiceComponentDto = new ServiceComponentDto();
                    oServiceComponentDto.v_ServiceComponentId = serviceComponentId;
                    oServiceComponentDto.v_ComponentId = obj.ComponentId;
                    oServiceComponentDto.i_MedicoTratanteId = dataService.MedicoTratanteId;
                    oServiceComponentDto.v_ServiceId = dataService.ServiceId;
                    oServiceComponentDto.i_ExternalInternalId = (int)ComponenteProcedencia.Interno;
                    oServiceComponentDto.i_ServiceComponentTypeId = obj.ComponentTypeId;
                    oServiceComponentDto.i_IsVisibleId = obj.UIIsVisibleId;
                    oServiceComponentDto.i_IsInheritedId = (int)SiNo.No;
                    oServiceComponentDto.d_StartDate = null;
                    oServiceComponentDto.d_EndDate = null;
                    oServiceComponentDto.i_index = obj.UIIndex;

                    var porcentajes = obj.Porcentajes.Split('-');

                    float p1 = porcentajes[0] == null || porcentajes[0] == "" ? 0 : float.Parse(porcentajes[0]);
                    float p2 = porcentajes[1] == null || porcentajes[1] == "" ? 0 : float.Parse(porcentajes[1]);

                    var pb = obj.Price;
                    oServiceComponentDto.r_Price = pb + (pb * p1 / 100) + (pb * p2 / 100);
                    oServiceComponentDto.r_Price = SetNewPrice(oServiceComponentDto.r_Price, obj.ComponentId);
                    oServiceComponentDto.i_IsInvoicedId = (int)SiNo.No;
                    oServiceComponentDto.i_ServiceComponentStatusId = (int)ServiceStatus.PorIniciar;
                    oServiceComponentDto.i_QueueStatusId = (int)QueueStatusId.Libre;
                    oServiceComponentDto.i_Iscalling = (int)FlagCall.NoseLlamo;
                    oServiceComponentDto.i_Iscalling_1 = (int)FlagCall.NoseLlamo;
                    oServiceComponentDto.v_IdUnidadProductiva = obj.IdUnidadProductiva;

                    var resultplan = new PlanDal().GetPlans(dataService.ProtocolId, obj.IdUnidadProductiva);
                    var tienePlan = false;
                    if (resultplan.Count > 0) tienePlan = true;
                    else tienePlan = false;
                    if (tienePlan)
                    {
                        if (resultplan[0].i_EsCoaseguro == 1)
                        {
                            oServiceComponentDto.d_SaldoPaciente = resultplan[0].d_Importe * decimal.Parse(oServiceComponentDto.r_Price.ToString()) / 100;
                            oServiceComponentDto.d_SaldoAseguradora = decimal.Parse(oServiceComponentDto.r_Price.ToString()) - oServiceComponentDto.d_SaldoPaciente;
                        }
                        if (resultplan[0].i_EsDeducible == 1)
                        {
                            oServiceComponentDto.d_SaldoPaciente = resultplan[0].d_Importe;
                            oServiceComponentDto.d_SaldoAseguradora = decimal.Parse(oServiceComponentDto.r_Price.ToString()) - resultplan[0].d_Importe;

                        }
                    }

                    var conditional = obj.IsConditionalId;
                    if (conditional == (int)SiNo.Si)
                    {
                        var fechaNacimiento = dataService.FechaNacimiento;
                        //Datos del paciente

                        if (fechaNacimiento != null)
                        {
                            var pacientAge = DateTime.Today.AddTicks(-fechaNacimiento.Ticks).Year - 1;

                            var pacientGender = dataService.GeneroId;

                            //Datos del protocolo
                            int analyzeAge = obj.Age.Value;
                            int analyzeGender = obj.GenderId.Value;
                            var @operator = (Operator2Values)obj.OperatorId;
                            GrupoEtario oGrupoEtario = (GrupoEtario)obj.GrupoEtarioId;
                            if ((int)@operator == -1)
                            {
                                //si la condicional del operador queda en --Seleccionar--
                                if (analyzeGender == (int)GenderConditional.Ambos)
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                                }
                                else if (pacientGender == analyzeGender)
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                                }
                                else
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                                }
                            }
                            else
                            {
                                if (analyzeGender == (int)GenderConditional.Masculino)
                                {
                                    oServiceComponentDto.i_IsRequiredId = SwitchOperator2Values(pacientAge, analyzeAge,
                                        @operator, pacientGender.Value, analyzeGender);
                                }
                                else if (analyzeGender == (int)GenderConditional.Femenino)
                                {
                                    oServiceComponentDto.i_IsRequiredId = SwitchOperator2Values(pacientAge, analyzeAge,
                                        @operator, pacientGender.Value, analyzeGender);
                                }
                                else if (analyzeGender == (int)GenderConditional.Ambos)
                                {
                                    oServiceComponentDto.i_IsRequiredId = SwitchOperator2Values(pacientAge, analyzeAge,
                                        @operator, pacientGender.Value, analyzeGender);
                                }
                            }
                            if (componentId == "N009-ME000000402") //Adolecente
                            {
                                if ((int)oGrupoEtario == -1)
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                                }
                                else if (13 <= pacientAge && pacientAge <= 18)
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                                }
                                else
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                                }

                            }
                            else if (componentId == "N009-ME000000403") //Adulto
                            {
                                if ((int)oGrupoEtario == -1)
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                                }
                                else if (19 <= pacientAge && pacientAge <= 60)
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                                }
                                else
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                                }
                            }
                            else if (componentId == "N009-ME000000404") //AdultoMayor
                            {
                                if ((int)oGrupoEtario == -1)
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                                }
                                else if (61 <= pacientAge)
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                                }
                                else
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                                }
                            }
                            else if (componentId == "N009-ME000000406")
                            {
                                if ((int)oGrupoEtario == -1)
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                                }
                                else if (12 >= pacientAge)
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                                }
                                else
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                                }
                            }
                            else if (componentId == "N009-ME000000401") //plan integral
                            {
                                if ((int)oGrupoEtario == -1)
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                                }
                                else if (12 >= pacientAge)
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                                }
                                else
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                                }
                            }
                            else if (componentId == "N009-ME000000400") //atencion integral
                            {
                                if ((int)oGrupoEtario == -1)
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                                }
                                else if (12 >= pacientAge)
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                                }
                                else
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                                }
                            }
                            else if (componentId == "N009-ME000000405") //consulta
                            {
                                if ((int)oGrupoEtario == -1)
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                                }
                                else if (12 >= pacientAge)
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                                }
                                else
                                {
                                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                                }
                            }
                            else
                            {
                                oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                            }
                        }
                    }
                    else
                    {
                        oServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                        if (obj.IsAdditional == null) continue;
                        var adicional = obj.IsAdditional;
                        if (adicional == 1)
                        {
                            oServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                        }
                    }
                    oServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                    oServiceComponentDto.i_ConCargoA = 0;
                    ctx.ServiceComponent.Add(oServiceComponentDto);
                }
                return ctx.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static int SwitchOperator2Values(int pacientAge, int analyzeAge, Operator2Values @operator,
            int pacientGender, int analyzeGender)
        {
            ServiceComponentDto objServiceComponentDto = new ServiceComponentDto();
            switch (@operator)
            {
                case Operator2Values.X_esIgualque_A:
                    if (analyzeGender == (int)GenderConditional.Ambos)
                    {
                        if (pacientAge == analyzeAge)
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                        }
                        else
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                        }
                    }
                    else
                    {
                        if (pacientAge == analyzeAge && pacientGender == analyzeGender)
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                        }
                        else
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                        }
                    }

                    break;
                case Operator2Values.X_noesIgualque_A:
                    if (analyzeGender == (int)GenderConditional.Ambos)
                    {
                        if (pacientAge != analyzeAge)
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                        }
                        else
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                        }
                    }
                    else
                    {
                        if (pacientAge != analyzeAge && pacientGender == analyzeGender)
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                        }
                        else
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                        }
                    }

                    break;
                case Operator2Values.X_esMenorque_A:

                    if (analyzeGender == (int)GenderConditional.Ambos)
                    {
                        if (pacientAge < analyzeAge)
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                        }
                        else
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                        }
                    }
                    else
                    {
                        if (pacientAge < analyzeAge && pacientGender == analyzeGender)
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                        }
                        else
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                        }
                    }

                    break;
                case Operator2Values.X_esMenorIgualque_A:

                    if (analyzeGender == (int)GenderConditional.Ambos)
                    {
                        if (pacientAge <= analyzeAge)
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                        }
                        else
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                        }
                    }
                    else
                    {
                        if (pacientAge <= analyzeAge && pacientGender == analyzeGender)
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                        }
                        else
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                        }
                    }

                    break;
                case Operator2Values.X_esMayorque_A:
                    if (analyzeGender == (int)GenderConditional.Ambos)
                    {
                        if (pacientAge > analyzeAge)
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                        }
                        else
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                        }
                    }
                    else
                    {
                        if (pacientAge > analyzeAge && pacientGender == analyzeGender)
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                        }
                        else
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                        }
                    }
                    break;
                case Operator2Values.X_esMayorIgualque_A:
                    if (analyzeGender == (int)GenderConditional.Ambos)
                    {
                        if (pacientAge >= analyzeAge)
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                        }
                        else
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                        }
                    }
                    else
                    {
                        if (pacientAge >= analyzeAge && pacientGender == analyzeGender)
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                        }
                        else
                        {
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.No;
                        }
                    }

                    break;
            }

            return objServiceComponentDto.i_IsRequiredId.Value;
        }

        private static float SetNewPrice(float value, string componentId)
        {
            try
            {

                if (value <= 0) return value;
                var objComponent = ctx.Component.Where(x => x.v_ComponentId == componentId).FirstOrDefault();

                if (objComponent.i_PriceIsRecharged != (int)SiNo.Si) return value;

                DateTime now = DateTime.Now.Date;
                string year = now.Year.ToString();
                string day = now.Day.ToString();
                string month = now.Month.ToString();

                bool IsRecharged = false;

                var objHolidays = ctx.Holiday.Where(x => x.d_Date == now && x.i_Year == int.Parse(year)).FirstOrDefault();

                if (objHolidays != null)
                {
                    IsRecharged = true;
                }
                else if (now >= DateTime.Parse(day + "/" + month + "/" + year + " 20:00:00") && now < DateTime.Parse(day + "/" + month + "/" + year + " 08:00:00").AddDays(1))
                {
                    IsRecharged = true;
                }
                else if (now.DayOfWeek == DayOfWeek.Sunday)
                {
                    IsRecharged = true;
                }

                if (IsRecharged)
                {
                    float newValueRecharged = value + (value * float.Parse("0.2"));
                    newValueRecharged = float.Parse(newValueRecharged.ToString("N2"));
                    return newValueRecharged;
                }

                return value;
            }
            catch (Exception ex)
            {
                return value;
            }
        }


    }
}
