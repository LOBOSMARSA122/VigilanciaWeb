using Newtonsoft.Json;
using SigesoftWeb.Controllers.Security;
using SigesoftWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SigesoftWeb.Controllers.Ventas
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class RegistroDeVentaController : Controller
    {
        [GeneralSecurity(Rol = "RegistroDeVenta-Index")]
        public ActionResult Index()
        {

  
            return View();
        }


        [GeneralSecurity(Rol = "RegistroDeVenta-GetSeriesDocumento")]
        public JsonResult GetSeriesDocumento(string IdDocumento)
        {
            Api API = new Api();
            string idEstablecimiento = ViewBag.USER.EstablecimientoPredeterminado.ToString();
            var result = API.Get<string>("Documento/GetSeriesDocumento?IdEstablecimiento=" + idEstablecimiento + "&IdDocumento=" + IdDocumento);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
  
}