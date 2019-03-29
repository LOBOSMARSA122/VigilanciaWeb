using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Warehouse
{
    [Table("MovementDetail")]
    public class MovementDetailBE
    {
        [Key, Column(Order = 1)]
        public string v_MovementId { get; set; }
        [Key, Column(Order = 2)]
        public string v_ProductId { get; set; }
        [Key, Column(Order = 3)]
        public string v_WarehouseId { get; set; }

        public float r_StockMax { get; set; }
        public float r_StockMin { get; set; }
        public int? i_MovementTypeId { get; set; }
        public float r_Quantity { get; set; }
        public float r_Price { get; set; }
        public float r_SubTotal { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }
}
