using System;
using System.ComponentModel.DataAnnotations;

namespace BE.Diagnostic
{
    public class DxFrecuenteBE
    {
        [Key]
        public string DxFrecuenteId { get; set; }

        public string DiseasesId { get; set; }
        public string CIE10Id { get; set; }
        public int? IsDeleted { get; set; }
        public int? InsertUserId { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
