using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    [Table("hospitalizacion")]
    public class HospitalizacionBE
    {
        [Key]
        public string v_HopitalizacionId { get; set; }
        public string v_PersonId { get; set; }
        public DateTime? d_FechaIngreso { get; set; }
        public DateTime? d_FechaAlta { get; set; }
        public string v_Comentario { get; set; }
        public string v_NroLiquidacion { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public decimal? d_PagoMedico { get; set; }
        public int? i_MedicoPago { get; set; }
        public decimal? d_PagoPaciente { get; set; }
        public int? i_PacientePago { get; set; }
        public string v_ComentaryUpdate { get; set; }
    }
}
