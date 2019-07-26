using SigesoftWeb.Controllers.Security;
using SigesoftWeb.Models;
using SigesoftWeb.Models.Message;
using SigesoftWeb.Models.RUC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SigesoftWeb.Controllers.ConsultasWeb
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ConsultaSISController : Controller
    {
        [GeneralSecurity(Rol = "ConsultaSIS-Index")]
        public ActionResult Index()
        {
            return View();
        }

        [GeneralSecurity(Rol = "ConsultaSIS-GetCapcha")]
        public JsonResult GetCaptcha()
        {
            Api API = new Api();
            var captcha = API.Get<MessageCustom>("ConsultaSIS/GetCapcha");
            return new JsonResult { Data = captcha, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [GeneralSecurity(Rol = "ConsultaSIS-GetData")]
        public JsonResult GetData(string captcha, string dni)
        {
            Api API = new Api();
            var data = API.Get<RootObjectSIS>("ConsultaSIS/GetData?captcha=" + captcha + "&dni=" + dni);
            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
    }
}