using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Component
{
    [Table("ComponentField")]
    public class ComponentFieldDto
    {
        [Key]
        public string v_ComponentFieldId { get; set; }
        public string v_TextLabel { get; set; }
        public int? i_LabelWidth { get; set; }
        public string v_Abbreviation { get; set; }
        public string v_DefaultText { get; set; }
        public int? i_ControlId { get; set; }
        public int? i_GroupId { get; set; }
        public int? i_ItemId { get; set; }
        public int? i_WidthControl { get; set; }
        public int? i_HeightControl { get; set; }
        public int? i_MaxLenght { get; set; }
        public int? i_IsRequired { get; set; }
        public int? i_IsCalculate { get; set; }
        public string v_Formula { get; set; }
        public int? i_Order { get; set; }
        public int? i_MeasurementUnitId { get; set; }
        public float? r_ValidateValue1 { get; set; }
        public float? r_ValidateValue2 { get; set; }
        public int? i_Column { get; set; }
        public int? i_defaultIndex { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public int? i_NroDecimales { get; set; }
        public int? i_ReadOnly { get; set; }
        public int? i_Enabled { get; set; }
    }
}
