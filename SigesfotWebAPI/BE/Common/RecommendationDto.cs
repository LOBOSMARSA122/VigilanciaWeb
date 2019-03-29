using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BE.Service;

namespace BE.Common
{
    [Table("recommendation")]
    public class RecommendationDto
    {
        [Key]
        public string v_RecommendationId { get; set; }

        public string v_ServiceId { get; set; }
        public ServiceDto service { get; set; }
        public string v_DiagnosticRepositoryId { get; set; }
        public DiagnosticRepositoryDto Diagnostic { get; set; }
        public string v_ComponentId { get; set; }
        public string v_MasterRecommendationId { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }
}
