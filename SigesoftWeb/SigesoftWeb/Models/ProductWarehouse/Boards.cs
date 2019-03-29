using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Models.ProductWarehouse
{
    public class Boards
    {
        public int TotalRecords { get; set; }
        public int Index { get; set; }
        public int Take { get; set; }
    }
    public class BoardSupplier: Boards
    {
        public int? SectorId { get; set; }
        public string RazonSocial { get; set; }
        public string RUC { get; set; }
        public List<SupplierList> List { get; set; }
    }

    public class SupplierList
    {
        public string SupplierId { get; set; }
        public int? SectorTypeId { get; set; }
        public string SectorTypeIdName { get; set; }
        public string IdentificationNumber { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Mail { get; set; }
        public string CreationUser { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? IsDeleted { get; set; }
    }
}