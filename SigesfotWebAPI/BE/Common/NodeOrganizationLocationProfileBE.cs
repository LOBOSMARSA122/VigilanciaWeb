using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    [Table("NodeOrganizationLocationProfile")]
    public class NodeOrganizationLocationProfileBE
    {
        [Key, Column(Order = 1)]
        public int? i_NodeId { get; set; }

        [Key, Column(Order = 2)]
        public string v_OrganizationId { get; set; }

        [Key, Column(Order = 3)]
        public string v_LocationId { get; set; }

        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }
}
