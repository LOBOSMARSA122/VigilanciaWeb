using Newtonsoft.Json;
using SigesoftWeb.Controllers.Security;
using SigesoftWeb.Models;
using SigesoftWeb.Models.Common;
using SigesoftWeb.Models.ProductWarehouse;
using SigesoftWeb.Models.Warehouse;
using SigesoftWeb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static SigesoftWeb.Models.Eso.RecipesCustom;

namespace SigesoftWeb.Controllers.ProductWarehouse
{
    public class ProductWarehouseController : Controller
    {
        [GeneralSecurity(Rol = "ProductWarehouse-Index")]
        public ActionResult Index()
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "grupoId" , ((int)Enums.SystemParameter.TypeMovement).ToString() },
            };

            Dictionary<string, string> arg2 = new Dictionary<string, string>()
            {
                { "grupoId" , ((int)Enums.SystemParameter.MotiveMovement).ToString() },
            };

            Dictionary<string, string> arg3 = new Dictionary<string, string>()
            {
                { "grupoId" , ((int)Enums.DataHierarchy.Sector).ToString()}
            };


            ViewBag.Sector = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("DataHierarchy/GetDataHierarchyByGrupoId", arg3), Constants.All);
            ViewBag.MotiveMovement = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId", arg2), Constants.Select);
            ViewBag.MovementType = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId", arg), Constants.All);
            return View();
        }

        [GeneralSecurity(Rol = "ProductWarehouse-Filter")]
        public ActionResult Filter(BoardMovement data)
        {
            Api API = new Api();
            Dictionary<string , string> arg = new Dictionary<string, string>()
            {
                { "String1" , JsonConvert.SerializeObject(data)}
            };

            ViewBag.DataQueryMovement = API.Post<BoardMovement>("ProductWarehouse/GetDataMovements", arg);
            return PartialView("_BoardQueryMovementPartial");
        }
        [GeneralSecurity(Rol = "ProductWarehouse-GetProducts")]
        public JsonResult GetProductsWarehouse(string warehouseId)
        {
            Api API = new Api();
            var result = API.Get<List<KeyValueDTO>>("Product/GetProduct?warehouseId=" + warehouseId);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [GeneralSecurity(Rol = "ProductWarehouse-GetDataTransfer")]
        public JsonResult GetNode(bool remoto)
        {
            Api API = new Api();
            var result = API.Get<List<KeyValueDTO>>("Node/GetAllNodeForCombo?remoto=" + remoto + "&nodeId=" + ViewBag.USER.NodeId);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "ProductWarehouse-Filter")]
        public ActionResult FilterSupplier(BoardSupplier data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1" , JsonConvert.SerializeObject(data)}
            };
            ViewBag.DataQuerySupplier = API.Post<BoardSupplier>("ProductWarehouse/GetDataSuppliers", arg);
            return PartialView("_BoardQuerySupplierPartial");
        }

        [GeneralSecurity(Rol = "ProductWarehouse-ProductMovement")]
        public ActionResult GenerateIngresoProduct(BoardPrintRecipes data)
        {
            data.MovementTypeId = (int)Enumeratores.MovementType.Ingreso;
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1" , JsonConvert.SerializeObject(data) },
                { "Int1" , ViewBag.USER.NodeId.ToString() },
                { "Int2" , ViewBag.USER.SystemUserId.ToString() },
            };

            var result = API.Post<string>("ProductWarehouse/GenerateMovementIngreso", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "ProductWarehouse-ProductMovement")]
        public ActionResult GenerateEgresoProduct(BoardPrintRecipes data)
        {
            data.MovementTypeId = (int)Enumeratores.MovementType.Egreso;

            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1" , JsonConvert.SerializeObject(data) },
                { "Int1" , ViewBag.USER.NodeId.ToString() },
                { "Int2" , ViewBag.USER.SystemUserId.ToString() },
            };

            var result = API.Post<string>("ProductWarehouse/GenerateMovementEgreso", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "ProductWarehouse-ProductMovement")]
        public ActionResult GenerateTransferProducts(BoradTransferProducts data)
        {
            Api API = new Api();

            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1" , JsonConvert.SerializeObject(data) },
                { "Int1" , ViewBag.USER.NodeId.ToString() },
                { "Int2" , ViewBag.USER.SystemUserId.ToString() },
            };

            var result = API.Post<string>("ProductWarehouse/GenerateMovementTransfer", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
