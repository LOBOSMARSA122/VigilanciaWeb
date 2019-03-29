using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Warehouse
{
    [Table("RestrictedWarehouseProfileList")]
    public class RestrictedWarehouseProfileListBE
    {
        public int ApplicationHierarchyId { get; set; }
        public int SystemUserId { get; set; }
        public int NodeId { get; set; }
        public string OrganizationId { get; set; }
        public string LocationId { get; set; }
        public string WarehouseId { get; set; }
        public string NodeName { get; set; }
        public string OrganizationName { get; set; }
        public string LocationName { get; set; }
        public string WarehouseName { get; set; }
    }
}
