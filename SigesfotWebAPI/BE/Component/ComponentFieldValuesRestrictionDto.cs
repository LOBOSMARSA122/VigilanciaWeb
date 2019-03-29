using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Component
{
    [Table("componentfieldvaluesrestriction")]
    public class ComponentFieldValuesRestrictionDto
    {
        [Key]
        public string v_ComponentFieldValuesRestrictionId { get; set; } 

        public string v_ComponentFieldValuesId { get; set; } 
        public string v_MasterRecommendationRestricctionId { get; set; } 
        public int? i_IsDeleted { get; set; } 
        public int? i_InsertUserId { get; set; } 
        public DateTime? d_InsertDate { get; set; } 
        public int? i_UpdateUserId { get; set; } 
        public DateTime? d_UpdateDate { get; set; } 
    }
}
