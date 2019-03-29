using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Warehouse
{   
    [Table("product")]
    public class ProductBE
    {
        [Key]
        public string v_ProductId { get; set; }
        public int? i_CategoryId { get; set; }

        public string v_Name { get; set; }

        public string v_GenericName { get; set; }
        public string v_BarCode { get; set; }

        public string v_ProductCode { get; set; }

        public string v_Brand { get; set; }
        public string v_Model { get; set; }
        public string v_SerialNumber { get; set; }
        public DateTime? d_ExpirationDate { get; set; }
        public int? i_MeasurementUnitId { get; set; }
        public float r_ReferentialCostPrice { get; set; }
        public float r_ReferentialSalesPrice { get; set; }

        public string v_Presentation { get; set; }
        public string v_AdditionalInformation { get; set; }
        public byte [] b_Image { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }
}
