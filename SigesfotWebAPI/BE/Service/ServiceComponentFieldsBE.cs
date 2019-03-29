using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Service
{
    [Table("ServiceComponentFields")]
    public class ServiceComponentFieldsDto
    {
        [Key]
        public string v_ServiceComponentFieldsId { get; set; }

        public string v_ServiceComponentId { get; set; }
        public string v_ComponentId { get; set; }
        public string v_ComponentFieldId { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }
}
