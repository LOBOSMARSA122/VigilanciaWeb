using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Protocol
{
    [Table("protocolsystemuser")]
    public class ProtocolSystemUserBE
    {
        [Key]
        public string v_ProtocolSystemUserId { get; set; }

        public int? i_SystemUserId { get; set; }
        public string v_ProtocolId { get; set; }
        public int? i_ApplicationHierarchyId { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }
}
