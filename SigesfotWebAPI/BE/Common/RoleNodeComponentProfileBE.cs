using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    public class RoleNodeComponentProfileBE
    {
        [Key]
        public string RoleNodeComponentId { get; set; }

        public int? NodeId { get; set; }
        public int? RoleId { get; set; }
        public string ComponentId { get; set; }
        public int? Read { get; set; }
        public int? Write { get; set; }
        public int? Dx { get; set; }
        public int? Approved { get; set; }
        public int? IsDeleted { get; set; }
        public int? InsertUserId { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
