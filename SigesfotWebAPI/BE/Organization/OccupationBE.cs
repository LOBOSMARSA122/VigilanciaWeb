using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Organization
{
    public class OccupationBE
    {
        [Key]
        public string OccupationId { get; set; }

        public string GesId { get; set; }
        public string GroupOccupationId { get; set; }
        public string Name { get; set; }
        public int? IsDeleted { get; set; }
        public int? InsertUserId { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
