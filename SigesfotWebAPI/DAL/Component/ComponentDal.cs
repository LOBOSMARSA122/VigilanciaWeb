using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using BE.Component;
using static BE.Common.Enumeratores;

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

        public List<KeyValueDTO> GetAllComponents()
        {
            //mon.IsActive = true;
            try
            {

                var DataComponentList = (from a in ctx.Component
                                         join B in ctx.SystemParameter on new { a = a.i_CategoryId, b = 116 } equals new { a = B.i_ParameterId, b = B.i_GroupId } into B_join
                                         from B in B_join.DefaultIfEmpty()
                                         where a.i_IsDeleted == 0 &&
                                               a.i_ComponentTypeId == 1
                                         select new KeyValueDTO
                                         {
                                             Value4 = a.i_CategoryId,//i_CategoryId
                                             Value = a.i_CategoryId == -1 ? a.v_Name : B.v_Value1, //CategoryName
                                             Value2 = a.v_ComponentId, // ComponentId
                                             Value3 = a.v_Name, // v_Name
                                             //Id = a.v_ComponentId
                                         }).ToList();


                List<KeyValueDTO> objData = DataComponentList.ToList();
                return objData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public BoardExamsCustom GetExamsForConsult(BoardExamsCustom data)
        {
            try
            {
                var query = (from A in ctx.ServiceComponent
                             join B in ctx.SystemParameter on new { a = A.i_ServiceComponentStatusId.Value, b = 127 }
                                      equals new { a = B.i_ParameterId, b = B.i_GroupId }
                             join C in ctx.Component on A.v_ComponentId equals C.v_ComponentId
                             join D in ctx.SystemParameter on new { a = A.i_QueueStatusId.Value, b = 128 }
                                      equals new { a = D.i_ParameterId, b = D.i_GroupId }
                             join E in ctx.Service on A.v_ServiceId equals E.v_ServiceId
                             join F in ctx.SystemParameter on new { a = C.i_CategoryId, b = 116 }
                                      equals new { a = F.i_ParameterId, b = F.i_GroupId } into F_join
                             from F in F_join.DefaultIfEmpty()

                             where A.v_ServiceId == data.ServiceId &&
                                   A.i_IsDeleted == (int)SiNo.No &&
                                   A.i_IsRequiredId == (int)SiNo.Si

                             select new ExamsCustom
                             {
                                 v_ComponentId = A.v_ComponentId,
                                 v_PersonId = E.v_PersonId,
                                 v_ComponentName = C.v_Name,
                                 i_ServiceComponentStatusId = A.i_ServiceComponentStatusId.Value,
                                 v_ServiceComponentStatusName = B.v_Value1,
                                 d_StartDate = A.d_StartDate.Value,
                                 d_EndDate = A.d_EndDate.Value,
                                 i_QueueStatusId = A.i_QueueStatusId.Value,
                                 v_QueueStatusName = D.v_Value1,
                                 ServiceStatusId = E.i_ServiceStatusId.Value,
                                 v_Motive = E.v_Motive,
                                 i_CategoryId = C.i_CategoryId,
                                 v_CategoryName = C.i_CategoryId == -1 ? C.v_Name : F.v_Value1,
                                 v_ServiceId = E.v_ServiceId,
                                 v_ServiceComponentId = A.v_ServiceComponentId,
                             });

                var objData = query.AsEnumerable()
                             .Where(s => s.i_CategoryId != -1)
                             .GroupBy(x => x.i_CategoryId)
                             .Select(group => group.First());

                List<ExamsCustom> obj = objData.ToList();

                obj.AddRange(query.Where(p => p.i_CategoryId == -1));

                data.List = obj;
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ExamsCustom> GetMedicalExam(string componentName)
        {
            try
            {

                var query = from A in ctx.Component

                            join J1 in ctx.SystemUser on new { i_InsertUserId = A.i_InsertUserId.Value }
                                                            equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                            from J1 in J1_join.DefaultIfEmpty()

                            join J2 in ctx.SystemUser on new { i_UpdateUserId = A.i_UpdateUserId.Value }
                                                            equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                            from J2 in J2_join.DefaultIfEmpty()

                            join B in ctx.SystemParameter on new { a = A.i_CategoryId, b = 116 }  // CATEGORIA DEL EXAMEN
                                                            equals new { a = B.i_ParameterId, b = B.i_GroupId } into B_join
                            from B in B_join.DefaultIfEmpty()

                            join E in ctx.SystemParameter on new { a = A.i_DiagnosableId.Value, b = 111 } equals new { a = E.i_ParameterId, b = E.i_GroupId }
                            join F in ctx.SystemParameter on new { a = A.i_ComponentTypeId.Value, b = 126 } equals new { a = F.i_ParameterId, b = F.i_GroupId }

                            where A.i_IsDeleted == 0 && (A.v_Name.Contains(componentName) || componentName == null)
                            select new ExamsCustom
                            {
                                v_ComponentId = A.v_ComponentId,
                                v_ComponentName = A.v_Name,
                                i_CategoryId = A.i_CategoryId,
                                v_CategoryName = B.v_Value1,
                                v_DiagnosableName = E.v_Value1,
                                v_ComponentTypeName = F.v_Value1,
                                v_CreationUser = J1.v_UserName,
                                v_UpdateUser = J2.v_UserName,
                                d_CreationDate = A.d_InsertDate,
                                d_UpdateDate = A.d_UpdateDate,
                                i_IsDeleted = A.i_IsDeleted.Value,
                                r_Price = A.r_BasePrice,
                                i_ComponentTypeId = A.i_ComponentTypeId,
                                i_index = A.i_UIIndex.Value,
                                v_IdUnidadProductiva = A.v_IdUnidadProductiva
                            };

                List<ExamsCustom> objData = query.ToList();
                return objData;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
