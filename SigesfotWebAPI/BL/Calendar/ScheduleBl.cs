using System;
using System.Collections.Generic;
using BE.Common;
using BE.Schedule;
using BE.Service;
using BE.Vigilancia;
using DAL.Calendar;
using DAL.Vigilancia;

namespace BL.Calendar
{
    public class ScheduleBl
    {
        public string ScheduleOcupationalConsultation(string personId, int nodeId, int systemUserId)
        {
            return "";
        }

        public string ScheduleMedicalConsultation(ScheduleCustom oScheduleCustom, int nodeId, int systemUserId)
        {
            if (oScheduleCustom.TypeId == (int)Enumeratores.TypeSchedule.AgendadoIniciado)
            {
                var oCalendarDto = new CalendarDto();
                oCalendarDto.v_PersonId = oScheduleCustom.PersonId;
                oCalendarDto.v_ProtocolId = Constants.PROTOCOL_VIGILANCIA;
                oCalendarDto.d_DateTimeCalendar = DateTime.Now;
                oCalendarDto.d_CircuitStartDate = DateTime.Now;
                oCalendarDto.d_EntryTimeCM = DateTime.Now;
                oCalendarDto.i_ServiceTypeId = (int)Enumeratores.masterService.AtxMedicaParticular;
                oCalendarDto.i_CalendarStatusId = (int)Enumeratores.CalendarStatus.Atendido;
                oCalendarDto.i_ServiceId = (int)Enumeratores.masterService.AtxMedicaParticular;
                oCalendarDto.i_NewContinuationId = (int)Enumeratores.Modality.NuevoServicio;
                oCalendarDto.i_LineStatusId = (int)Enumeratores.LineStatus.EnCircuito;
                oCalendarDto.i_IsVipId = (int)Enumeratores.SiNo.No;

                return new SchedulePersonDal().Schedule(oCalendarDto, nodeId, systemUserId);
            }
            else
            {
                var oCalendarDto = new CalendarDto();
                oCalendarDto.v_PersonId = oScheduleCustom.PersonId;
                oCalendarDto.v_ProtocolId = Constants.PROTOCOL_VIGILANCIA;
                oCalendarDto.d_DateTimeCalendar = oScheduleCustom.ScheduleDate;
                oCalendarDto.d_CircuitStartDate = null;
                oCalendarDto.d_EntryTimeCM = null;
                oCalendarDto.i_ServiceTypeId = (int)Enumeratores.masterService.AtxMedicaParticular;
                oCalendarDto.i_CalendarStatusId = (int)Enumeratores.CalendarStatus.Agendado;
                oCalendarDto.i_ServiceId = (int)Enumeratores.masterService.AtxMedicaParticular;
                oCalendarDto.i_NewContinuationId = (int)Enumeratores.Modality.NuevoServicio;
                oCalendarDto.i_LineStatusId = (int)Enumeratores.LineStatus.FueraCircuito;
                oCalendarDto.i_IsVipId = (int)Enumeratores.SiNo.No;

                var serviceId = new SchedulePersonDal().Schedule(oCalendarDto, nodeId, systemUserId);

                var oVigilanciaServiceDto = new VigilanciaServiceDto();
                oVigilanciaServiceDto.v_ServiceId = serviceId;
                oVigilanciaServiceDto.v_Commentary = oScheduleCustom.Commentary;
                oVigilanciaServiceDto.v_VigilanciaId = oScheduleCustom.VigilanciId;
                new VigilanciaDal().AddVigilanciaService(oVigilanciaServiceDto, nodeId, systemUserId);
                return serviceId;
            }
            
        }
        
    }
}
