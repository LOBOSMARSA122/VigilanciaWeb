using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    [Table("embarzo")]
    public class EmbarazoBE
    {
        [Key]
        public string v_EmbarazoId { get; set; }

        public string v_PersonId { get; set; }
        public string v_Anio { get; set; }
        public string v_Cpn { get; set; }
        public string v_Complicacion { get; set; }
        public string v_Parto { get; set; }
        public string v_PesoRn { get; set; }
        public string v_Puerpio { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public string v_ObservacionesGestacion { get; set; }
        public string v_ComentaryUpdate { get; set; }
    }
}
