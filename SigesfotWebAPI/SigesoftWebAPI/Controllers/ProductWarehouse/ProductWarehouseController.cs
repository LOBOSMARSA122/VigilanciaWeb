using BE.Common;
using BE.ProductWarehouse;
using BE.Warehouse;
using BL.ProductWarehouse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static BE.Eso.RecipesCustom;

namespace SigesoftWebAPI.Controllers.ProductWarehouse
{
    public class ProductWarehouseController : ApiController
    {
        InputOutputBl _InputOutput = new InputOutputBl();
        SupplierBl _SupplierBl = new SupplierBl();
        [HttpPost]
        public IHttpActionResult GetDataMovements(MultiDataModel model)
        {
            BoardMovement data = JsonConvert.DeserializeObject<BoardMovement>(model.String1);
            var ListData = _InputOutput.GetDataMovements(data);

            return Ok(ListData);
        }

        [HttpPost]
        public IHttpActionResult GetDataSuppliers(MultiDataModel model)
        {
            BoardSupplier data = JsonConvert.DeserializeObject<BoardSupplier>(model.String1);
            var Data = _SupplierBl.GetdataSuppliers(data);

            return Ok(Data);
        }

        [HttpPost]
        public IHttpActionResult GenerateMovementIngreso(MultiDataModel model)
        {

            var data = JsonConvert.DeserializeObject<BoardPrintRecipes>(model.String1);
            data.NodeId = model.Int1;
            data.InsertUserId = model.Int2;

            var result = _InputOutput.GenerateMovementInput(data);

            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult GenerateMovementEgreso(MultiDataModel model)
        {

            var data = JsonConvert.DeserializeObject<BoardPrintRecipes>(model.String1);
            data.NodeId = model.Int1;
            data.InsertUserId = model.Int2;
            
            var result = _InputOutput.GenerateMovementOutput(data);

            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult GenerateMovementTransfer(MultiDataModel model)
        {

            var data = JsonConvert.DeserializeObject<BoradTransferProducts>(model.String1);
            data.NodeId = model.Int1;
            data.InsertUserId = model.Int2;

            var result = _InputOutput.ProcessTransfer(data);

            return Ok(result);
        }
    }
}
