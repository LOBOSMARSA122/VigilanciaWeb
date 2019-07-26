using Newtonsoft.Json;
using SigesoftWeb.Controllers.Security;
using SigesoftWeb.Models;
using SigesoftWeb.Models.Common;
using SigesoftWeb.Models.Especialidades;
using SigesoftWeb.Models.Message;
using SigesoftWeb.Models.Pacient;
using SigesoftWeb.Models.RUC;
using SigesoftWeb.Models.Service;
using SigesoftWeb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SigesoftWeb.Utils.Enums;

namespace SigesoftWeb.Controllers.Reservas
{
    public class ReservasController : Controller
    {

        public ActionResult Index()
        {
            Api API = new Api();

            BoardEspeciality data = new BoardEspeciality();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1" , JsonConvert.SerializeObject(data)}
            };

            ViewBag.TipoDocumento = API.Get<List<Dropdownlist>>("DataHierarchy/GetDataHierarchyByGrupoId?grupoId=" + ((int)Enums.DataHierarchy.DocType).ToString());
            ViewBag.Genero = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.Gender).ToString());

            ViewBag.Especialidades = API.Post<BoardEspeciality>("Especiality/GetAllEspeciality", arg);

            return View();
        }

        public JsonResult GetCaptcha()
        {
            Api API = new Api();
            var captcha = API.Get<MessageCustom>("ConsultaSIS/GetCapcha");
            return new JsonResult { Data = captcha, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult UpdateCreatePacient(PacientCustom data)
        {
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
                { "Int1" , "11"},
                { "Int2" , "9"},
            };


            var result = API.Post<MessageCustom>(url, arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult GetData(string captcha, string dni)
        {
            Api API = new Api();
            var data = API.Get<RootObjectSIS>("ConsultaSIS/GetData?captcha=" + captcha + "&dni=" + dni);
            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

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
                string url = "Pacient/FindPacientByDocNumberOrPersonId?value=" + docNumber;

                Api API = new Api();

                var result = API.Get<PacientCustom>(url);
                if (result == null)
                {
                    var imgcaptcha = API.Get<MessageCustom>("ConsultaSIS/GetCapcha");
                    return new JsonResult { Data = imgcaptcha, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

            }
        }

        public JsonResult AgendarPaciente(ServiceCustom data)
        {
            Api API = new Api();

            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data) },
                { "Int2" , "11"},
                { "Int1" , "9"},
            };
            var result = API.Post<MessageCustom>("Calendar/CreateCalendar", arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}