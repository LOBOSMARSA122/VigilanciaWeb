using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Security
{
    [Table("SystemUserGobalProfile")]
    public class SystemUserGobalProfileBE
    {
        [Key, Column(Order = 1)]
        public int? i_SystemUserId { get; set; }
        [Key, Column(Order = 2)]
        public int? i_ApplicationHierarchyId { get; set; }
        public int? i_IsDeleted { get; set; }
    }
}
