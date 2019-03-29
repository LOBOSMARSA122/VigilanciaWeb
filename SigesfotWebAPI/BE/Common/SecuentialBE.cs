using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Common
{
    [Table("Secuential")]
    public class SecuentialBE
    {
        [Key, Column(Order = 1)]
        public int? i_NodeId { get; set; }

        [Key, Column(Order = 2)]
        public int? i_TableId { get; set; }

        public int? i_SecuentialId { get; set; }
    }
}
