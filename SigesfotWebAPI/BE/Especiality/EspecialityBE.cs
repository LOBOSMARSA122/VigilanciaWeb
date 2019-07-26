using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Table("especiality")]
    public class EspecialityBE
    {
        [Key]
        public string v_EspecialityId { get; set; }

        public string v_ProtocolId { get; set; }
        public string v_EspecialityName { get; set; }
        public byte[] b_EspecialityPicture { get; set; }
        public TimeSpan? t_TimeForAttention { get; set; }
        public decimal? r_Cost { get; set; }
        public string v_Description { get; set; }
        public int? i_IsDeleted { get; set; }
        public TimeSpan? t_StartTime { get; set; }
        public TimeSpan? t_EndTime { get; set; }
        public TimeSpan? t_StartTime2 { get; set; }
        public TimeSpan? t_EndTime2 { get; set; }
    }
}
