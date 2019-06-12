using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Z_CommonSAMBHS
{
    [Table("systemparameter")]
    public class SystemParameterBE
    {
        [Key, Column(Order = 1)]
        public int i_GroupId { get; set; }
        [Key, Column(Order = 2)]
        public int i_ParameterId { get; set; }

        public int? i_ParentParameterId { get; set; }
        public string v_Value1 { get; set; }
        public string v_Value2 { get; set; }
        public string v_Field { get; set; }
        public int? i_Sort { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }
}
