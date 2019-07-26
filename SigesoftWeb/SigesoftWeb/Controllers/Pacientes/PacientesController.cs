using Newtonsoft.Json;
using SigesoftWeb.Controllers.Security;
using SigesoftWeb.Models;
using SigesoftWeb.Models.Common;
using SigesoftWeb.Models.Message;
using SigesoftWeb.Models.Pacient;
using SigesoftWeb.Models.RUC;
using SigesoftWeb.Utils;
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
            Api API = new Api();
            ViewBag.EstadoCivil = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.EstadoCivil).ToString());
            ViewBag.TipoDocumento = API.Get<List<Dropdownlist>>("DataHierarchy/GetDataHierarchyByGrupoId?grupoId=" + ((int)Enums.DataHierarchy.DocType).ToString());
            ViewBag.Genero = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.Gender).ToString());
            ViewBag.NivelEstudios = API.Get<List<Dropdownlist>>("DataHierarchy/GetDataHierarchyByGrupoId?grupoId=" + ((int)Enums.DataHierarchy.NivelEstudio).ToString());
            ViewBag.GrupoSanguineo = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.GrupoSanguineo).ToString());
            ViewBag.Distrito = API.Get<List<Dropdownlist>>("DataHierarchy/GetDataHierarchyByGrupoId?grupoId=" + ((int)Enums.SystemParameter.DistProvDep).ToString());
            ViewBag.FactorSanguineo = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.FactorSanguineo).ToString());
            ViewBag.TipoSeguro = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.TipoSeguro).ToString());
            ViewBag.ResideLugar = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.ResideLugar).ToString());
            ViewBag.AltitudLabor = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.AltitudLabor).ToString());
            ViewBag.LugarLabor = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.LugarLabor).ToString());
            ViewBag.PuestoActuPostu = API.Get<List<Dropdownlist>>("SystemParameter/GetPuestos");
            ViewBag.Parentesco = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.Parentesco).ToString());
            ViewBag.Titulares = API.Get<List<Dropdownlist>>("SystemParameter/GetTitulares");

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

        [GeneralSecurity(Rol = "Pacientes-GetPacientByDocNumber")]
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

        [GeneralSecurity(Rol = "Pacientes-FindPacientByPersonId")]
        public JsonResult GetPacientByPersonId(string personId)
        {
            string url = "Pacient/FindPacientByDocNumberOrPersonId?value=" + personId;
            Api API = new Api();

            var result = API.Get<PacientCustom>(url);


            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Pacientes-GetDataPacient")]
        public ActionResult GetDataPacient(BoardPacients data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1" , JsonConvert.SerializeObject(data)},
            };
            ViewBag.Pacients = API.Post<BoardPacients>("Pacient/GetAllPacient", arg);
            return PartialView("_BoardPacientPartial");
        }
    }
}