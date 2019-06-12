using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Models.Protocol
{
    public class ProtocolCustom
    {
        public string v_ProtocolId { get; set; }
        public string v_Name { get; set; }
        public string Geso { get; set; }
        public string TipoEso { get; set; }
        public string EmpresaCliente { get; set; }
        public string EmpresaEmpleadora { get; set; }
        public string EmpresaTrabajo { get; set; }
        public string EmpresaClienteName { get; set; }
        public string EmpresaEmpleadoraName { get; set; }
        public string EmpresaTrabajoName { get; set; }
        public string ProtocolName { get; set; }
        public int i_EsoTypeId { get; set; }
        public string v_EmployerOrganizationId { get; set; }
        public string v_EmployerLocationId { get; set; }
        public string v_GroupOccupationId { get; set; }
        public string v_CustomerOrganizationId { get; set; }
        public string v_CustomerLocationId { get; set; }

        public string v_WorkingOrganizationId { get; set; }
        public string v_WorkingLocationId { get; set; }
        public int i_MasterServiceId { get; set; }
        public string v_CostCenter { get; set; }
        public int i_MasterServiceTypeId { get; set; }
        public int i_HasVigency { get; set; }
        public int? i_ValidInDays { get; set; }
        public int i_IsActive { get; set; }
        public string v_NombreVendedor { get; set; }
        public int i_OrganizationTypeId { get; set; }
    }
}