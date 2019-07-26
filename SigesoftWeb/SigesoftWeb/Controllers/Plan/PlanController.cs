using Newtonsoft.Json;
using SigesoftWeb.Controllers.Security;
using SigesoftWeb.Models;
using SigesoftWeb.Models.Lineas;
using SigesoftWeb.Models.Message;
using SigesoftWeb.Models.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SigesoftWeb.Controllers.Plan
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class PlanController : Controller
    {
        // GET: Plan
        public ActionResult Index()
        {
            return View();
        }

        [GeneralSecurity(Rol = "Plan-SavePlan")]
        public JsonResult SavePlan(PlanCustom data)
        {
            Api API = new Api();

            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data) },
                { "Int1", ViewBag.USER.SystemUserId.ToString() },
                { "Int2", ViewBag.USER.NodeId.ToString() },
            };

            var result = API.Post<MessageCustom>("Plan/SavePlan", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Plan-GetPlanByProtocolId")]
        public JsonResult GetPlanByProtocolId(string protocolId)
        {
            Api API = new Api();

            var result = API.Get<List<PlanCustom>>("Plan/GetPlanByProtocolId?protocolId=" + protocolId);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Plan-GetPlanByProtocolId")]
        public JsonResult DeletedPlan(string planId)
        {
            Api API = new Api();

            var result = API.Get<MessageCustom>("Plan/DeletedPlan?planId=" + planId);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}