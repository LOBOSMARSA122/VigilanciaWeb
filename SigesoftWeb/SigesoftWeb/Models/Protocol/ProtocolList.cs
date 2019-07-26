using SigesoftWeb.Models.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Models.Protocol
{

    public class BoardProtocol : Boards
    {
        public string ProtocolName { get; set; }
        public int? MasterServiceType { get; set; }
        public int? MasterService { get; set; }
        public string EmpresaTrabajo { get; set; }
        public string EmpresaEmpleadora { get; set; }
        public string EmpresaCliente { get; set; }
        public string GESO { get; set; }
        public int? EsoType { get; set; }
        public string ComponentName { get; set; }
        public int IsActive { get; set; }
        public List<ProtocolList> List { get; set; }
    }


    public class ProtocolList
    {
        public bool select { get; set; }
        public string v_ProtocolId { get; set; }
        public string v_Protocol { get; set; }
        public string v_Name { get; set; }
        public string v_Organization { get; set; }
        public string v_Location { get; set; }
        public string v_EsoType { get; set; }
        public string v_GroupOccupation { get; set; }
        public string v_OrganizationInvoice { get; set; }
        public string v_CostCenter { get; set; }
        public string v_IntermediaryOrganization { get; set; }
        public string v_MasterServiceName { get; set; }
        public string v_Geso { get; set; }
        public string v_Occupation { get; set; }

        public string v_OrganizationId { get; set; }
        public int? i_EsoTypeId { get; set; }
        public string v_GroupOccupationId { get; set; }
        public string v_WorkingOrganizationId { get; set; }
        public string v_OrganizationInvoiceId { get; set; }

        public string v_LocationId { get; set; }
        public string v_WorkingLocationId { get; set; }
        public string v_CustomerLocationId { get; set; }

        public int i_MasterServiceId { get; set; }
        public int? i_ServiceTypeId { get; set; }

        public int? i_IsActive { get; set; }

        public int i_IsDeleted { get; set; }
        public string v_CreationUser { get; set; }
        public string v_UpdateUser { get; set; }
        public DateTime? d_CreationDate { get; set; }
        public DateTime? d_UpdateDate { get; set; }

        public string v_ContacName { get; set; }
        public string v_Address { get; set; }

        public int? Comision { get; set; }

        public string v_SectorTypeName { get; set; }
        public string v_OrganizationAddress { get; set; }
        public string v_CustomerOrganizationId { get; set; }
        public string v_NombreVendedor { get; set; }
        public string v_ComponenteNombre { get; set; }
        public string AseguradoraId { get; set; }

        public double? r_PriceFactor { get; set; }
        public double? r_HospitalBedPrice { get; set; }
        public double? r_DiscountExam { get; set; }
        public double? r_MedicineDiscount { get; set; }

        public int i_RecordType { get; set; }
        public int i_RecordStatus { get; set; }

        public List<ComponentCustom> ListComponents { get; set; }

    }


    public class ComponentCustom
    {
        public string v_ProtocolComponentId { get; set; }
        public string v_ComponentId { get; set; }
        public string v_Name { get; set; }
        public float r_Price { get; set; }
        public int? i_OperadorId { get; set; }
        public string v_OperadorName { get; set; }
        public int? i_Edad { get; set; }

        public int? i_GenderId { get; set; }
        public int? i_IsConditional { get; set; }
        public string v_IsConditional { get; set; }
        public int? i_IsAditional { get; set; }
        public int? i_ComponentTypeId { get; set; }
        public string v_ComponentTypeName { get; set; }
        public int? i_GrupoEtario { get; set; }
        public string v_GenderName { get; set; }
        public int? i_IsIMC { get; set; }
        public decimal? IMC { get; set; }
        public decimal? r_ValueIMC { get; set; }
        public int? RecordStatus { get; set; }
        public int? RecordType { get; set; }

    }
}