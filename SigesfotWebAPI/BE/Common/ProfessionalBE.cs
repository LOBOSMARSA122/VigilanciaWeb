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
        public int? i_ProfesionId { get; set; }
        public byte[] b_SignatureImage { get; set; }
    }
}
