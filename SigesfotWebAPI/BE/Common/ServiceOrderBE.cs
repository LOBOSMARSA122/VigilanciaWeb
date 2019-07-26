using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    [Table("serviceorder")]
    public class ServiceOrderBE
    {
        [Key]
        public string  v_ServiceOrderId { get; set; }

        public string  v_CustomServiceOrderId { get; set; }
        public string  v_Description { get; set; }
        public string  v_Comentary { get; set; }
        public int?  i_NumberOfWorker { get; set; }
        public float?  r_TotalCost { get; set; }
        public DateTime? d_DeliveryDate { get; set; }
        public int? i_ServiceOrderStatusId { get; set; }
        public int? i_LineaCreditoId { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime?  d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public int? i_MostrarPrecio { get; set; }
        public int? i_EsProtocoloEspecial { get; set; }
        public string  v_ComentaryUpdate { get; set; }
    }
}
