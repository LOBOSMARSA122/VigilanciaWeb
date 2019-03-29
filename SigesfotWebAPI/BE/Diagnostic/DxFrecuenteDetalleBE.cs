using System;
using System.ComponentModel.DataAnnotations;

namespace BE.Diagnostic
{
    public class DxFrecuenteDetalleBE
    {
        [Key]
        public string DxFrecuenteDetalleId { get; set; }

        public string DxFrecuenteId { get; set; }
        public string MasterRecommendationRestricctionId { get; set; }
        public int? IsDeleted { get; set; }
        public int? InsertUserId { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
