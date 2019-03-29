using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    [Table("Supplier")]
    public class SupplierBE
    {
        [Key]
        public string v_SupplierId { get; set; }
        public int? i_SectorTypeId { get; set; }
        public string v_IdentificationNumber { get; set; }
        public string v_Name { get; set; }
        public string v_Address { get; set; }
        public string v_PhoneNumber { get; set; }
        public string v_Mail { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public int? i_UpdateNodeId { get; set; }
    }
}
