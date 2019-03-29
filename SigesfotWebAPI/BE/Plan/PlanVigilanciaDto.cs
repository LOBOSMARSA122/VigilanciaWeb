using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Plan
{
    [Table("planvigilancia")]
    public class PlanVigilanciaDto
    {
        [Key]
        public string v_PlanVigilanciaId { get; set; }
        public string v_Name { get; set; }
        public string v_Description { get; set; }
        public string v_OrganizationId { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }

    [Table("planvigilanciadiseases")]
    public class PlanVigilanciaDiseasesDto
    {
        [Key]
        public string v_PlanVigilanciaDiseasesId { get; set; }
        public string v_PlanVigilanciaId { get; set; }
        public string v_DiseasesId { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }
}
