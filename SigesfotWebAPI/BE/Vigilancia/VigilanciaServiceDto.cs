using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Vigilancia
{
    [Table("vigilanciaservice")]
    public class VigilanciaServiceDto
    {
        [Key]
        public string v_VigilanciaServiceId { get; set; }
        public string v_VigilanciaId { get; set; }
        public string v_ServiceId { get; set; }
        public string v_Commentary { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }

    }
}
