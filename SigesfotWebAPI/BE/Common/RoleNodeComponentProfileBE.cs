using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    [Table("rolenodecomponentprofile")]
    public class RoleNodeComponentProfileBE
    {
        [Key]
        public string v_RoleNodeComponentId { get; set; }

        public int? i_NodeId { get; set; }
        public int? i_RoleId { get; set; }
        public string v_ComponentId { get; set; }
        public int? i_Read { get; set; }
        public int? i_Write { get; set; }
        public int? i_Dx { get; set; }
        public int? i_Approved { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public string v_ComentaryUpdate { get; set; }
    }
}
