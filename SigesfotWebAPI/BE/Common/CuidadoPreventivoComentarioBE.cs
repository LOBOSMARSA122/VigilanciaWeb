using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    [Table("CuidadoPreventivoComentario")]
    public class CuidadoPreventivoComentarioBE
    {
        [Key]
        public string v_CuidadoPreventivoComentarioId { get; set; }

        public string v_PersonId { get; set; }
        public int i_GrupoId { get; set; }
        public int i_ParametroId { get; set; }
        public string v_Comentario { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public string v_ComentaryUpdate { get; set; }
    }
}
