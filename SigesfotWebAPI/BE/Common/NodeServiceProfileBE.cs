using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    [Table("nodeserviceprofile")]
    public class NodeServiceProfileBE
    {
        [Key]
        public string v_NodeServiceProfileId { get; set; }

        public int? i_NodeId { get; set; }
        public int? i_ServiceTypeId { get; set; }
        public int? i_MasterServiceId { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public string v_ComentaryUpdate { get; set; }
    }
}
