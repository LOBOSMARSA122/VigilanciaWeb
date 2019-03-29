using SigesoftWeb.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Models.Component
{
    public class AdditionalExams
    {
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ComponentAdditional> Components { get; set; }
    }

    public class ComponentAdditional
    {
        public string ComponenId { get; set; }
        public string ComponentName { get; set; }
        public int CategoryId { get; set; }
        public List<FieldAdditional> fields { get; set; }
    }

    public class FieldAdditional
    {
        public string ComponentFieldId { get; set; }
        public string ComponentId { get; set; }

        public string TextLabel { get; set; }
        public int? LabelWidth { get; set; }
        public string abbreviation { get; set; }
        public string DefaultText { get; set; }
        public int? ControlId { get; set; }
        public int? GroupId { get; set; }
        public int? ItemId { get; set; }
        public int? WidthControl { get; set; }
        public int? HeightControl { get; set; }
        public int? MaxLenght { get; set; }
        public int? IsRequired { get; set; }
        public int? IsCalculate { get; set; }
        public string Formula { get; set; }
        public List<Formulate> FormulaList { get; set; }
        public List<TargetFieldOfCalculate> TargetFieldOfCalculateId { get; set; }
        public int Order { get; set; }
        public int? MeasurementUnitId { get; set; }
        public float ValidateValue1 { get; set; }
        public float ValidateValue2 { get; set; }
        public int? Column { get; set; }
        public string defaultIndex { get; set; }
        public int? NroDecimales { get; set; }
        public int? ReadOnly { get; set; }
        public int? Enabled { get; set; }
        public string Group { get; set; }
        public int IsSourceFieldToCalculate { get; set; }
        public List<KeyValueDTO> ComboValues { get; set; }
    }

    public class Formulate
    {
        public string v_Formula { get; set; }
        public string v_TargetFieldOfCalculateId { get; set; }
    }

    public class TargetFieldOfCalculate
    {
        public string v_TargetFieldOfCalculateId { get; set; }
    }
}