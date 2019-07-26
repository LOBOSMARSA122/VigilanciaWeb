using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Organization
{
    [Table("occupation")]
    public class OccupationBE
    {
        [Key]
        public string v_OccupationId { get; set; }

        public string v_GesId { get; set; }
        public string v_GroupOccupationId { get; set; }
        public string v_Name { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }
}
