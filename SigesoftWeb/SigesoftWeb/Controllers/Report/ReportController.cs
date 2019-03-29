using SigesoftWeb.Controllers.Security;
using SigesoftWeb.Models;
using SigesoftWeb.Models.Common;
using SigesoftWeb.Models.Warehouse;
using SigesoftWeb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SigesoftWeb.Controllers.Report
{
    public class ReportController : Controller
    {
        // GET: ProductOutput
        [GeneralSecurity(Rol = "ProductOutput-BoardProduct")]
        public ActionResult ProductOutput()
        {
            Api API = new Api();
            Dictionary<string, string> argCategoryProd = new Dictionary<string, string>()
            {
                { "grupoId" , ((int)Enums.DataHierarchy.CategoryProd).ToString() },
            };
            ViewBag.CategoryProd = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("DataHierarchy/GetDataHierarchyByGrupoId", argCategoryProd), Constants.All);

            Dictionary<string, string> argOrgLoc = new Dictionary<string, string>()
            {
                { "nodeId" , "9" },
            };
            ViewBag.OrganizationIdLocationId = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("ReportProduct/GetJoinOrganizationAndLocationNotInRestricted", argOrgLoc), Constants.All);

            Dictionary<string, string> argWarehouseProduct = new Dictionary<string, string>()
            {
                { "nodeId" , "9" },
                { "OrganizationId" , "" },
                { "LocationId" , "" },
            };
            ViewBag.WarehouseId = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("ReportProduct/GetWarehouseNotInRestricted", argWarehouseProduct), Constants.All);

            return View();
        }
        public ActionResult FilterReportProduct(BoardProductWarehouse data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "OrganizationLocationId", data.OrganizationLocationId},
                { "WarehouseId",data.WarehouseId},
                { "CategoryId", data.CategoryId.ToString()},
                { "ProductCode",data.ProductCode},
                { "Name", data.Name},
                { "Index", data.Index.ToString()},
                { "Take", data.Take.ToString()}
            };

            ViewBag.REGISTROSPROD = API.Post<BoardProductWarehouse>("ReportProduct/GetAllProductWarehouse", arg);
            return PartialView("_ReportProductPartial");
        }

        public JsonResult GetWarehouseNotInRestricted(string OrganizationId, string LocationId)
        {
            Api API = new Api();
            Dictionary<string, string> argWarehouseId = new Dictionary<string, string>()
            {
                { "nodeId" , "9" },
                { "OrganizationId" , OrganizationId },
                { "LocationId" , LocationId },
            };
            List<Dropdownlist> Warehouses = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("ReportProduct/GetWarehouseNotInRestricted", argWarehouseId), Constants.Select);

            return new JsonResult { Data = Warehouses, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}