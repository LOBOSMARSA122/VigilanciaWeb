using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    [Table("AntecedenteAsistencial")]
    public class AntecedentesAsistencialBE
    {
        [Key]
        public string v_AntecendenteAsistencialId { get; set; }

        public string v_personId { get; set; }
        public int i_GrupoEtario { get; set; }
        public int i_GrupoData { get; set; }
        public int i_ParametroId { get; set; }
        public int? i_Valor { get; set; }
        public int? i_Quien { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public string v_ComentaryUpdate { get; set; }
    }
}
