using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Warehouse
{
    public class ProductsForMigrationBE
    {
        [Key]
        public int? ProductForMigrationId { get; set; }
        public string WarehouseId { get; set; }
        public string ProductId { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }
        public float StockMin { get; set; }
        public float StockMax { get; set; }
        public float StockActual { get; set; }
        public int? MovementTypeId { get; set; }
        public string MovementType { get; set; }
        public int? MotiveTypeId { get; set; }
        public string MotiveType { get; set; }
        public DateTime? MovementDate { get; set; }
        public DateTime? InsertDate { get; set; }
    }
}
