using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Z_CommonSAMBHS
{
    [Table("systemuser")]
    public class SystemUserBE
    {
        [Key]
        public int i_SystemUserId { get; set; }

        public int? i_PersonId { get; set; }
        public int? i_RoleId { get; set; }
        public string v_UserName { get; set; }
        public string v_Password { get; set; }
        public string v_SecretQuestion { get; set; }
        public string v_SecretAnswer { get; set; }
        public string v_CodeBar { get; set; }
        public int? i_UsuarioContable { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }
}
