using Newtonsoft.Json;
using SigesoftWeb.Controllers.Security;
using SigesoftWeb.Models;
using SigesoftWeb.Models.Common;
using SigesoftWeb.Models.Message;
using SigesoftWeb.Models.Pacient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static SigesoftWeb.Utils.Enums;

namespace SigesoftWeb.Controllers.Pacientes
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class PacientesController : Controller
    {
        [GeneralSecurity(Rol = "Pacientes-Index")]
        public ActionResult Index()
        {
            
            return View();

        }

        [GeneralSecurity(Rol = "Pacientes-UpdateCreatePacient")]
        public JsonResult UpdateCreatePacient(PacientCustom data)
        {
            var user = ViewBag.USER.SystemUserId;
            string url = "Pacient/CreateOrUpdatePacient";
            if (data.v_PersonId == null)
            {
                data.ActionType = (int)ActionType.Create;
            }
            else
            {
                data.ActionType = (int)ActionType.Edit;
            }
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data)},
                { "Int1" , ViewBag.USER.SystemUserId.ToString()},
                { "Int2" , ViewBag.USER.NodeId.ToString()},
            };

            
            var result = API.Post<MessageCustom>(url, arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [GeneralSecurity(Rol = "Pacientes-UpdateCreatePacient")]
        public JsonResult GetPacientByDocNumber(string docNumber)
        {
            if (string.IsNullOrWhiteSpace(docNumber) || docNumber.Length <= 7)
            {
                MessageCustom result = new MessageCustom();
                result.Error = true;
                result.Message = "Ingrese un documento correcto por favor.";
                result.Status = 404;
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var user = ViewBag.USER.SystemUserId;
                string url = "Pacient/FindPacientByDocNumberOrPersonId?value=" + docNumber;

                Api API = new Api();

                var result = API.Get<PacientCustom>(url);
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [GeneralSecurity(Rol = "Pacientes-FindPacientByPersonId")]
        public JsonResult GetPacientByPersonId(string personId)
        {
            string url = "Pacient/FindPacientByDocNumberOrPersonId?value=" + personId;
            Api API = new Api();

            var result = API.Get<PacientCustom>(url);


            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}