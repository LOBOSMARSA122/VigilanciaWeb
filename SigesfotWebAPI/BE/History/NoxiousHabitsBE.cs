using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.History
{
    [Table("NoxiousHabits")]
    public class NoxiousHabitsBE
    {
        [Key]
        public string v_NoxiousHabitsId { get; set; }

        public string v_PersonId { get; set; }
        public int? i_TypeHabitsId { get; set; }
        public string v_Frequency { get; set; }
        public string v_Comment { get; set; }
        public string v_DescriptionHabit { get; set; }
        public string v_DescriptionQuantity { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }
}
