using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Component
{
    [Table("componentFields")]
    public class ComponentFieldsDto
    {
        [Key, Column(Order = 1)]
        public string v_ComponentId { get; set; }
        [Key, Column(Order = 2)]
        public string v_ComponentFieldId { get; set; }
        public string v_Group { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }
}
