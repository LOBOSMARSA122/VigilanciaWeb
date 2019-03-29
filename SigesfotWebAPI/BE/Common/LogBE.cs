using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    [Table("log")]
    public class LogBE
    {
        [Key]
        public string v_LogId { get; set; }
        public int? i_NodeLogId { get; set; }
        public int? i_EventTypeId { get; set; }
        public string v_OrganizationId { get; set; }
        public DateTime? d_Date { get; set; }
        public int? i_SystemUserId { get; set; }
        public string v_ProcessEntity { get; set; }
        public string v_ElementItem { get; set; }
        public int? i_Success { get; set; }
        public string v_ErrorException { get; set; }
    }
}
