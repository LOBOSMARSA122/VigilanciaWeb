using BE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using BE.Eso;
using BE.Protocol;
using BE.Service;
using DAL.Common;
using DAL.Protocol;
using DAL.Service;

namespace DAL.Calendar
{
    public class SchedulePersonDal
    {
        public string Schedule(CalendarDto oCalendarDto, int nodeId, int systemUserId)
        {
            //using (var ts = new TransactionScope())
            //{
                var oServiceDto = PopulateServiceDto(oCalendarDto);

                var serviceId = new ServiceDal().AddService(oServiceDto, nodeId, systemUserId);

                AddServiceComponentInBlock(serviceId, oCalendarDto.v_ProtocolId, oCalendarDto.v_PersonId, nodeId,
                    systemUserId);

                oCalendarDto.v_ServiceId = serviceId;
                AddCalendar(oCalendarDto, nodeId, systemUserId);

                return serviceId;
                
            //}
        }

        public string SaveAndGetServiceComponentId(string serviceId, string personId, List<ServiceComponentFieldsList> oServicecomponentfields, int nodeId, int systemUserId)
        {
            var listServiceComponentDto = new List<ServiceComponentDto>();

            var components = oServicecomponentfields.GroupBy(g => g.v_ComponentId).Select(s => s.FirstOrDefault());

            foreach (var Component in components)
                listServiceComponentDto.Add(PopulateServiceComponentTemp(serviceId, Component.v_ComponentId, nodeId, systemUserId));

            var obj = new ServiceComponentDal().AddServiceComponentInBlockTemp(listServiceComponentDto, nodeId, systemUserId);

            obj.Sort((x, y) => x.v_ComponentId.CompareTo(y.v_ComponentId));
            // Orden obligatorio para capturar siempre el v_ServiceComponentId correcto
            obj.OrderBy(o1 => o1.v_ServiceComponentId).ToList();

            return obj[0].v_ServiceComponentId;
        }


        public void AddServiceComponentTemp(string serviceId, string componentId, int nodeId, int systemUserId)
        {
            var listServiceComponentDto = new List<ServiceComponentDto>();
            listServiceComponentDto.Add(PopulateServiceComponentTemp(serviceId, componentId, nodeId, systemUserId));

            new ServiceComponentDal().AddServiceComponentInBlock(listServiceComponentDto, nodeId, systemUserId);

        }
        #region Private Methods

        private void AddCalendar(CalendarDto oCalendarDto, int nodeId, int systemUserId)
        {
            //var oCalendarDto = PopulateCalendarDtoSurveillance(serviceId, personId, protocolId);
            new CalendarDal().AddCalendar(oCalendarDto, nodeId, systemUserId);
        }

        private CalendarDto PopulateCalendarDtoSurveillance(string serviceId, string personId, string protocolId)
        {
            var oCalendarDto = new CalendarDto();
            oCalendarDto.v_ServiceId = serviceId;
            oCalendarDto.v_PersonId = personId;
            //oCalendarDto.d_DateTimeCalendar = DateTime.Now;
            //oCalendarDto.d_CircuitStartDate = DateTime.Now;
            //oCalendarDto.d_EntryTimeCM = DateTime.Now;
            oCalendarDto.i_ServiceTypeId = (int)Enumeratores.masterService.AtxMedica;
            oCalendarDto.i_CalendarStatusId = (int)Enumeratores.CalendarStatus.Atendido;
            oCalendarDto.i_ServiceId = (int)Enumeratores.masterService.AtxMedicaParticular;
            oCalendarDto.v_ProtocolId = protocolId;
            oCalendarDto.i_NewContinuationId = (int)Enumeratores.Modality.NuevoServicio;
            oCalendarDto.i_LineStatusId = (int)Enumeratores.LineStatus.EnCircuito;
            oCalendarDto.i_IsVipId = (int)Enumeratores.SiNo.No;

            return oCalendarDto;
        }

        private void AddServiceComponentInBlock(string serviceId, string protocolId, string personId, int nodeId, int systemUserId)
        {
            var protocolComponents = new ProtocolComponentDal().GetProtocolComponents(protocolId);

            var listServiceComponentDto = new List<ServiceComponentDto>();
            foreach (var protocolComponent in protocolComponents)
            {
                listServiceComponentDto.Add(PopulateServiceComponentDto(serviceId, personId, protocolComponent, nodeId, systemUserId));
            }

            new ServiceComponentDal().AddServiceComponentInBlock(listServiceComponentDto, nodeId, systemUserId);

        }
        
