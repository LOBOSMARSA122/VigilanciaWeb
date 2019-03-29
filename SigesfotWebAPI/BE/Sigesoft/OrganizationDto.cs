using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Sigesoft
{
    public class OrganizationDto
    {
        
        public String v_OrganizationId { get; set; }

        
        public Nullable<Int32> i_OrganizationTypeId { get; set; }

        
        public Nullable<Int32> i_SectorTypeId { get; set; }

        
        public String v_SectorName { get; set; }

        
        public String v_SectorCodigo { get; set; }

        
        public String v_IdentificationNumber { get; set; }

        
        public String v_Name { get; set; }

        
        public String v_Address { get; set; }

        
        public String v_PhoneNumber { get; set; }

        
        public String v_Mail { get; set; }

        
        public String v_ContacName { get; set; }

        
        public String v_Contacto { get; set; }

        
        public String v_EmailContacto { get; set; }

        
        public String v_Observation { get; set; }

        
        public Nullable<Int32> i_NumberQuotasOrganization { get; set; }

        
        public Nullable<Int32> i_NumberQuotasMen { get; set; }

        
        public Nullable<Int32> i_NumberQuotasWomen { get; set; }

        
        public Nullable<Int32> i_DepartmentId { get; set; }

        
        public Nullable<Int32> i_ProvinceId { get; set; }

        
        public Nullable<Int32> i_DistrictId { get; set; }

        
        public Nullable<Int32> i_IsDeleted { get; set; }

        
        public Nullable<Int32> i_InsertUserId { get; set; }

        
        public Nullable<DateTime> d_InsertDate { get; set; }

        
        public Nullable<Int32> i_UpdateUserId { get; set; }

        
        public Nullable<DateTime> d_UpdateDate { get; set; }

        
        public Byte[] b_Image { get; set; }

        
        public String v_ContactoMedico { get; set; }

        
        public String v_EmailMedico { get; set; }


    }
}
