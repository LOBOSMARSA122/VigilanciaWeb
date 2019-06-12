using BE.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Service
{
    public class ServiceCustom
    {
        public string ProtocolId { get; set; }
        public string ServiceId { get; set; }
        public string OrganizationId { get; set; }
        public string PersonId { get; set; }
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
}
