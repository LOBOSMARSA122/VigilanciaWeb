using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.History
{
    [Table("PersonMedicalHistory")]
    public class PersonMedicalHistoryBE
    {
        [Key]
        public string v_PersonMedicalHistoryId { get; set; }
        public string v_PersonId { get; set; }
        public string v_DiseasesId { get; set; }
        public int? i_TypeDiagnosticId { get; set; }
        public DateTime? d_StartDate { get; set; }
        public string v_DiagnosticDetail { get; set; }
        public string v_TreatmentSite { get; set; }
        public int? i_AnswerId { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public int? i_SoloAnio { get; set; }
        public string v_NombreHospital { get; set; }
        public string v_Complicaciones { get; set; }
    }
}
