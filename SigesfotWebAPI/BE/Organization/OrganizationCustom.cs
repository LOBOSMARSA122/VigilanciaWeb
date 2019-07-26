using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Organization
{
    public class OrganizationCustom
    {
        
        public string v_OrganizationId { get; set; }       
        public int? i_OrganizationTypeId { get; set; }    
        public string v_OrganizationTypeIdName { get; set; }
        public int? i_UserInterfaceOrder { get; set; }
        public int? i_SectorTypeId { get; set; }
        public string v_SectorTypeIdName { get; set; }
        public string v_IdentificationNumber { get; set; }
        public string v_Name { get; set; }
        public string v_Address { get; set; }
        public string v_PhoneNumber { get; set; }
        public string v_Mail { get; set; }
        public string v_CreationUser { get; set; }
        public string v_UpdateUser { get; set; }
        public DateTime? d_CreationDate { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public int? i_IsDeleted { get; set; }
        public string v_SectorName { get; set; }
        public string v_SectorCodigo { get; set; }
        public string v_EmailContacto { get; set; }
        public string v_Sede { get; set; }
        public byte[] b_Image { get; set; }
        public bool b_Seleccionar { get; set; }
        public string v_ContactoMedico { get; set; }
        public string v_EmailMedico { get; set; }
        public string v_ContacName { get; set; }
        public string v_Contacto { get; set; }
        public string v_Observation { get; set; }
        public int? i_NumberQuotasOrganization { get; set; }
        public int? i_NumberQuotasMen { get; set; }
        public int? i_DepartmentId { get; set; }
        public int? i_ProvinceId { get; set; }
        public int? i_DistrictId { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public string v_CIIUDescription1 { get; set; }
    }
}
