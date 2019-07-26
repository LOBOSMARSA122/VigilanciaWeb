using BE.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Service
{
    public class Boards
    {
        public int TotalRecords { get; set; }
        public int Index { get; set; }
        public int Take { get; set; }
    }

    public class BoardServiceCustomList : Boards
    {
        public string Pacient { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public int TipoServicio { get; set; }
        public int Servicio { get; set; }
        public string OrganizationId { get; set; }
        public string ProtocolId { get; set; }
        public string ServiceId { get; set; }
        public int EstadoServicio { get; set; }
        public int UsuarioMedico { get; set; }

        public List<ServiceCustomList> List { get; set; }
    }

    public class ServiceCustom
    {
        public string ProtocolId { get; set; }
        public string ServiceId { get; set; }
        public string OrganizationId { get; set; }
        public string PersonId { get; set; }
        public DateTime FechaCalendario { get; set; }
        public int? MasterServiceId { get; set; }
        public int? MasterServiceTypeId { get; set; }
        public int? ServiceStatusId { get; set; }
        public int? AptitudeStatusId { get; set; }
        public int? FlagAgentId { get; set; }
        public string Motive { get; set; }
        public int? IsFac { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int? GeneroId { get; set; }
        public int? MedicoTratanteId { get; set; }
        public string CentroCosto { get; set; }
        public ProtocolCustom DataProtocol { get; set; }
    }

    public class ServiceCustomList
    {
        public bool b_FechaEntrega { get; set; }
        public DateTime? d_FechaEntrega { get; set; }
        public string v_ServiceId { get; set; }
        public string v_Pacient { get; set; }
        public string v_PersonId { get; set; }
        public string v_ServiceStatusName { get; set; }
        public int? i_ServiceStatusId { get; set; }
        public string v_AptitudeStatusName { get; set; }
        public string v_OrganizationName { get; set; }
        public string v_LocationName { get; set; }
        public string v_ProtocolId { get; set; }
        public string v_ProtocolName { get; set; }
        public string v_ComponentId { get; set; }
        public int i_LineStatusId { get; set; }
        public string v_DiagnosticRepositoryId { get; set; }
        public string v_DiseasesName { get; set; }
        public DateTime? d_ExpirationDateDiagnostic { get; set; }
        public string v_Recommendation { get; set; }
        public int? i_ServiceId { get; set; }
        public DateTime? d_ServiceDate { get; set; }
        public string v_PacientDocument { get; set; }
        public int? i_ServiceTypeId { get; set; }
        public string v_CustomerOrganizationId { get; set; }
        public string v_CustomerLocationId { get; set; }
        public int? i_MasterServiceId { get; set; }
        public int? i_AptitudeStatusId { get; set; }
        public int? i_EsoTypeId { get; set; }
        public int? i_IsDeleted { get; set; }
        public string v_CreationUser { get; set; }
        public string v_UpdateUser { get; set; }
        public DateTime? d_CreationDate { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public int? i_StatusLiquidation { get; set; }
        public object Liq { get; set; }
        public string v_MasterServiceName { get; set; }
        public string v_EsoTypeName { get; set; }
        public string CIE10 { get; set; }
        public DateTime? d_FechaNacimiento { get; set; }
        public string NroPoliza { get; set; }
        public string Moneda { get; set; }
        public string NroFactura { get; set; }
        public decimal? Valor { get; set; }
        public int? i_FinalQualificationId { get; set; }
        public string v_Restriccion { get; set; }
        public decimal? d_Deducible { get; set; }
        public int? i_IsDeletedDx { get; set; }
        public byte[] LogoEmpresaPropietaria { get; set; }

        public int? i_IsDeletedRecomendaciones { get; set; }
        public int? i_IsDeletedRestricciones { get; set; }

        public int i_age { get; set; }
        public DateTime? d_BirthDate { get; set; }

        public string UsuarioMedicina { get; set; }

        public string CompMinera { get; set; }
        public string Tercero { get; set; }
        public int item { get; set; }
        public int? i_ApprovedUpdateUserId { get; set; }
        public string v_DocNumber { get; set; }

        public string UsuarioCrea { get; set; }
        public string TipoServicio { get; set; }
        public string TipoServicioMaster { get; set; }
        public string TipoServicioESO { get; set; }
        public string v_MedicoTratante { get; set; }

        public string Fecha { get; set; }
    }
}
