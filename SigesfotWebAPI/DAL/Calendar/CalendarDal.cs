using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BE.Calendar;
using BE.Common;
using BE.Component;
using BE.Service;
using DAL.AdditionalExam;
using DAL.Common;
using DAL.Hospitalizacion;
using DAL.Service;
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

                oCalendarDto.i_IsDeleted = (int)SiNo.No;
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
                                PrecioTotalProtocolo = src.r_Price,
                                v_ProtocolName = pro.v_Name,
                                Puesto = per.v_CurrentOccupation,
                                Nombres = per.v_FirstName,
                                ApeMaterno = per.v_SecondLastName,
                                ApePaterno = per.v_FirstLastName,
                                i_MasterServiceId = ser.i_MasterServiceId.Value,
                                //i_MedicoTratanteId = ser.i_MedicoTratanteId.Value,
                                v_ProtocolId = pro.v_ProtocolId,
                                v_OrganizationId = org.v_OrganizationId,
                                RucEmpFact = org.v_IdentificationNumber,
                                i_ServiceTypeId = cal.i_ServiceTypeId.Value,
                            }).OrderBy(x => x.v_CalendarId).ToList();

                int skip = (data.Index - 1) * data.Take;
                var results = (from p in List
                              group p by p.v_CalendarId into grupo                       
                              select new {
                                  Precio = grupo.Sum(x => x.PrecioTotalProtocolo),
                                  Id = grupo.Key
                              }).ToList();


                var ListCalendar = List.GroupBy(g => g.v_CalendarId).Select(s => s.First()).ToList();

                foreach (var obj in results)
                {
                    ListCalendar.Find(x => x.v_CalendarId == obj.Id).PrecioTotalProtocolo = obj.Precio;
                }

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

        public List<CalendarList> GetPacientInLineByComponentId(DateTime CurrentDate, List<string> pobjComponentIds, int masterServiceId)
        {

            int isDeleted = (int)SiNo.No;
            int lineStatus = (int)LineStatus.EnCircuito;
            int isRequired = (int)SiNo.Si;

            try
            {
                DatabaseContext dbContext = new DatabaseContext();

                var query = (from A in dbContext.Calendar
                            join B in dbContext.Person on A.v_PersonId equals B.v_PersonId
                            join C in dbContext.Service on A.v_ServiceId equals C.v_ServiceId
                            join D in dbContext.ServiceComponent on A.v_ServiceId equals D.v_ServiceId
                            //where pobjComponentIds.Contains(D.v_ComponentId)
                            //orderby D.v_ServiceComponentId
                            join E in dbContext.SystemParameter on new { a = C.i_ServiceStatusId.Value, b = 125 } equals new { a = E.i_ParameterId, b = E.i_GroupId }
                            join G in dbContext.SystemParameter on new { a = A.i_ServiceTypeId.Value, b = 119 } equals new { a = G.i_ParameterId, b = G.i_GroupId }
                            join H in dbContext.SystemParameter on new { a = A.i_ServiceId.Value, b = 119 } equals new { a = H.i_ParameterId, b = H.i_GroupId }
                            join I in dbContext.SystemParameter on new { a = A.i_IsVipId.Value, b = 111 } equals new { a = I.i_ParameterId, b = I.i_GroupId }

                            join J in dbContext.Protocol on C.v_ProtocolId equals J.v_ProtocolId into J_join
                            from J in J_join.DefaultIfEmpty()

                            join K in dbContext.SystemParameter on new { a = J.i_EsoTypeId.Value, b = 118 }
                                         equals new { a = K.i_ParameterId, b = K.i_GroupId } into K_join
                            from K in K_join.DefaultIfEmpty()

                            join L in dbContext.Component on D.v_ComponentId equals L.v_ComponentId into L_join
                            from L in L_join.DefaultIfEmpty()


                            join P in dbContext.SystemParameter on new { a = 116, b = L.i_CategoryId }
                            equals new { a = P.i_GroupId, b = P.i_ParameterId } //into P_join


                                //************************************************************************************

                            where A.i_IsDeleted == isDeleted &&
                                  A.i_LineStatusId == lineStatus &&
                                  D.i_IsRequiredId == isRequired &&
                                  C.i_MasterServiceId == masterServiceId

                            select new CalendarList
                            {
                                v_CalendarId = A.v_CalendarId,
                                v_PersonId = A.v_PersonId,
                                v_Pacient = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                v_ServiceId = A.v_ServiceId,
                                v_ServiceStatusName = E.v_Value1,
                                v_ServiceTypeName = G.v_Value1,
                                v_ServiceName = H.v_Value1,
                                v_EsoTypeName = K.v_Value1,
                                v_ProtocolName = J.v_Name,
                                d_ServiceDate = C.d_ServiceDate.Value,
                                v_ServiceComponentId = D.v_ServiceComponentId,
                                i_IsVipId = A.i_IsVipId.Value,
                                v_IsVipName = I.v_Value1,
                                d_Birthdate = B.d_Birthdate.Value,
                                i_ServiceStatusId = C.i_ServiceStatusId.Value,
                                d_DateTimeCalendar = A.d_DateTimeCalendar.Value,
                                v_DocNumber = B.v_DocNumber,
                                i_CategoryId = L.i_CategoryId,
                                v_ProtocolId = J.v_ProtocolId,
                                i_ServiceId = A.i_ServiceId.Value,
                                v_ComponentId = D.v_ComponentId,
                                i_QueueStatusId = D.i_QueueStatusId.Value,
                                Piso = P.v_Value2
                            }).ToList();
                var queryFinal = query.Where(x => x.d_DateTimeCalendar.Value.Date == CurrentDate.Date)
                    .GroupBy(x => x.v_PersonId)
                    .Select(group => group.First()).ToList();

                queryFinal.ForEach(x => x.i_Edad = GetEdad(x.d_Birthdate));
                List<CalendarList> objData = queryFinal.ToList();

                return objData;

            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public List<CalendarList> GetPacientInLineByComponentId_ATX(DateTime CurrentDate, List<string> pobjComponentIds, int masterServiceId, int UsuerId)
        {

            int isDeleted = (int)SiNo.No;
            int lineStatus = (int)LineStatus.EnCircuito;
            int isRequired = (int)SiNo.Si;

            try
            {
                DatabaseContext dbContext = new DatabaseContext();

                var query = (from A in dbContext.Calendar
                            join B in dbContext.Person on A.v_PersonId equals B.v_PersonId
                            join C in dbContext.Service on A.v_ServiceId equals C.v_ServiceId
                            join D in dbContext.ServiceComponent on A.v_ServiceId equals D.v_ServiceId
                            where pobjComponentIds.Contains(D.v_ComponentId)
                            //orderby D.v_ServiceComponentId
                            join E in dbContext.SystemParameter on new { a = C.i_ServiceStatusId.Value, b = 125 } equals new { a = E.i_ParameterId, b = E.i_GroupId }
                            join G in dbContext.SystemParameter on new { a = A.i_ServiceTypeId.Value, b = 119 } equals new { a = G.i_ParameterId, b = G.i_GroupId }
                            join H in dbContext.SystemParameter on new { a = A.i_ServiceId.Value, b = 119 } equals new { a = H.i_ParameterId, b = H.i_GroupId }
                            join I in dbContext.SystemParameter on new { a = A.i_IsVipId.Value, b = 111 } equals new { a = I.i_ParameterId, b = I.i_GroupId }

                            join J in dbContext.Protocol on C.v_ProtocolId equals J.v_ProtocolId into J_join
                            from J in J_join.DefaultIfEmpty()

                            join K in dbContext.SystemParameter on new { a = J.i_EsoTypeId.Value, b = 118 }
                                         equals new { a = K.i_ParameterId, b = K.i_GroupId } into K_join
                            from K in K_join.DefaultIfEmpty()

                            join L in dbContext.Component on D.v_ComponentId equals L.v_ComponentId into L_join
                            from L in L_join.DefaultIfEmpty()


                            join P in dbContext.SystemParameter on new { a = 116, b = L.i_CategoryId }
                            equals new { a = P.i_GroupId, b = P.i_ParameterId } //into P_join

                           // Empresa / Sede Trabajo  ********************************************************
                            join ow in dbContext.Organization on J.v_WorkingOrganizationId equals ow.v_OrganizationId into ow_join
                            from ow in ow_join.DefaultIfEmpty()

                            join lw in dbContext.Location on new { a = J.v_WorkingOrganizationId, b = J.v_WorkingLocationId }
                                 equals new { a = lw.v_OrganizationId, b = lw.v_LocationId } into lw_join
                            from lw in lw_join.DefaultIfEmpty()

                                //************************************************************************************

                            where A.i_IsDeleted == isDeleted &&
                                  A.i_LineStatusId == lineStatus &&
                                  D.i_IsRequiredId == isRequired &&
                                  C.i_MasterServiceId == masterServiceId &&
                                  D.i_MedicoTratanteId == UsuerId

                            select new CalendarList
                            {
                                v_CalendarId = A.v_CalendarId,
                                v_PersonId = A.v_PersonId,
                                v_Pacient = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                v_ServiceId = A.v_ServiceId,
                                v_ServiceStatusName = E.v_Value1,
                                v_ServiceTypeName = G.v_Value1,
                                v_ServiceName = H.v_Value1,
                                v_EsoTypeName = K.v_Value1,
                                v_ProtocolName = J.v_Name,
                                d_ServiceDate = C.d_ServiceDate.Value,
                                v_ServiceComponentId = D.v_ServiceComponentId,
                                i_IsVipId = A.i_IsVipId.Value,
                                v_IsVipName = I.v_Value1,
                                d_Birthdate = B.d_Birthdate.Value,
                                i_ServiceStatusId = C.i_ServiceStatusId.Value,
                                d_DateTimeCalendar = A.d_DateTimeCalendar.Value,
                                v_DocNumber = B.v_DocNumber,
                                i_CategoryId = L.i_CategoryId,
                                v_ProtocolId = J.v_ProtocolId,
                                i_ServiceId = A.i_ServiceId.Value,
                                v_ComponentId = D.v_ComponentId,
                                v_WorkingOrganizationName = ow.v_Name,
                                Piso = P.v_Value2
                            }).ToList();

                var queryFinal = query.Where(x => x.d_DateTimeCalendar.Value.Date == CurrentDate)
                    .GroupBy(x => x.v_PersonId)
                    .Select(group => group.First()).ToList();

                queryFinal.ForEach(x => x.i_Edad = GetEdad(x.d_Birthdate));
                List<CalendarList> objData = queryFinal.ToList();

                return objData;

            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public bool CircuitStart(string pstrCalendarId, int userId)
        {
            try
            {
                using (var ts = new TransactionScope())
                {
                    var objCalendarDto = ctx.Calendar.Where(x => x.v_CalendarId == pstrCalendarId).FirstOrDefault();
                    var serviceId = objCalendarDto.v_ServiceId;
                    objCalendarDto.v_CalendarId = pstrCalendarId;
                    objCalendarDto.i_LineStatusId = (int)LineStatus.EnCircuito;
                    objCalendarDto.i_CalendarStatusId = (int)CalendarStatus.Atendido;
                    objCalendarDto.d_CircuitStartDate = DateTime.Now;
                    objCalendarDto.d_UpdateDate = DateTime.Now;
                    objCalendarDto.i_UpdateUserId = userId;

                    ctx.SaveChanges();

                    var result = new ServiceDal().UpdateServiceForCalendar(DateTime.Now, userId, serviceId);
                    if (!result) throw new Exception("");
                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SaveAdditionalExamsForCalendar(List<AdditionalExamCreate> data, int userId, int nodeId)
        {
            try
            {
                DatabaseContext dbContext = new DatabaseContext();
                using (var ts = new TransactionScope())
                {
                    foreach (var objExam in data)
                    {
                        if (objExam.IsNewService == (int)SiNo.No)
                        {
                            var unidadProductiva = GetMedicalExam(objExam.ComponentId);
                            var newId = new Common.Utils().GetPrimaryKey(nodeId, 24, "SC"); ;
                            ServiceComponentDto objServiceComponentDto = new ServiceComponentDto();
                            objServiceComponentDto.v_ServiceComponentId = newId;
                            objServiceComponentDto.i_ConCargoA = -1;
                            objServiceComponentDto.v_ServiceId = objExam.ServiceId;
                            objServiceComponentDto.i_ExternalInternalId = (int)ComponenteProcedencia.Interno;
                            objServiceComponentDto.i_ServiceComponentTypeId = 1;
                            objServiceComponentDto.i_IsVisibleId = 1;
                            objServiceComponentDto.i_IsInheritedId = (int)SiNo.No;
                            objServiceComponentDto.d_StartDate = null;
                            objServiceComponentDto.d_EndDate = null;
                            objServiceComponentDto.i_index = 1;
                            objServiceComponentDto.r_Price = objExam.Price;
                            objServiceComponentDto.v_ComponentId = objExam.ComponentId;
                            objServiceComponentDto.i_IsInvoicedId = (int)SiNo.No;
                            objServiceComponentDto.i_ServiceComponentStatusId = (int)ServiceStatus.PorIniciar;
                            objServiceComponentDto.i_QueueStatusId = (int)QueueStatusId.Libre;
                            objServiceComponentDto.i_Iscalling = (int)Flag_Call.NoseLlamo;
                            objServiceComponentDto.i_Iscalling_1 = (int)Flag_Call.NoseLlamo;
                            objServiceComponentDto.i_IsManuallyAddedId = (int)SiNo.No;
                            objServiceComponentDto.i_IsRequiredId = (int)SiNo.Si;
                            objServiceComponentDto.v_IdUnidadProductiva = unidadProductiva;
                            objServiceComponentDto.i_MedicoTratanteId = objExam.MedicoTratante;
                            objServiceComponentDto.d_SaldoPaciente = 0;
                            objServiceComponentDto.d_SaldoAseguradora = 0;
                            objServiceComponentDto.i_IsDeleted = 0;
                            objServiceComponentDto.d_InsertDate = DateTime.Now;
                            dbContext.ServiceComponent.Add(objServiceComponentDto);
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            CalendarDto _CalendarDto = new CalendarDto();
                            var protocolId = Constants.Prot_Hospi_Adic;
                            _CalendarDto.v_PersonId = objExam.PersonId;
                            _CalendarDto.d_DateTimeCalendar = DateTime.Now;
                            _CalendarDto.d_CircuitStartDate = DateTime.Now;
                            _CalendarDto.d_EntryTimeCM = DateTime.Now;
                            _CalendarDto.i_ServiceTypeId = (int)ServiceType.Particular;
                            _CalendarDto.i_ServiceId = (int)MasterService.Hospitalizacion;

                            _CalendarDto.i_CalendarStatusId = (int)CalendarStatus.Agendado;
                            _CalendarDto.i_LineStatusId = (int)LineStatus.EnCircuito;
                            _CalendarDto.v_ProtocolId = protocolId;
                            _CalendarDto.i_NewContinuationId = 1;
                            _CalendarDto.i_LineStatusId = (int)LineStatus.EnCircuito;
                            _CalendarDto.i_IsVipId = (int)SiNo.No;

                            var result = AddCalendar(_CalendarDto, nodeId, userId);

                            if (result)
                            {
                                ServiceDto _ServiceDto = new ServiceDto();

                                _ServiceDto = ctx.Service.Where(x => x.v_ServiceId == objExam.ServiceId).FirstOrDefault();
                                _ServiceDto.d_ServiceDate = DateTime.Now;
                                _ServiceDto.i_ServiceStatusId = (int)ServiceStatus.Iniciado;
                                _ServiceDto.d_UpdateDate = DateTime.Now;
                                _ServiceDto.i_UpdateUserId = userId;
                                ctx.SaveChanges();

                                var servicesComponents = new ServiceComponentDal().GetServiceComponents(objExam.ServiceId);
                                if (servicesComponents != null)
                                {
                                    foreach (var servicesComponent in servicesComponents)
                                    {

                                        var oservicecomponentDto = ctx.ServiceComponent.Where(x => x.v_ServiceComponentId == servicesComponent.v_ServiceComponentId).FirstOrDefault();
                                        oservicecomponentDto.i_MedicoTratanteId = 11;
                                        oservicecomponentDto.i_IsVisibleId = 1;
                                        oservicecomponentDto.v_ServiceComponentId = servicesComponent.v_ServiceComponentId;
                                        oservicecomponentDto.i_UpdateUserId = userId;
                                        oservicecomponentDto.d_UpdateDate = DateTime.Now;
                                        ctx.SaveChanges();
                                    }

                                    var resultHospi = new HospitalizacionDal().AddHospitalizacionService(null, objExam.ServiceId, nodeId, userId);
                                    if (!resultHospi) throw new Exception("");
                                }
                                else
                                {
                                    throw new Exception("");
                                }

                            }
                            else
                            {
                                throw new Exception("");
                            }
                        }

                    }

                    var resultAdditional = new AdditionalExamDal().UpdateAdditionalExamByComponentIdAndServiceId(data, userId);
                    if (!resultAdditional) throw new Exception("");
                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        

        public string GetMedicalExam(string componentId)
        {
            //mon.IsActive = true;

            try
            {
                DatabaseContext dbContext = new DatabaseContext();


                var objEntity = (from a in dbContext.Component
                                 where a.v_ComponentId == componentId
                                 select a).FirstOrDefault();

                return objEntity.v_IdUnidadProductiva;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
