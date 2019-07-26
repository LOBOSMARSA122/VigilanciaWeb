using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    [Table("serviceorderdetail")]
    public class ServiceOrderDetailBE
    {
        [Key]
        public string v_ServiceOrderDetailId { get; set; }

        public string v_ServiceOrderId { get; set; }
        public string v_ProtocolId { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public float? r_ProtocolPrice { get; set; }
        public int? i_NumberOfWorkerProtocol { get; set; }
        public float? r_Total { get; set; }
        public string v_ComentaryUpdate { get; set; }
    }
}
