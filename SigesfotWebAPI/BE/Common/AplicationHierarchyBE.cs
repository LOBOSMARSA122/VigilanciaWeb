using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Common
{
    [Table("applicationhierarchy")]
    public class AplicationHierarchyBE
    {
        [Key]
        public int? i_ApplicationHierarchyId {get; set;}
        public int? i_ApplicationHierarchyTypeId {get; set;}
        public int? i_Level {get; set;}
        public string v_Description {get; set;}
        public string v_Form {get; set;}
        public string v_Code {get; set;}
        public int? i_ParentId {get; set;}
        public int? i_ScopeId {get; set;}
        public int? i_TypeFormId {get; set;}
        public int? i_ExternalUserFunctionalityTypeId {get; set;}
        public int? i_IsDeleted {get; set;}
        public int? i_InsertUserId {get; set;}
        public DateTime? d_InsertDate {get; set;}
        public int? i_UpdateUserId {get; set;}
        public DateTime? d_UpdateDate {get; set;}
    }
}
