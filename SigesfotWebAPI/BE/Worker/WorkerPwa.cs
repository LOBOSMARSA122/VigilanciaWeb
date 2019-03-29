using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Worker
{
    public class WorkerPwa
    {
        public string PersonId { get; set; }
        public int DocTypeId{ get; set; }
        public string DocTypeName { get; set; }
        public string DocNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public int Edad { get; set; }
        public int? MartialStatusId { get; set; }
        public string MartialStatus { get; set; }
        public int? LevelOfId { get; set; }
        public string LevelOf{ get; set; }
        public string TelephoneNumber { get; set; }
        public string AdressLocation { get; set; }
        public string CurrentOcupacion { get; set; }
        public string Email { get; set; }
        public int CountNewNotifications { get; set; }
    }
}
