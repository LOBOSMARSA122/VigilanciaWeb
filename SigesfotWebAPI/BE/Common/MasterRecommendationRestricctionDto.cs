using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    [Table("masterrecommendationrestricction")]
    public class MasterRecommendationRestricctionDto
    {
        [Key]
        public string v_MasterRecommendationRestricctionId { get; set; }
        public string v_Name { get; set; }
        public int? i_TypifyingId { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }
}
