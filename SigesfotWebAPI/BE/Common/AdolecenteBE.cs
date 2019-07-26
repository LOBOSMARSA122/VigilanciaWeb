using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    [Table("adolescente")]
    public class AdolescenteBE
    {
        [Key]
        public string v_AdolescenteId { get; set; }

        public string v_PersonId { get; set; }
        public string v_EdadInicioTrabajo { get; set; }
        public string v_TipoTrabajo { get; set; }
        public string v_NroHorasTv { get; set; }
        public string v_NroHorasJuegos { get; set; }
        public string v_MenarquiaEspermarquia { get; set; }
        public string v_EdadInicioRS { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public string v_NombreCuidador { get; set; }
        public string v_EdadCuidador { get; set; }
        public string v_DniCuidador { get; set; }
        public string v_ViveCon { get; set; }
        public string v_Observaciones { get; set; }
        public string v_ComentaryUpdate { get; set; }
    }
}
