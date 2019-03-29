using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Organization
{
    [Table("OrganizationPerson")]
    public class OrganizationPersonBE
    {
        [Key]
        public string v_OrganizationPersonId { get; set; }
        public string v_PersonId { get; set; }
        public string v_OrganizationId { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }

    }
}
