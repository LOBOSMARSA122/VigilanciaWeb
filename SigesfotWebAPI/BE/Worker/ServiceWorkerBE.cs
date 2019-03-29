using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace BE.Worker
{
    public class ServiceWorkerBE
    {
        public string PatientId { get; set; }
        public string PatientFullName { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string Occupation { get; set; }
        public string PlanVigilancia { get; set; }
        public string VigilanciaId { get; set; }
        public int Age { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Gender { get; set; }
        public bool Active { get; set; }
        public bool EmoToReview { get; set; }
        public int EmoToReviewCounter { get; set; }
        public bool ControlInProgress { get; set; }
        public List<ServiceWorker> Services { get; set; }
    }

    public class ServiceWorker
    {
        public string ServiceId { get; set; }
        public string ProtocolId { get; set; }
        public DateTime? ServiceDate { get; set; }
        public int? IsRevisedHistoryId { get; set; }
        public int? StatusVigilanciaId { get; set; }
        public List<DiseasesService> ListDiseasesService{ get; set; }
    }

    public class DiseasesService
    {
        public string ServiceId { get; set; }
        public string DiseasesId { get; set; }
    } 
}
