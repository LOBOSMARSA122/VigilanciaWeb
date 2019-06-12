using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Calendar;
using BE.Common;
using BE.Service;
using DAL.Common;
using static BE.Common.Enumeratores;

namespace DAL.Calendar
{
    public class CalendarDal
    {
        private static DatabaseContext ctx = new DatabaseContext();

        public bool AddCalendar(CalendarDto oCalendarDto, int nodeId, int systemUserId)
        {
            try
            {
                var calendarId = new Common.Utils().GetPrimaryKey(nodeId, 22, "CA");

                oCalendarDto.v_CalendarId = calendarId;

                oCalendarDto.i_IsDeleted = (int)Enumeratores.SiNo.No;
                oCalendarDto.d_InsertDate = DateTime.UtcNow;
                oCalendarDto.i_InsertUserId = systemUserId;

                ctx.Calendar.Add(oCalendarDto);
                return ctx.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        public BoardCalendar GetAllCalendar(BoardCalendar data)
        {
            try
            {


                data.FechaFin = data.FechaFin.Value.AddHours(24);
                BoardCalendar _BoardCalendar = new BoardCalendar();
                string filterPacient = data.PacientName == "" ? null : data.PacientName;
                string filterDocNumber = data.DocNumber == "" ? null : data.DocNumber;
                var List = (from cal in ctx.Calendar
                            join sys in ctx.SystemUser on cal.i_InsertUserId equals sys.i_SystemUserId
                            join per in ctx.Person on cal.v_PersonId equals per.v_PersonId
                            join ser in ctx.Service on cal.v_ServiceId equals ser.v_ServiceId
                            join org in ctx.Organization on new { a = ser.v_OrganizationId }
                                                              equals new { a = org.v_OrganizationId } into org_join
                            from org in org_join.DefaultIfEmpty()
                            join sysp3 in ctx.SystemParameter on new { a = cal.i_ServiceId.Value, b = 119 } equals new { a = sysp3.i_ParameterId, b = sysp3.i_GroupId }
                            join sysp4 in ctx.SystemParameter on new { a = cal.i_NewContinuationId.Value, b = 121 } equals new { a = sysp4.i_ParameterId, b = sysp4.i_GroupId }
                            join sysp5 in ctx.SystemParameter on new { a = cal.i_CalendarStatusId.Value, b = 122 } equals new { a = sysp5.i_ParameterId, b = sysp5.i_GroupId }
                            join pro in ctx.Protocol on new { a = ser.v_ProtocolId }
                                                              equals new { a = pro.v_ProtocolId } into pro_join
                            from pro in pro_join.DefaultIfEmpty()
                            join sysp7 in ctx.SystemParameter on new { a = pro.i_EsoTypeId.Value, b = 118 }
                                                              equals new { a = sysp7.i_ParameterId, b = sysp7.i_GroupId } into sysp7_join
                            from sysp7 in sysp7_join.DefaultIfEmpty()
                            join sysp9 in ctx.SystemParameter on new { a = ser.i_AptitudeStatusId.Value, b = 124 } equals new { a = sysp9.i_ParameterId, b = sysp9.i_GroupId }
                            join org2 in ctx.Organization on new { a = pro.v_CustomerOrganizationId }
                                                            equals new { a = org2.v_OrganizationId } into org2_join
                            from org2 in org2_join.DefaultIfEmpty()

                            join loc in ctx.Location on new { a = pro.v_CustomerOrganizationId, b = pro.v_CustomerLocationId }
                                                                equals new { a = loc.v_OrganizationId, b = loc.v_LocationId } into loc_join
                            from loc in loc_join.DefaultIfEmpty()

                            join gro in ctx.GroupOccupation on pro.v_GroupOccupationId equals gro.v_GroupOccupationId
                            join src in ctx.ServiceComponent on ser.v_ServiceId equals src.v_ServiceId
                            where cal.d_DateTimeCalendar.Value > data.FechaInicio.Value && cal.d_DateTimeCalendar.Value < data.FechaFin.Value
                            && (per.v_DocNumber.Contains(filterDocNumber) || filterDocNumber == null) && (cal.i_ServiceId == data.MasterService || data.MasterService == -1) &&
                            (cal.i_NewContinuationId == data.Modalidad || data.Modalidad == -1) && (cal.i_LineStatusId == data.Cola || data.Cola == -1)
                            && (cal.i_IsVipId == data.Vip || data.Vip == -1)
                            && (cal.i_CalendarStatusId == data.EstadoCita || data.EstadoCita == -1)
                            && (per.v_FirstName.Contains(filterPacient) || per.v_FirstLastName.Contains(filterPacient) || per.v_SecondLastName.Contains(filterPacient) || filterPacient == null)
                            && src.i_IsDeleted == (int)SiNo.No && cal.i_IsDeleted == (int)SiNo.No && src.i_IsRequiredId == (int)SiNo.Si && per.i_IsDeleted == (int)SiNo.No
                            select new CalendarCustom
                            {
                                v_Pacient = per.v_FirstLastName + "  " + per.v_SecondLastName + ", " + per.v_FirstName,
                                v_PersonId = per.v_PersonId,
                                v_CalendarId = cal.v_CalendarId,
                                v_ServiceId = ser.v_ServiceId,
                                d_DateTimeCalendar = cal.d_DateTimeCalendar,
                                v_DocNumber = per.v_DocNumber,
                                d_SalidaCM = cal.d_SalidaCM,
                                v_AptitudeStatusName = sysp9.v_Value1,
                                v_ServiceName = sysp3.v_Value1,
                                v_NewContinuationName = sysp4.v_Value1,
                                v_EsoTypeName = sysp7.v_Value1,
                                v_CalendarStatusName = sysp5.v_Value1,
                                v_CreationUser = sys.v_UserName,
                                v_OrganizationLocationProtocol = org2.v_Name + " / " + loc.v_Name,
                                v_WorkingOrganizationName = org2.v_Name,
                                d_EntryTimeCM = cal.d_EntryTimeCM,
                                d_Birthdate = per.d_Birthdate,
                                GESO = gro.v_Name,
                                v_ProtocolName = pro.v_Name,
                                Puesto = per.v_CurrentOccupation,
                                Nombres = per.v_FirstName,
                                ApeMaterno = per.v_SecondLastName,
                                ApePaterno = per.v_FirstLastName,
                                i_MasterServiceId = ser.i_MasterServiceId.Value,
                                //i_MedicoTratanteId = ser.i_MedicoTratanteId.Value,
                                v_ProtocolId = pro.v_ProtocolId,
                                RucEmpFact = org.v_IdentificationNumber,
                                i_ServiceTypeId = cal.i_ServiceTypeId.Value,
                            }).OrderBy(x => x.v_CalendarId).ToList();

                int skip = (data.Index - 1) * data.Take;

                var ListCalendar = List.GroupBy(g => g.v_CalendarId).Select(s => s.First()).ToList();
                data.TotalRecords = ListCalendar.Count;

                if (data.Take > 0)
                    ListCalendar = ListCalendar.Skip(skip).Take(data.Take).ToList();

                ListCalendar.ForEach(x => x.i_Edad = GetEdad(x.d_Birthdate.Value));
                data.List = ListCalendar;
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public CalendarDto GetCalendarByServiceId(string serviceId)
        {
            return ctx.Calendar.Where(x => x.v_ServiceId == serviceId).FirstOrDefault();
        }

        public bool UpdateCalendarForProtocol(ServiceCustom data, int UserId)
        {
            try
            {
                var objCalendar = ctx.Calendar.Where(x => x.v_ServiceId == data.ServiceId).FirstOrDefault();
                objCalendar.i_UpdateUserId = UserId;
                objCalendar.d_UpdateDate = DateTime.Now;
                objCalendar.i_ServiceTypeId = data.MasterServiceTypeId;
                objCalendar.i_ServiceId = data.MasterServiceId;

                return ctx.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int GetEdad(DateTime BirthDate)
        {
            int edad = DateTime.Today.AddTicks(-BirthDate.Ticks).Year - 1;
            return edad;
        }
    }
}
