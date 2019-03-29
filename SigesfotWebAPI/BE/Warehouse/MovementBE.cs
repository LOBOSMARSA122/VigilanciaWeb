using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Warehouse
{
    [Table("Movement")]
    public class MovementBE
    {
        [Key]
        public string v_MovementId { get; set; }
        public string v_WarehouseId { get; set; }
        public string v_SupplierId { get; set; }
        public int? i_ProcessTypeId { get; set; }
        public string v_ParentMovementId { get; set; }
        public string v_Motive { get; set; }
        public int? i_MotiveTypeId { get; set; }
        public DateTime? d_Date { get; set; }
        public float r_TotalQuantity { get; set; }
        public int? i_MovementTypeId { get; set; }
        public int? i_RequireRemoteProcess { get; set; }
        public string v_RemoteWarehouseId { get; set; }
        public int? i_CurrencyId { get; set; }
        public float r_ExchangeRate { get; set; }
        public string v_ReferenceDocument { get; set; }
        public int? i_CostCenterId { get; set; }
        public string v_Observations { get; set; }
        public int? i_IsLocallyProcessed { get; set; }
        public int? i_IsRemoteProcessed { get; set; }

        public int? i_InsertUserId { get; set; }
        //public int? i_IsDeleted { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public int? i_UpdateNodeId { get; set; }
    }
}
