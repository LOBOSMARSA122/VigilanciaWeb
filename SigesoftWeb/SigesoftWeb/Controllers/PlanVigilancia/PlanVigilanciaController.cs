using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using SigesoftWeb.Controllers.Security;
using SigesoftWeb.Models;
using SigesoftWeb.Models.Common;
using SigesoftWeb.Models.Plan;
using SigesoftWeb.Utils;

namespace SigesoftWeb.Controllers.PlanVigilancia
{
    public class PlanVigilanciaController : Controller
    {
        [GeneralSecurity(Rol = "PlanVigilancia-BoardPlanVigilancia")]
        public ActionResult Index()
        {
            //Api API = new Api();

            //Dictionary<string, string> arg = new Dictionary<string, string>()
            //{
            //    { "grupoId" , ((int)Enums.SystemParameter.OrgType).ToString() }
            //};

            //ViewBag.OrgType = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId", arg), Constants.All);
            return View();
        }

        [GeneralSecurity(Rol = "PlanVigilancia-BoardPlanVigilancia")]
        public ActionResult FilterPlanVigilancia(BoardPlanVigilancia data)
        {

            Api API = new Api();

            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "Name" , data.Name},
                { "OrganizationId" , data.OrganizationId},
                { "Take" , data.Take.ToString()},
                { "Index" , data.Index.ToString()}
            };

            ViewBag.DataPV = API.Post<BoardPlanVigilancia>("PlanVigilancia/Filter", arg);

            return PartialView("_BoardPlanVigilancia");
        }

        [GeneralSecurity(Rol = "PlanVigilancia-BoardPlanVigilancia")]
        public ActionResult CreatePlanVigilancia(string planVigianciaId)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "planVigianciaId" , planVigianciaId }
            };

            ViewBag.DataPVById = API.Get<PlanVigilanciaCustom>("PlanVigilancia/GetId" , arg);
            return View();
        }

        [GeneralSecurity(Rol = "PlanVigilancia-BoardPlanVigilancia")]
        public JsonResult SavePlanVigilancia(PlanVigilanciaCustom data)
        {

            data.SystemUserId = ViewBag.USER.SystemUserId;
            data.NodeId = ViewBag.USER.NodeId;
            Api API = new Api();
            string url = "PlanVigilancia/Save";
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                {  "String1" , JsonConvert.SerializeObject(data) },
            };

            var response = API.Post<bool>(url, arg);
            return Json(response);
        }

        [GeneralSecurity(Rol = "PlanVigilancia-BoardPlanVigilancia")]
        public JsonResult SearchDisease(string name)
        {
            Api API = new Api();
            string url = "PlanVigilancia/SearchDisease";

            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                {  "name" , name },
            };

            List<string> response = API.Get<List<string>>(url, arg);
            return new JsonResult { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "PlanVigilancia-BoardPlanVigilancia")]
        public JsonResult RemovePV(string planVigilanciaId)
        {
             
            Api API = new Api();
            string url = "PlanVigilancia/Remove";

            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                {  "planVigilanciaId" , planVigilanciaId },
                { "systemUserId" , ViewBag.USER.SystemUserId.ToString()}
            };

            bool response = API.Get<bool>(url, arg);
            return new JsonResult { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}