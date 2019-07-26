using Newtonsoft.Json;
using SigesoftWeb.Controllers.Security;
using SigesoftWeb.Models;
using SigesoftWeb.Models.Common;
using SigesoftWeb.Models.Especialidades;
using SigesoftWeb.Models.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SigesoftWeb.Controllers.Especialidades
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class EspecialidadesController : Controller
    {
        [GeneralSecurity(Rol = "Especialidades-Index")]
        public ActionResult Index()
        {
            Api API = new Api();
            ViewBag.Protocols = API.Get<List<Dropdownlist>>("SystemParameter/GetProtocolsForComboEspeciality?Service=10&ServiceType=9");
            return View();
        }

        [GeneralSecurity(Rol = "Especialidades-BoardEspeciality")]
        public ActionResult GetData(BoardEspeciality data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data)},
            };
            ViewBag.Especiality = API.Post<BoardEspeciality>("Especiality/GetAllEspeciality", arg);
            return PartialView("_BoardEspecialityPartial");
        }

        

        [GeneralSecurity(Rol = "Especialidades-DeleteEspeciality")]
        public JsonResult DeleteEspeciality(string especialityId)
        {
            Api API = new Api();

            var result = API.Get<MessageCustom>("Especiality/DeletedEspeciality?especialityId=" + especialityId);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        [GeneralSecurity(Rol = "Especialidades-AddEspeciality")]
        public JsonResult AddEspeciality(EspecialityCustom data)
        {
            Api API = new Api();
            data.b_EspecialityPicture = System.Convert.FromBase64String(data.v_EspecialityPicture);
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data) },
            };

            var result = API.Post<MessageCustom>("Especiality/AddEspeciality", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [GeneralSecurity(Rol = "Especialidades-GetEspeciality")]
        public JsonResult GetEspeciality(string especialityId)
        {
            Api API = new Api();

            var result = API.Get<EspecialityCustom>("Especiality/GetEspecialityById?especialityId=" + especialityId);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
    }
}