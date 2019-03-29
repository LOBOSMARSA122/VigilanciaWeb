using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using BE.Component;

namespace DAL.Component
{
    public class ComponentDal
    {
        private DatabaseContext ctx = new DatabaseContext();

        public List<AdditionalExams> ListOfAdditionalExams()
        {
            var list = new List<AdditionalExams>();

            var components = (from a in ctx.Component where a.i_IsDeleted == (int)Enumeratores.SiNo.No select a).ToList();

            var componentFields = (from a in ctx.ComponentFields where a.i_IsDeleted == (int)Enumeratores.SiNo.No select a).ToList();

            var componentField = (from a in ctx.ComponentField where a.i_IsDeleted == (int)Enumeratores.SiNo.No select a).ToList();
            
            var systemParameter = (from a in ctx.SystemParameter where a.i_IsDeleted == (int)Enumeratores.SiNo.No select a).ToList();

            var categories = (from a in components
                              join b in ctx.SystemParameter on new { a = a.i_CategoryId, b = 116 }
                                equals new { a = b.i_ParameterId, b = b.i_GroupId }
                                where a.i_IsDeleted == (int) Enumeratores.SiNo.No && (b.i_ParameterId == 1 || b.i_ParameterId == 10 || b.i_ParameterId == 11)
                            select new
                            {
                                CategoryId = a.i_CategoryId,
                                CategoryName = b.v_Value1
                            }).ToList().GroupBy(g => g.CategoryId).Select(s => s.First()).ToList();
           
            foreach (var category in categories)
            {
                var oAdditionalExams = new AdditionalExams();

                oAdditionalExams.CategoryId = category.CategoryId;
                oAdditionalExams.CategoryName = category.CategoryName;
                oAdditionalExams.Components = (from a in components
                                               where a.i_CategoryId == category.CategoryId && a.i_IsDeleted == (int)Enumeratores.SiNo.No
                                               select new ComponentAdditional
                                                {
                                                    ComponenId = a.v_ComponentId,
                                                    ComponentName = a.v_Name,
                                                    CategoryId = a.i_CategoryId,
                                                    fields = (from subA in componentFields
                                                              join subB in componentField on subA.v_ComponentFieldId equals subB.v_ComponentFieldId
                                                              where subA.v_ComponentId == a.v_ComponentId 
                                                                    && subA.i_IsDeleted == (int)Enumeratores.SiNo.No 
                                                                    && subB.i_IsDeleted == (int)Enumeratores.SiNo.No
                                                              select new FieldAdditional
                                                              {
                                                                    ComponentFieldId = subA.v_ComponentFieldId,
                                                                    ComponentId = subA.v_ComponentId,
                                                                    TextLabel = subB.v_TextLabel,
                                                                    LabelWidth = subB.i_LabelWidth.Value,
                                                                    abbreviation = subB.v_Abbreviation,
                                                                    DefaultText = subB.v_DefaultText,
                                                                    ControlId = subB.i_ControlId.Value,
                                                                    GroupId = subB.i_GroupId.Value,
                                                                    ItemId = subB.i_ItemId.Value,
                                                                  WidthControl = subB.i_WidthControl.Value,
                                                                  HeightControl = subB.i_HeightControl.Value,
                                                                  MaxLenght = subB.i_MaxLenght.Value,
                                                                  IsRequired = subB.i_IsRequired.Value,
                                                                  IsCalculate = subB.i_IsCalculate.Value,
                                                                  Formula = subB.v_Formula,
                                                                  Order = subB.i_Order.Value,
                                                                  MeasurementUnitId = subB.i_MeasurementUnitId.Value,
                                                                  ValidateValue1 = subB.r_ValidateValue1.Value,
                                                                  ValidateValue2 = subB.r_ValidateValue2.Value,
                                                                  Column = subB.i_Column.Value,
                                                                  defaultIndex = subB.v_DefaultText,
                                                                  NroDecimales = subB.i_NroDecimales,
                                                                  ReadOnly = subB.i_ReadOnly.Value,
                                                                  Enabled = subB.i_Enabled.Value,
                                                                  Group = subA.v_Group,
                                                                  ComboValues = subB.i_GroupId == 0 ? null : (from sub2A in systemParameter
                                                                                                              where sub2A.i_GroupId == subB.i_GroupId
                                                                                 select new KeyValueDTO
                                                                                 {
                                                                                     Id = sub2A.i_ParameterId.ToString(),
                                                                                     Value = sub2A.v_Value1
                                                                                 }).ToList()
                                                              }).ToList()
                                                }).ToList();

                list.Add(oAdditionalExams);
            }

            foreach (var category in list)
            {
                foreach (var component in category.Components)
                {
                    Formulate formu = null;
                    TargetFieldOfCalculate targetFieldOfCalculate = null;

                    foreach (var item in component.fields)
                    {
                        List<Formulate> formuList = new List<Formulate>();
                        List<TargetFieldOfCalculate> targetFieldOfCalculateList = new List<TargetFieldOfCalculate>();

                        var find = component.fields.FindAll(p => p.Formula != null && p.Formula.Contains(item.ComponentFieldId));

                        if (find.Count != 0)
                        {
                            item.IsSourceFieldToCalculate = (int)Enumeratores.SiNo.Si;

                            foreach (var f in find)
                            {
                                formu = new Formulate();
                                formu.v_Formula = f.Formula;
                                formu.v_TargetFieldOfCalculateId = f.ComponentFieldId;
                                formuList.Add(formu);

                                targetFieldOfCalculate = new TargetFieldOfCalculate();
                                targetFieldOfCalculate.v_TargetFieldOfCalculateId = f.ComponentFieldId;
                                targetFieldOfCalculateList.Add(targetFieldOfCalculate);
                            }

                            item.FormulaList = formuList;
                            item.TargetFieldOfCalculateId = targetFieldOfCalculateList;
                        }
                    }
                }
            }

            return list;
        }
    }
}
