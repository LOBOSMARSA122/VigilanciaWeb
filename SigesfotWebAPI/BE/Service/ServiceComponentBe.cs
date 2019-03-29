using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Service
{
    public class ServiceComponentBe
    {
        public string ServiceId { get; set; }
        public string ServiceComponentId { get; set; }
        public string ComponentId { get; set; }
        public int ServiceComponentStatusId { get; set; }
        public string Comment { get; set; }
        public int ExternalInternalId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int QueueStatusId { get; set; }
        public string CreationUser { get; set; }
        public string UpdateUser { get; set; }
        public string CreationDate { get; set; }
        public string UpdateDate { get; set; }
        public int IsDeleted { get; set; }
        public int? IsApprovedId { get; set; }
    }
}
