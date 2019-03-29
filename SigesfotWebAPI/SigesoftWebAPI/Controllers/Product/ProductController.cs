using BL.ProductBl;
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
    }
}
