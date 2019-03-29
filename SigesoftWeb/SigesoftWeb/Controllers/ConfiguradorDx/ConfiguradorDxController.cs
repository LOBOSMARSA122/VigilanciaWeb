using Newtonsoft.Json;
using SigesoftWeb.Controllers.Security;
using SigesoftWeb.Models;
using SigesoftWeb.Models.ConfigDx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SigesoftWeb.Controllers.ConfiguradorDx
{
    public class ConfiguradorDxController : Controller
    {
        [GeneralSecurity(Rol = "ConfiguradorDx-Index")]
        public ActionResult Index()
        {
            return View();
        }

        [GeneralSecurity(Rol = "ConfiguradorDx-DiagnosticValue")]
        public JsonResult SearchDiagnostic(string value)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "name" , value  },
            };

            var result = API.Get<List<string>>("PlanVigilancia/SearchDisease", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "ConfiguradorDx-ProductValue")]
        public JsonResult SearchProduct(string value)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "name" , value  },
            };

            var result = API.Get<List<string>>("ConfigDiagnostic/SearchProduct", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "ConfiguradorDx-Save")]
        public JsonResult Save(List<ConfigDxCustom> configDxCustom)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1" , JsonConvert.SerializeObject(configDxCustom)  },
                { "Int1" , ViewBag.USER.NodeId.ToString() },
                { "Int2" , ViewBag.USER.SystemUserId.ToString() },
            };

            var result = API.Post<string>("ConfigDiagnostic/Save", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "ConfiguradorDx-BoardConfig")]
        public ActionResult GetAllConfigDx(int index, int take)
        {
            Api API = new Api();
            ViewBag.Result = API.Get<BoardConfigDx>("ConfigDiagnostic/GetAllConfigDx?index=" + index + "&take=" + take);
            return PartialView("_BoardConfigDxPartial");
        }

        [GeneralSecurity(Rol = "ConfiguradorDx-Save")]
        public JsonResult GetConfigDxByDiseasesId(string diseasesId, string warehouseId)
        {
            Api API = new Api();

            var result = API.Get<List<ConfigDxCustom>>("ConfigDiagnostic/GetConfigDxByDiseasesId?diseasesId=" + diseasesId + "&warehouseId=" + warehouseId);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
