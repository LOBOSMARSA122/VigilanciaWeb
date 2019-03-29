using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.History
{
    [Table("TypeOfEep")]
    public class TypeOfEppBE
    {
        [Key]
        public string v_TypeofEEPId { get; set; }
        public string v_HistoryId { get; set; }
        public int? i_TypeofEEPId { get; set; }
        public float r_Percentage { get; set; }
        public int i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }
}
