using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Warehouse
{
    [Table("warehouse")]
    public class WarehouseBE
    {
        [Key]
        public string v_WarehouseId { get; set; }

        public string v_OrganizationId { get; set; }
        public string v_LocationId { get; set; }
        public string v_Name { get; set; }
        public string v_AdditionalInformation { get; set; }
        public int? i_CostCenterId { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }
}
