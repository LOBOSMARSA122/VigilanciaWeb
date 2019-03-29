using BE.Common;
using DAL.ProductDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.ProductBl
{
    public class ProductBl
    {
        ProductDal _productDal = new ProductDal();
        public List<KeyValueDTO> GetProducts(string warehouseId) {

            var listValues = _productDal.GetProduct(warehouseId);

            return listValues;
        }
    }
}
