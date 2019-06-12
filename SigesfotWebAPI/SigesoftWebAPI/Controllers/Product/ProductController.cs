using BE.Common;
using BE.Z_SAMBHSCUSTOM.Productos;
using BL.ProductBl;
using BL.z_ProductsSAMBHS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.Product
{
    public class ProductController : ApiController
    {
        ProductBl _productBl = new ProductBl();
        [HttpGet]
        public IHttpActionResult GetProduct(string warehouseId)
        {
            var result = _productBl.GetProducts(warehouseId);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult GetProductsSAMBHS(MultiDataModel data)
        {

            BoardProductsSAMBHS _data = JsonConvert.DeserializeObject<BoardProductsSAMBHS>(data.String1);
            var result = new ProductSAMBHSBL().GetProductsSAMBHS(_data);
            return Ok(result);
        }
    }
}
