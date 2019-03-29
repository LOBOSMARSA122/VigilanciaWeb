using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Models.Vigilancia
{
    public class Vigilancia
    {
        public string v_VigilanciaId { get; set; }
        public string v_PersonId { get; set; }
        public string v_PlanVigilanciaId { get; set; }
        public int i_WasNotifiedId { get; set; }
        public int i_ConfirmedNotification { get; set; }
        public string v_Commentary { get; set; }
        public DateTime? d_StartDate { get; set; }
        public DateTime? d_EndDate { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }

    }

    public class VigilanciaServiceCustom
    {
        public string PersonId { get; set; }
        public string ServiceId { get; set; }
        public string Trabajador { get; set; }
        public string DateSchedule { get; set; }
        public int DoctoResponsibleId { get; set; }
        public string DoctoResponsibleName { get; set; }
        
    }
}