        private ServiceComponentDto PopulateServiceComponentTemp(string serviceId, string componentId, int nodeId, int systemUserId)
        {
            var oServiceComponentDto = new ServiceComponentDto
            {
                v_ServiceId = serviceId,
                i_ExternalInternalId = (int)Enumeratores.ComponenteProcedencia.Interno,
                i_ServiceComponentTypeId = (int)Enumeratores.ComponentType.Examen,
                i_IsVisibleId = (int)Enumeratores.SiNo.No,
                i_IsInheritedId = (int)Enumeratores.SiNo.No,
                i_index = 0,// protocolComponent.UIIndex,
                r_Price = 0,
                v_ComponentId = componentId,
                i_IsInvoicedId = (int)Enumeratores.SiNo.No,
                i_ServiceComponentStatusId = (int)Enumeratores.ServiceStatus.PorIniciar,
                i_QueueStatusId = (int)Enumeratores.QueueStatusId.Libre,
                i_Iscalling = (int)Enumeratores.Flag_Call.NoseLlamo,
                i_Iscalling_1 = (int)Enumeratores.Flag_Call.NoseLlamo,
                v_IdUnidadProductiva = string.Empty,
                i_IsManuallyAddedId = (int)Enumeratores.SiNo.No,
                i_IsRequiredId = (int)Enumeratores.SiNo.Si
            };
            return oServiceComponentDto;
        }

        private ServiceComponentDto PopulateServiceComponentDto(string serviceId, string personId, ProtocolComponentCustom protocolComponent, int nodeId, int systemUserId)
        {
            var oServiceComponentDto = new ServiceComponentDto
            {
                v_ServiceId = serviceId,
                i_ExternalInternalId = (int)Enumeratores.ComponenteProcedencia.Interno,
                i_ServiceComponentTypeId = protocolComponent.ComponentTypeId,
                i_IsVisibleId = protocolComponent.UIIsVisibleId,
                i_IsInheritedId = (int)Enumeratores.SiNo.No,
                i_index = protocolComponent.UIIndex,
                r_Price = 0,
                v_ComponentId = protocolComponent.ComponentId,
                i_IsInvoicedId = (int)Enumeratores.SiNo.No,
                i_ServiceComponentStatusId = (int)Enumeratores.ServiceStatus.PorIniciar,
                i_QueueStatusId = (int)Enumeratores.QueueStatusId.Libre,
                i_Iscalling = (int)Enumeratores.Flag_Call.NoseLlamo,
                i_Iscalling_1 = (int)Enumeratores.Flag_Call.NoseLlamo,
                v_IdUnidadProductiva = protocolComponent.IdUnidadProductiva,
                i_IsManuallyAddedId = (int)Enumeratores.SiNo.No,
                i_IsRequiredId = protocolComponent.IsConditionalId == (int)Enumeratores.SiNo.No ? (int)Enumeratores.SiNo.Si : RequiredComponent(personId, protocolComponent, nodeId, systemUserId)
            };
            return oServiceComponentDto;
        }

        private int? RequiredComponent(string personId, ProtocolComponentCustom protocolComponent, int nodeId, int systemUserId)
        {
            var operationResult = new OperationResult();
            var personDto = new PersonDal().GetPersonDto(personId);

            var pacientAge = personDto.d_Birthdate == null ? 0 : Common.Utils.GetAge(personDto.d_Birthdate.Value);
            var pacientGender = personDto.i_SexTypeId == null ? 0 : personDto.i_SexTypeId.Value;
            var analyzeAge = (int)protocolComponent.Age;
            var analyzeGender = (int)protocolComponent.GenderId;
            var @operator = (Enumeratores.Operator2Values)protocolComponent.OperatorId;
            var oGrupoEtario = (Enumeratores.GrupoEtario)protocolComponent.GrupoEtarioId;

            var oOperatorX = new OperatorX { value = @operator };

            var result = oOperatorX.IsRequired(pacientAge, analyzeAge, pacientGender, analyzeGender);

            return result;
        }

        private ServiceDto PopulateServiceDto(CalendarDto oCalendarDto)
        {
            var oServiceDto = new ServiceDto
            {
                v_ProtocolId = oCalendarDto.v_ProtocolId,
                v_PersonId = oCalendarDto.v_PersonId,
                i_MasterServiceId = (int)oCalendarDto.i_ServiceId,
                i_ServiceStatusId = (int)Enumeratores.ServiceStatus.PorIniciar,
                i_AptitudeStatusId = (int)Enumeratores.AptitudeStatus.SinAptitud,
                d_ServiceDate = oCalendarDto.d_DateTimeCalendar,
                d_GlobalExpirationDate = null,
                d_ObsExpirationDate = null,
                i_FlagAgentId = 1,
                v_Motive = string.Empty,
                i_IsFac = (int)Enumeratores.SiNo.No
            };

            return oServiceDto;

        }

        #endregion

    }
}
