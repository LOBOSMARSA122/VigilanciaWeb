using SigesoftWeb.Models;
using SigesoftWeb.Models.Common;
using System.Collections.Generic;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using SigesoftWeb.Utils;
using SigesoftWeb.Controllers.Security;

namespace SigesoftWeb.Controllers.Common
{
    public class PacientController : Controller
    {
        [GeneralSecurity(Rol = "Pacient-BoardPacient")]
        public ActionResult Index()
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "grupoId" , ((int)Enums.DataHierarchy.DocType).ToString() }
            };

            ViewBag.DocType = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("DataHierarchy/GetDataHierarchyByGrupoId", arg), Constants.All);
            return View();
        }

        public ActionResult FilterPacient(BoardPacient data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "Pacient",data.Pacient},
                { "DocTypeId", data.DocTypeId.ToString()},
                { "DocNumber", data.DocNumber},
                { "Index", data.Index.ToString()},
                { "Take", data.Take.ToString()}
            };
            ViewBag.Pacients = API.Post<BoardPacient>("Pacient/GetBordPacients", arg);
            return PartialView("_BoardPacientsPartial");
        }

        [GeneralSecurity(Rol = "Pacient-CreatePacient")]
        public ActionResult CreatePacient(string id)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "grupoId" , ((int)Enums.DataHierarchy.DocType).ToString() }
            };

            ViewBag.DocType = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("DataHierarchy/GetDataHierarchyByGrupoId", arg), Constants.Select);
            ViewBag.Pacient = API.Get<Pacients>("Pacient/GetPacientById", new Dictionary<string, string> { { "pacientId", id } });
            return View();
        }

        [GeneralSecurity(Rol = "Pacient-CreatePacient")]
        public ActionResult Pacient()
        {
            ViewBag.Pacient = new BoardPacient() { List = new List<Pacients>(), Take = 10 };
            return View();
        }

        [GeneralSecurity(Rol = "Pacient-CreatePacient")]
        public JsonResult DeletePacient(string id)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "String1", id.ToString() },
                { "Int2", ViewBag.USER.SystemUserId.ToString() }
            };
            bool response = API.Post<bool>("Pacient/DeletePacient", args);
            return Json(response);
        }

        [GeneralSecurity(Rol = "Pacient-CreatePacient")]
        public JsonResult EditPacient(Pacients data)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "String1", JsonConvert.SerializeObject(data) },
                { "Int1", ViewBag.USER.SystemUserId.ToString() }
            };
            bool response = API.Post<bool>("Pacient/EditPacient", args);
            return Json(response);
        }

        [GeneralSecurity(Rol = "Pacient-CreatePacient")]
        public JsonResult AddPacient(Pacients pacient)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "String1", JsonConvert.SerializeObject(pacient) },
                { "Int1", ViewBag.USER.SystemUserId.ToString() }
            };
            bool response = API.Post<bool>("Pacient/AddPacient", args);
            return Json(response);
        }
    }

}
