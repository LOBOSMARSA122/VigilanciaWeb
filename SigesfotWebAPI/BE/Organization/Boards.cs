using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Organization
{
    public class Boards
    {
        public int TotalRecords { get; set; }
        public int Index { get; set; }
        public int Take { get; set; }
    }

    public class BoardCompany : Boards
    {
        public int? OrganizationTypeId { get; set; }
        public string IdentificationNumber { get; set; }
        public string Name { get; set; }
        public List<Company> List { get; set; }
    }

    public class Company : Boards
    {
        public string OrganizationType { get; set; }
        public string OrganizationId { get; set; }
        public int? OrganizationTypeId { get; set; }
        public int? SectorTypeId { get; set; }
        public string SectorName { get; set; }
        public string SectorCodigo { get; set; }
        public string IdentificationNumber { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Mail { get; set; }
        public string ContacName { get; set; }
        public string Contacto { get; set; }
        public string EmailContacto { get; set; }
        public string Observation { get; set; }
        public int? NumberQuotasOrganization { get; set; }
        public int? NumberQuotasMen { get; set; }
        public int? NumberQuotasWomen { get; set; }
        public int? DepartmentId { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? IsDeleted { get; set; }
        public int? InsertUserId { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public byte[] Image { get; set; }
        public string ContactoMedico { get; set; }
        public string EmailMedico { get; set; }
    }
}
