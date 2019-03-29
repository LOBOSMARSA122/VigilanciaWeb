using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Diagnostic
{
    [Table("diseases")]
    public class DiseasesDto
    {
        [Key]
        public string v_DiseasesId { get; set; }

        public string v_CIE10Id { get; set; }
        public string v_Name { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }
}
