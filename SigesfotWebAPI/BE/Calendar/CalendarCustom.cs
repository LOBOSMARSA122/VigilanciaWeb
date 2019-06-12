using BE.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Calendar
{
    public class Boards
    {
        public int TotalRecords { get; set; }
        public int Index { get; set; }
        public int Take { get; set; }
    }

    public class BoardCalendar : Boards
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string DocNumber { get; set; }
        public string PacientName { get; set; }
        public int? ServiceType { get; set; }
        public int? MasterService { get; set; }
        public int? Modalidad { get; set; }
        public int? Cola { get; set; }
        public int? Vip { get; set; }
        public int? EstadoCita { get; set; }
        public List<CalendarCustom> List { get; set; }

    }
    public class CalendarCustom
    {
        public int i_ServiceTypeId { get; set; }
        public string v_ServiceId { get; set; }
        public string v_CalendarId { get; set; }
        public DateTime? d_DateTimeCalendar { get; set; }
        public string v_Pacient { get; set; }
        public string v_DocNumber { get; set; }
        public string v_LineStatusName { get; set; }
        public string v_ServiceStatusName { get; set; }
        public DateTime? d_SalidaCM { get; set; }
        public string v_AptitudeStatusName { get; set; }
        public string v_ServiceTypeName { get; set; }
        public string v_ServiceName { get; set; }
        public string v_NewContinuationName { get; set; }
        public string v_EsoTypeName { get; set; }
        public string v_CalendarStatusName { get; set; }
        public string v_ProtocolName { get; set; }
        public string v_IsVipName { get; set; }
        public string v_OrganizationLocationProtocol { get; set; }
        public string v_OrganizationLocationService { get; set; }
        public DateTime? d_EntryTimeCM { get; set; }
        public bool b_Seleccionar { get; set; }
        public int i_Edad { get; set; }
        public string GESO { get; set; }
        public string Puesto { get; set; }
        public string Nombres { get; set; }
        public string ApePaterno { get; set; }
        public string ApeMaterno { get; set; }
        public byte[] HuellaTrabajador { get; set; }
        public byte[] FirmaTrabajador { get; set; }
        public string v_WorkingOrganizationName { get; set; }
        public string v_ProtocolId { get; set; }
        public byte[] FotoTrabajador { get; set; }
        public DateTime? d_Birthdate { get; set; }
        public float? PrecioTotalProtocolo { get; set; }
        public string v_PersonId { get; set; }
        public string v_NumberDocument { get; set; }
        public int i_MasterServiceId { get; set; }
        public int i_MedicoTratanteId { get; set; }
        public string v_OrganizationId { get; set; }
        public string RucEmpFact { get; set; }
        public string v_CreationUser { get; set; }
    }
}
