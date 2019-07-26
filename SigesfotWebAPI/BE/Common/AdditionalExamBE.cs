using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    [Table("additionalexam")]
    public class AdditionalExamBE
    {
        [Key]
        public string v_AdditionalExamId { get; set; }

        public string v_ServiceId { get; set; }
        public string v_PersonId { get; set; }
        public string v_ProtocolId { get; set; }
        public string v_ComponentId { get; set; }
        public string v_Commentary { get; set; }
        public int? i_IsProcessed { get; set; }
        public int? i_IsNewService { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }
}
