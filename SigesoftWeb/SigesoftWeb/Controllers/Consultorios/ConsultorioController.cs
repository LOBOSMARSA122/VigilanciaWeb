using Newtonsoft.Json;
using SigesoftWeb.Controllers.Security;
using SigesoftWeb.Models;
using SigesoftWeb.Models.Calendar;
using SigesoftWeb.Models.Common;
using SigesoftWeb.Models.Component;
using SigesoftWeb.Models.Message;
using SigesoftWeb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SigesoftWeb.Controllers.Consultorios
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ConsultorioController : Controller
    {
        [GeneralSecurity(Rol = "Consultorio-Index")]
        public ActionResult Index()
        {
            Api API = new Api();
            string roleId = "1";//Verificar esto, manda 0 ViewBag.USER.Permissions[0].RoleId.ToString();
            string nodeId = ViewBag.USER.NodeId.ToString();
            ViewBag.TypeService = API.Get<List<Dropdownlist>>("SystemParameter/GetParameterTypeServiceByGrupoId?grupoId=" + ((int)Enums.SystemParameter.TypeService).ToString());
            ViewBag.Servicios = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroMasterServiceByGrupoId?serviceType=9");
            ViewBag.Componentes = API.Get<List<KeyValueDTO>>("Component/GetAllComponent?nodeId=" + nodeId + "&roleNodeId=" + roleId);
            return View();
        }

        [GeneralSecurity(Rol = "Consultorio-GetWaitingPacient")]
        public ActionResult GetWaitingPacient(string i_ServiceId, string componentId, string categoryId)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1" , DateTime.Now.Date.ToString() },
                { "String2" , componentId },
                { "Int2" , categoryId},
                { "Int3" , ViewBag.USER.SystemUserId.ToString()},
                { "Int1" , i_ServiceId },


            };
            var url = "";
            if (categoryId == "10")
            {
                url = "Calendar/GetPacientInLineByComponentId";
            }
            else
            {
                url = "Calendar/GetPacientInLineByComponentId_Atx";
            }

            ViewBag.WaitingPacients = API.Post<List<CalendarList>>(url, arg);
            return PartialView("_BoardWaitingPacient");
        }

        [GeneralSecurity(Rol = "Consultorio-GetExamenes")]
        public ActionResult GetExamenes(BoardExamsCustom data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1" , JsonConvert.SerializeObject(data) },
            };
            ViewBag.ExamsPacient = API.Post<BoardExamsCustom>("Consultorio/GetExamenes", arg);
            return PartialView("_BoardExamenPartial");
        }

        [GeneralSecurity(Rol = "Consultorio-CallingPacient")]
        public JsonResult CallingPacient(BoardExamsCustom data)
        {

            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data) },
            };
            var result = API.Post<BoardExamsCustom>("Consultorio/CallingPacient", arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Consultorio-EliminarExamen")]
        public JsonResult EliminarExamen(BoardExamsCustom data)
        {

            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data) },
                { "Int1", ViewBag.USER.SystemUserId.ToString() },
            };
            var result = API.Post<MessageCustom>("Consultorio/EliminarExamen", arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Consultorio-GetAdditionalExams")]
        public JsonResult GetAdditionalExams(BoardExamsCustom data)
        {

            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data) },
            };
            var result = API.Post<List<Categoria>>("Consultorio/GetAdditionalExams", arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [GeneralSecurity(Rol = "Consultorio-BuscarCoincidencia")]
        public JsonResult BuscarCoincidencia(string componentId, string serviceId)
        {

            Api API = new Api();
            var result = API.Get<bool>("Consultorio/BuscarCoincidencia?componentId=" + componentId + "&serviceId=" + serviceId);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Consultorio-SaveExamsAdditional")]
        public JsonResult SaveExamsAdditional(string data)
        {

            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", data },
                { "Int1", ViewBag.USER.SystemUserId.ToString() },
                { "Int2", ViewBag.USER.NodeId.ToString() },
            };
            var result = API.Post<MessageCustom>("Consultorio/SaveExamsAdditional", arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Consultorio-LiberarPaciente")]
        public JsonResult LiberarPaciente(string Category, string ServiceId, string CategoryName, string CalendarId)
        {

            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "Int1", Category },
                { "String1", ServiceId },
                { "String2", CategoryName },
                { "String3", CalendarId },
            };
            var result = API.Post<MessageCustom>("Consultorio/LiberarPaciente", arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Consultorio-GetExamenes")]
        public ActionResult ViewAdditionalExams(string serviceId)
        {
            Api API = new Api();
            ViewBag.Components = API.Get<List<KeyValueDTO>>("Consultorio/GetAllComponent");
            ViewBag.AdditionalExams = API.Get<List<AdditionalExamUpdate>>("Consultorio/ViewAdditionalExams?serviceId=" + serviceId + "&userId=" + ViewBag.USER.SystemUserId.ToString());
            return PartialView("_BoardAdditionalExams");
        }

        [GeneralSecurity(Rol = "Consultorio-SaveUpdateAdditionalExam")]
        public JsonResult SaveUpdateAdditionalExam(string additionalExamId, string componentId, string serviceId, string comentario)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "Int1", ViewBag.USER.SystemUserId.ToString() },
                { "String1", additionalExamId },
                { "String2", componentId },
                { "String3", serviceId },
                { "String4", comentario },
            };
            var result = API.Post<MessageCustom>("Consultorio/SaveUpdateAdditionalExam", arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Consultorio-SaveDeletedAdditionalExam")]
        public JsonResult SaveDeletedAdditionalExam(string additionalExamId, string comentario, string serviceId)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "Int1", ViewBag.USER.SystemUserId.ToString() },
                { "String1", additionalExamId },
                { "String2", comentario },
                { "String3", serviceId },
            };
            var result = API.Post<MessageCustom>("Consultorio/SaveDeletedAdditionalExam", arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Consultorio-ReImprimirAdicionales")]
        public JsonResult ReImprimirAdicionales(string serviceId)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "Int1", ViewBag.USER.SystemUserId.ToString() },
                { "String1", serviceId },
            };
            var result = API.Post<MessageCustom>("Consultorio/ReImprimirAdicionales", arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}