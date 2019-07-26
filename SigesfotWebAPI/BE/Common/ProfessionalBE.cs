using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    [Table("professional")]
    public class ProfessionalBE
    {
        [Key]
        public string v_PersonId { get; set; }
        public int? i_ProfessionId { get; set; }
        public string v_ProfessionalCode { get; set; }
        public string v_ProfessionalInformation { get; set; }
        public byte[] b_SignatureImage { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public int? i_UpdateNodeId { get; set; }
        public string v_ComponentId { get; set; }
        public string v_ComentaryUpdate { get; set; }

    }
}
