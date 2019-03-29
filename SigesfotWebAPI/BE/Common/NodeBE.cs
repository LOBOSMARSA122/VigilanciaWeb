using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    [Table("Node")]
    public class NodeBE
    {
        [Key]
        public int? i_NodeId { get; set; }
        public string v_Description { get; set; }
        public string v_GeografyLocationId { get; set; }
        public string v_GeografyLocationDescription { get; set; }
        public int? i_NodeTypeId { get; set; }
        public DateTime? d_BeginDate { get; set; }
        public DateTime? d_EndDate { get; set; }
        public string v_PharmacyWarehouseId { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }
}
