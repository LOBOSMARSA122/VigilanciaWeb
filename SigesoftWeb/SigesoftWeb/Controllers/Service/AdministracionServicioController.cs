using Newtonsoft.Json;
using SigesoftWeb.Controllers.Security;
using SigesoftWeb.Models;
using SigesoftWeb.Models.Common;
using SigesoftWeb.Models.Message;
using SigesoftWeb.Models.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SigesoftWeb.Controllers.Service
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class AdministracionServicioController : Controller
    {
        [GeneralSecurity(Rol = "AdministracionServicio-Index")]
        public ActionResult Index()
        {
            Api API = new Api();
            ViewBag.MasterService = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroMasterServiceByGrupoId?serviceType=9");
            ViewBag.EstadoServicio = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=125");
            ViewBag.Users = API.Get<List<Dropdownlist>>("SystemParameter/GetUser");
            return View();
        }

        [GeneralSecurity(Rol = "AdministracionServicio-FilterService")]
        public ActionResult FilterService(BoardServiceCustomList data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data)},
            };
            ViewBag.Services = API.Post<BoardServiceCustomList>("Service/GetServices", arg);
            return PartialView("_BoardServicesPartial");
        }

        [GeneralSecurity(Rol = "AdministracionServicio-GenerateHistoriaClinica")]
        public JsonResult GenerateHistoriaClinica(string personId, string serviceId)
        {
            Api API = new Api();

            var result = API.Get<MessageCustom>("HistoriaClinica/GenerateHistoriaClinica?personId=" + personId + "&serviceId=" + serviceId);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}