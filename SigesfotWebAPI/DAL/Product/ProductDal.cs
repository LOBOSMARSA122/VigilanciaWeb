using BE.Common;
using BE.ProductWarehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ProductDal
{
    public class ProductDal
    {
        public List<KeyValueDTO> GetProduct(string warehouseId) {
            DatabaseContext dbContext = new DatabaseContext();

            if (!string.IsNullOrWhiteSpace(warehouseId))
            {
                var query = from a in dbContext.ProductWarehouse
                            join b in dbContext.Product on a.v_ProductId equals b.v_ProductId
                            join eee in dbContext.DataHierarchy on new { a = b.i_MeasurementUnitId.Value, b = 105 } // Unid medida
                                                              equals new { a = eee.i_ItemId, b = eee.i_GroupId } into J8_join
                            from eee in J8_join.DefaultIfEmpty()

                            join dhy in dbContext.DataHierarchy on new { a = b.i_CategoryId.Value, b = 103 } // Unid medida
                                                              equals new { a = dhy.i_ItemId, b = dhy.i_GroupId } into dhy_join
                            from dhy in dhy_join.DefaultIfEmpty()
                            where eee.i_IsDeleted == 0 && a.v_WarehouseId == warehouseId
                            select new KeyValueDTO
                            {
                                Id = a.v_ProductId,
                                Value = "Producto : " + b.v_Name + " / Marca : " + b.v_Brand + " / Modelo : " + b.v_Model + " / Nro. Serie : " + b.v_SerialNumber,
                                Value2 = dhy.v_Value1,
                                Value3 = b.v_Presentation + " " + eee.v_Value1,
                                Value4 = (int)a.r_StockActual,

                            };
                var query1 = query.AsEnumerable()
                             .Select(x => new KeyValueDTO
                             {
                                 Id = x.Id,
                                 Value = x.Value + " / (Stock Actual : " + x.Value4 + ")",
                                 Value2 = x.Value2,
                                 Value3 = x.Value3,
                                 Value4 = x.Value4
                             }).ToList().Take(15);
                List<KeyValueDTO> objDataList = query1.OrderBy(p => p.Value).ToList();
                return objDataList;
            }
            else
            {
                var query = (from a in dbContext.Product
                             join dhy in dbContext.DataHierarchy on new { a = a.i_CategoryId.Value, b = 103 } // Unid medida
                                                              equals new { a = dhy.i_ItemId, b = dhy.i_GroupId } into dhy_join
                             from dhy in dhy_join.DefaultIfEmpty()
                             where a.i_IsDeleted == (int)Enumeratores.SiNo.No
                             select new KeyValueDTO
                             {
                                 Id = a.v_ProductId,
                                 Value = "Producto : " + a.v_Name + " / Marca : " + a.v_Brand + " / Modelo : " + a.v_Model + " / Nro. Serie : " + a.v_SerialNumber,
                                 Value2 = dhy.v_Value1,
                             }).Take(15);
                List<KeyValueDTO> objDataList = query.OrderBy(p => p.Value).ToList();
                return objDataList;
            }

            
            
            

        }

        
    }
}
