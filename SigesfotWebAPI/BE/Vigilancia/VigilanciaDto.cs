using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Vigilancia
{
    [Table("vigilancia")]
    public class VigilanciaDto
    {
        [Key]
        public string v_VigilanciaId { get; set; }
        public string v_PersonId { get; set; }
        public string v_PlanVigilanciaId { get; set; }
        public int i_WasNotifiedId { get; set; }
        public int i_ConfirmedNotification { get; set; }
        public string v_Commentary { get; set; }
        public int i_DoctorRespondibleId { get; set; }
        public int i_StateVigilanciaId { get; set; }
        public DateTime? d_StartDate { get; set; }
        public DateTime? d_EndDate { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }

    }
}
