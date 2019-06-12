using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    [Table("holidays")]
    public class HolidaysBE
    {
        [Key]
        public string v_HolidayId { get; set; }
        public int? i_Year { get; set; }
        public DateTime? d_Date { get; set; }
        public string v_Reason { get; set; }
        public int? i_IsDeleted { get; set; }
    }
}
