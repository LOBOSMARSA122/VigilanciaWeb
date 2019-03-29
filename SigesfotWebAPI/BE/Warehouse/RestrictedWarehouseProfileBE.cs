using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Warehouse
{
    [Table("RestrictedWarehouseProfile")]
    public class RestrictedWarehouseProfileBE
    {
        [Key, Column(Order = 1)]
        public int? i_SystemUserId { get; set; }
        [Key, Column(Order = 2)]
        public string v_WarehouseId { get; set; }
        [Key, Column(Order = 3)]
        public int? i_NodeId { get; set; }

        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }
}
