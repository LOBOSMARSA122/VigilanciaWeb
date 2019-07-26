using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    [Table("adulto")]
    public class AdultoBE
    {
        [Key]
        public string v_AdultoId { get; set; }

        public string v_PersonId { get; set; }
        public string v_NombreCuidador { get; set; }
        public string v_EdadCuidador { get; set; }
        public string v_DniCuidador { get; set; }
        public string v_MedicamentoFrecuente { get; set; }
        public string v_ReaccionAlergica { get; set; }
        public string v_InicioRS { get; set; }
        public string v_NroPs { get; set; }
        public string v_FechaUR { get; set; }
        public string v_RC { get; set; }
        public string v_Parto { get; set; }
        public string v_Prematuro { get; set; }
        public string v_Aborto { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public string v_DescripcionAntecedentes { get; set; }
        public string v_OtrosAntecedentes { get; set; }
        public string v_FlujoVaginal { get; set; }
        public string v_ObservacionesEmbarazo { get; set; }
        public string v_ComentaryUpdate { get; set; }
    }
}
