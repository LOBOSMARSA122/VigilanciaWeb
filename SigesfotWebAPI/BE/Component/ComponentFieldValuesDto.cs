using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BE.Diagnostic;

namespace BE.Component
{
    [Table("ComponentFieldValues")]
    public class ComponentFieldValuesDto
    {
        [Key]
        public string v_ComponentFieldValuesId { get; set; }
        
        public string v_DiseasesId { get; set; }
        public DiseasesDto Diseases { get; set; }
        public string v_ComponentFieldId { get; set; }
        public string v_AnalyzingValue1 { get; set; }
        public string v_AnalyzingValue2 { get; set; }
        public int? i_OperatorId { get; set; }
        public string v_LegalStandard { get; set; }
        public int? i_IsAnormal { get; set; }
        public int? i_ValidationMonths { get; set; }
        public int? i_GenderId { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }
}
