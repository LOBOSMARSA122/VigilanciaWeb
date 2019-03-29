using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.History
{
    [Table("WorkStationDangers")]
    public class WorkStationDangersBE
    {
        [Key]
        public string v_WorkstationDangersId { get; set; }

        public string v_HistoryId { get; set; }
        public int? i_DangerId { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public int? i_NoiseSource { get; set; }
        public int? i_NoiseLevel { get; set; }
        public string v_TimeOfExposureToNoise { get; set; }
    }
}
