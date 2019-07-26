using BE.Pacient;
using BE.ProtocolSystemUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Security
{

    public class SystemUserCustom:PacientCustom
    {
        public int i_SystemUserId { get; set; }           
        //public string v_PersonId { get; set; }
        public int? i_NodeId { get; set; }
        public string v_UserName { get; set; }
        //public string v_Password { get; set; }
        public string v_SecretQuestion { get; set; }
        public string v_SecretAnswer { get; set; }
        //public int? i_IsDeleted { get; set; }
        //public int? i_InsertUserId { get; set; }
        //public DateTime? d_InsertDate { get; set; }
        //public int? i_UpdateUserId { get; set; }
        //public DateTime? d_UpdateDate { get; set; }
        public string v_InsertUser { get; set; }
        public string v_UpdateUser { get; set; }
        public string v_NodeName { get; set; }
        public string v_PersonName { get; set; }
        
        //public string v_DocNumber { get; set; }
        public DateTime? d_ExpireDate { get; set; }
        public string v_ProtocolId { get; set; }
        public string v_ProtocolSystemUserId { get; set; }
        public string v_RolVenta { get; set; }
        public int? i_RolVenta { get; set; }
        public int? i_RolVentaId { get; set; }
        public int? i_ProfesionId { get; set; }
        public string Profesion { get; set; }
        public string MedicoTratante { get; set; }
        public string Direccion { get; set; }
        public string CMP { get; set; }
        public string Telefono { get; set; }
        public string InfAdicional { get; set; }
        public string v_ProfessionalCode { get; set; }
        public string v_ProfessionalInformation { get; set; }

        public List<ProtocolSystemUserCustom> ListPremisos { get; set; }

    }
}
