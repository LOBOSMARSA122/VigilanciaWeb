using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{    
    public class Boards
    {
        public int TotalRecords { get; set; }
        public int Index { get; set; }
        public int Take { get; set; }
    }

    public class BoardPacient : Boards
    {
        public string Pacient { get; set; }
        public int DocTypeId { get; set; }
        public string DocNumber { get; set; }

        public List<Pacients> List { get; set; }
    }

    public class Pacients
    {
        public string PacientId { get; set; }
        public string PacientFullName { get; set; }
        public int DocTypeId { get; set; }
        public string DocType { get; set; }
        public string DocNumber { get; set; }
        public string TelephoneNumber { get; set; }


        public string FirstName { get; set; }
        public string FirstLastName { get; set; }
        public string SecondLastName { get; set; }
        //public DateTime? Birthdate { get; set; }
        //public string BirthPlace { get; set; }
        //public int SexTypeId { get; set; }
        //public int MaritalStatusId { get; set; }
        //public int LevelOfId { get; set; }
        //public string AdressLocation { get; set; }
        //public string GeografyLocationId { get; set; }
        //public string ContactName { get; set; }
        //public string EmergencyPhone { get; set; }
        //public byte[] PersonImage { get; set; }
        //public string Mail { get; set; }
        //public int BloodGroupId  { get; set; }
        //public int BloodFactorId { get; set; }
        //public byte[] FingerPrintTemplate { get; set; }
        //public byte[] RubricImage { get; set; }
        //public byte[] FingerPrintImage { get; set; }
        //public string RubricImageText { get; set; }
        //public string CurrentOccupation { get; set; }
        //public int DepartmentId { get; set; }
        //public int ProvinceId { get; set; }
        //public int DistrictId { get; set; }
        //public int ResidenceInWorkplaceId { get; set; }
        //public string ResidenceTimeInWorkplace { get; set; }
        //public int TypeOfInsuranceId { get; set; }
        //public int NumberLivingChildren { get; set; }
        //public int NumberDependentChildren { get; set; }
        //public int OccupationTypeId { get; set; }
        //public string OwnerName { get; set; }
        //public int NumberLiveChildren { get; set; }
        //public int NumberDeadChildren { get; set; }
    }
}
