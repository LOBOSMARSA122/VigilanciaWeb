using SigesoftWeb.Controllers.Security;
using SigesoftWeb.Models;
using SigesoftWeb.Models.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SigesoftWeb.Controllers.Protocol
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ProtocolController : Controller
    {
        [GeneralSecurity(Rol = "Protocol-Index")]
        public ActionResult Index()
        {
            return View();
        }

        [GeneralSecurity(Rol = "Protocol-GetDataProtocol")]
        public JsonResult GetDataProtocol(string protocolId)
        {
            Api API = new Api();
            var result = API.Get<ProtocolCustom>("Protocol/GetDataProtocol?protocolId=" + protocolId);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}