using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BE.Common;
using BE.ConfDx;
using BL.ConfigDx;
using BL.WareHouse;
using Newtonsoft.Json;

namespace SigesoftWebAPI.Controllers
{
    public class ConfigDiagnosticController : ApiController
    {
        WarehouseBl _oWarehouseBl = new WarehouseBl();
        ConfigDxBl _oConfigDxBl = new ConfigDxBl();
        [HttpGet]
        public IHttpActionResult SearchProduct(string name)
        {
            var result = _oWarehouseBl.SearchProduct(name);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult Save(MultiDataModel model)
        {
            if (string.IsNullOrWhiteSpace(model.String1))
                return BadRequest("Información Inválida");

            var data = JsonConvert.DeserializeObject<List<ConfigDxCustom>>(model.String1);

            _oConfigDxBl.SaveConfigDX(data, model.Int1, model.Int2);
            return Ok("OK");
        }

        [HttpGet]
        public IHttpActionResult GetAllConfigDx(int index, int take) {
            var result = _oConfigDxBl.GetAllConfigDx(index, take);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetConfigDxByDiseasesId(string diseasesId, string warehouseId)
        {
            var result = _oConfigDxBl.GetConfigDxByDiseasesId(diseasesId, warehouseId);
            return Ok(result);
        }
    }
}
