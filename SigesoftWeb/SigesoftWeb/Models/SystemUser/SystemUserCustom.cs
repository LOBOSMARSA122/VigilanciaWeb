using SigesoftWeb.Models.Pacient;
using SigesoftWeb.Models.Professional;
using SigesoftWeb.Models.ProtocolSystemUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Models.SystemUser
{

    public class BoardSystemUserExternal
    {
        public PacientCustom EntityPerson { get; set; }
        public ProfessionalCustom EntityProfessional { get; set; }
        public SystemUserCustom EntitySystemUser { get; set; }
        public List<ProtocolSystemUserCustom> ListEntityProtocolSystemUser { get; set; }
    }

    public class SystemUserCustom:PacientCustom
    {
        public int? i_SystemUserId { get; set; }
        public string v_UserName { get; set; }
        public string v_SecretQuestion { get; set; }
        public string v_SecretAnswer { get; set; }
        public DateTime? d_ExpireDate { get; set; }
        public string v_SystemUserByOrganizationId { get; set; }
        public int? i_SystemUserTypeId { get; set; }
        public int? i_RolVentaId { get; set; }
        public string v_PersonName { get; set; }
        public string v_ProfessionalCode { get; set; }
        public string v_ProfessionalInformation { get; set; }
        public int? i_ProfesionId { get; set; }
        public List<ProtocolSystemUserCustom> ListPremisos { get; set; }

    }
}