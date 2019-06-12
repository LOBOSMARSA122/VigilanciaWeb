using HtmlAgilityPack;
using Newtonsoft.Json;
using SigesoftWeb.Controllers.Security;
using SigesoftWeb.Models;
using SigesoftWeb.Models.RUC;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SigesoftWeb.Controllers.RUC
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class RucController : Controller
    {
        public RucController()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            _myCookie = new CookieContainer();
        }
        #region Fields
        public enum Resul
        {
            Ok = 0,
            NoResul = 1,
            ErrorCapcha = 2,
            Error = 3,
        }
        private readonly CookieContainer _myCookie;
        #endregion
        // GET: Ruc
        public ActionResult Index()
        {
            return View();
        }

        //[GeneralSecurity(Rol = "Ruc-GetDataContribuyenteByRuc")]
        //public JsonResult GetDataContribuyenteByRuc(string ruc)
        //{
        //    Api API = new Api();
        //    var result = API.Get<RootObject>("DataRuc/GetDataContribuyente?numDoc=" + ruc);
        //    return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}

        
    }
}