using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    [Table("restriction")]
    public class RestrictionDto
    {
        [Key]
        public string v_RestrictionId { get; set; }

        public string v_DiagnosticRepositoryId { get; set; }
        public string v_ServiceId { get; set; }
        public string v_ComponentId { get; set; }
        public string v_MasterRestrictionId { get; set; }
        public DateTime? d_StartDateRestriction { get; set; }
        public DateTime? d_EndDateRestriction { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public int i_UpdateUserId { get; set; }
    }
}
