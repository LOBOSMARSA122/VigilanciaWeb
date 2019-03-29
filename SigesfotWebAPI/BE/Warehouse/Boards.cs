using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Warehouse
{
    public class Boards
    {
        public int TotalRecords { get; set; }
        public int Index { get; set; }
        public int Take { get; set; }
    }

    public class BoardProductWarehouse : Boards
    {
        public int? CategoryId { get; set; }
        public string OrganizationLocationId { get; set; }
        public string WarehouseId { get; set; }
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<ProductWarehouse> List { get; set; }
    }


    public class ProductWarehouse : Boards
    {
        public string Category { get; set; }
        public string ProductName { get; set; }
        public string WarehouseId { get; set; }
        public string ProductId { get; set; }
        public float StockMin { get; set; }
        public float StockMax { get; set; }
        public float StockActual { get; set; }
        public int? InsertUserId { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class BoardProduct : Boards
    {
        public int CategoryId { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public List<Products> List { get; set; }
    }

    public class Products
    {
        public string ProductId { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryProd { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }

        public string GenericName { get; set; }
        public string BarCode { get; set; }

        public string ProductCode { get; set; }

        public string Brand { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int? MeasurementUnitId { get; set; }
        public float ReferentialCostPrice { get; set; }
        public float ReferentialSalesPrice { get; set; }

        public string Presentation { get; set; }
        public string AdditionalInformation { get; set; }
        public byte[] Image { get; set; }
        public int? IsDeleted { get; set; }
        public int? InsertUserId { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
    }


    public class BoardMovement : Boards
    {
        public string OrganizationLocationId { get; set; }
        public string WarehouseId { get; set; }
        public int MovementType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<MovementList> List { get; set; }
    }

    public class Movements
    {
        public string MovementId { get; set; }
        public string MovementTypeId { get; set; }
        public DateTime? Date { get; set; }
        public string ProcessTypeId { get; set; }
        public string MotiveTypeId { get; set; }
        public string Supplier { get; set; }
        public float TotalQuantity { get; set; }
        public string ReferenceDocument { get; set; }

        public int? IsDeleted { get; set; }
        public int? InsertUserId { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    public class BoardMovementDataProcess : Boards
    {
        //public string ProductId { get; set; }
        //public string TotalQuantity { get; set; }
        //public float Price { get; set; }
        public int RecordStatus { get; set; }
        //public string ReferenceDocument { get; set; }
        public List<MovementsDetails> MovementDetailProduct { get; set; }
        public List<MovementsCustom> MovementProduct { get; set; }
    }
    public class MovementsDetails
    {
        public string MovementId { get; set; }
        public string ProductId { get; set; }
        public string WarehouseId { get; set; }
        public float Price { get; set; }
        public float Quantity { get; set; }
        public string StockMax { get; set; }
        public string StockMin { get; set; }
        public string MovementTypeId { get; set; }
        public int? i_MovementTypeId { get; set; }
        public string SubTotal { get; set; }
        public string UpdateDate { get; set; }
    }

    public class MovementsCustom
    {
        public string MovementId { get; set; }
        public string MovementTypeId { get; set; }
        public int? i_MovementTypeId { get; set; }
        public string WarehouseId { get; set; }
        public DateTime? Date { get; set; }
        public string ProcessTypeId { get; set; }
        public string MotiveTypeId { get; set; }
        public string Supplier { get; set; }
        public float TotalQuantity { get; set; }
        public string ReferenceDocument { get; set; }
        public int? NodeId { get; set; }
        public string ProductId { get; set; }

        public int? IsDeleted { get; set; }
        public int? InsertUserId { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    public class MessagesList
    {
        public List<string> ErrorMessage { get; set; }
        public List<string> SuccessMessage { get; set; }
    }


    public class MovementList
    {

        public int i_NodeId { get; set; }
        public string v_MovementId { get; set; }
        public string v_OrganizationId { get; set; }
        public string v_WarehouseId { get; set; }
        public string v_SupplierId { get; set; }
        public string v_ProductName { get; set; }
        public string v_SupplierName { get; set; }
        public string v_NodeName { get; set; }
        public int? i_MovementTypeId { get; set; }
        public string v_MovementTypeDescription { get; set; }
        public int? i_MotiveTypeId { get; set; }
        public string v_MotiveTypeDescription { get; set; }
        public DateTime? d_MovementDate { get; set; }
        public float? r_TotalQuantity { get; set; }
        public string v_ReferentDocument { get; set; }
        public string v_IsProcessed { get; set; }
        public int? i_ProcessTypeId { get; set; }
        public string v_CreationUser { get; set; }
        public string v_UpdateUser { get; set; }
        public DateTime? d_CreationDate { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public string v_UpdateNodeName { get; set; }
    }
}
