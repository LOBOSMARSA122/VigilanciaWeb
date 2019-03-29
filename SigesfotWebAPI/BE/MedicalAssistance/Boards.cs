using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace BE.MedicalAssistance
{
    public class Boards
    {
        public int TotalRecords { get; set; }
        public int Index { get; set; }
        public int Take { get; set; }
    }

    public class BoardPatient : Boards
    {
        public int SystemUserId { get; set; }
        public string Patient { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Workerstatus { get; set; }
        public string PlanVigilanciaId { get; set; }
        public string SystemUserByOrganizationId { get; set; }
        public string EmployerOrganizationId { get; set; }
        public List<Patients> List { get; set; }
    }

    public class Patients
    {
        
        public string ServiceId { get; set; }
        public string ProtocolId { get; set; }
        public string PatientId { get; set; }
        public string PatientFullName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Occupation { get; set; }
        public DateTime? ServiceDate { get; set; }
        public DateTime? Birthdate { get; set; }
        public string AptitudeStatus { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string ProtocolName { get; set; }
        public string OrganizationLocation { get; set; }
        public string Geso { get; set; }
        public int MasterServiceId { get; set; }
        public string MasterService { get; set; }
        public int? IsRevisedHistoryId { get; set; }
        public string PendingEvent { get; set; }
        public int StatusOrganizationPerson { get; set; }
        public int EsoTypeId { get; set; }
        public int ServiceStatusId { get; set; }
        public int? StatusVigilanciaId { get; set; }
        public bool Active { get; set; }
        public bool EmoToReview { get; set; }
        public int EmoToReviewCounter { get; set; }
        public bool ControlInProgress { get; set; }
        public string PlanVigilancia { get; set; }
        public string VigilanciaId { get; set; }
    }


}
