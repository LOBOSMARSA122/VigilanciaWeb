using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Component
{
    [Table("component")]
    public class ComponentDto
    {
        [Key]
        public string v_ComponentId { get; set; }
        public string v_Name { get; set; }
        public int i_CategoryId { get; set; }
        public float r_BasePrice { get; set; }
        public int? i_DiagnosableId { get; set; }
        public int? i_IsApprovedId { get; set; }
        public int? i_ComponentTypeId { get; set; }
        public int? i_UIIsVisibleId { get; set; }
        public int? i_UIIndex { get; set; }
        public int? i_ValidInDays { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public string v_IdUnidadProductiva { get; set; }
        public string v_CodigoSegus { get; set; }   
        public int? i_PriceIsRecharged { get; set; }
    }
}
