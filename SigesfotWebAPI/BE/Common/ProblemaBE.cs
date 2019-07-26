using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    [Table("problema")]
    public class ProblemaBE
    {
        [Key]
        public string v_ProblemaId { get; set; }
        public int? i_Tipo { get; set; }
        public string v_PersonId { get; set; }
        public DateTime? d_Fecha { get; set; }
        public string v_Descripcion { get; set; }
        public int? i_EsControlado { get; set; }
        public string v_Observacion { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public string v_ComentaryUpdate { get; set; }
    }
}
