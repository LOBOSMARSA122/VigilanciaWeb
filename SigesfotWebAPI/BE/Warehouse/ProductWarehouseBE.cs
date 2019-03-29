using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Warehouse
{
    [Table("ProductWarehouse")]
    public class ProductWarehouseBE
    {
        [Key, Column(Order = 1)]
        public string v_WarehouseId { get; set; }
        [Key, Column(Order = 2)]
        public string v_ProductId { get; set; }

        public float r_StockMin { get; set; }
        public float r_StockMax { get; set; }
        public float r_StockActual { get; set; }

        //#region Creado
        //public int? IsDeleted { get; set; }
        //#endregion
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }
}
