using Newtonsoft.Json;
using SigesoftWeb.Controllers.Security;
using SigesoftWeb.Models;
using SigesoftWeb.Models.Common;
using SigesoftWeb.Models.Component;
using SigesoftWeb.Models.Lineas;
using SigesoftWeb.Models.Message;
using SigesoftWeb.Models.Protocol;
using SigesoftWeb.Models.ProtocolComponent;
using SigesoftWeb.Models.RUC;
using SigesoftWeb.Models.ServiceOrder;
using SigesoftWeb.Models.SystemUser;
using SigesoftWeb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SigesoftWeb.Controllers.Protocol
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ProtocolController : Controller
    {
        [GeneralSecurity(Rol = "Protocol-Index")]
        public ActionResult Index()
        {
            Api API = new Api();
            ViewBag.TypeService = API.Get<List<Dropdownlist>>("SystemParameter/GetParameterTypeServiceByGrupoId?grupoId=" + ((int)Enums.SystemParameter.TypeService).ToString());
            ViewBag.TipoEso = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.TipoEso).ToString());
            ViewBag.Operador = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.Operador).ToString());
            ViewBag.Genero = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.GenderAll).ToString());
            ViewBag.Genero2 = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.Gender).ToString());
            ViewBag.GrupoEtario = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.GrupoEtario).ToString());
            ViewBag.Empresa = API.Get<List<Dropdownlist>>("SystemParameter/GetOrganizationAndLocation");
            ViewBag.OrgaAseguradora = API.Get<List<Dropdownlist>>("SystemParameter/GetOrganizationForCombo");
            ViewBag.TipoDocumento = API.Get<List<Dropdownlist>>("DataHierarchy/GetDataHierarchyByGrupoId?grupoId=" + ((int)Enums.DataHierarchy.DocType).ToString());
            ViewBag.NivelEstudios = API.Get<List<Dropdownlist>>("DataHierarchy/GetDataHierarchyByGrupoId?grupoId=" + ((int)Enums.DataHierarchy.NivelEstudio).ToString());
            ViewBag.Profesional = API.Get<List<Dropdownlist>>("DataHierarchy/GetDataHierarchyByGrupoId?grupoId=" + ((int)Enums.DataHierarchy.Profesion).ToString());
            ViewBag.EstadoCivil = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.EstadoCivil).ToString());
            ViewBag.TipoProtocolo = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=118");
            ViewBag.StatusOrderService = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=194");
            ViewBag.LineaCredito = API.Get<List<Dropdownlist>>("DataHierarchy/GetDataHierarchyByGrupoId?grupoId=122");
            ViewBag.ExternalPermisos = API.Get<List<Dropdownlist>>("DataHierarchy/GetExternalPermisionForChekedListByTypeId?ExternalUserFunctionalityTypeId=" + ((int)Enums.ExternalUserFunctionalityType.PermisosOpcionesUsuarioExternoWeb).ToString());
            ViewBag.Notificaciones = API.Get<List<Dropdownlist>>("DataHierarchy/GetExternalPermisionForChekedListByTypeId?ExternalUserFunctionalityTypeId=" + ((int)Enums.ExternalUserFunctionalityType.NotificacionesUsuarioExternoWeb).ToString());
            ViewBag.Lineas = API.Get<List<LineasCustom>>("LineaSAMBHS/GetAllLineas");
            return View();
        }

        [GeneralSecurity(Rol = "Protocol-GetDataProtocol")]
        public ActionResult GetProtocolComponentByProtocolId(string protocolId)
        {
            Api API = new Api();
            ViewBag.ProtocolComponent = API.Get<BoardProtocolComponent>("Protocol/GetProtocolComponentByProtocolId?protocolId=" + protocolId);
            return PartialView("_BoardComponentPartial");
        }

        [GeneralSecurity(Rol = "Protocol-GetDataProtocol")]
        public ActionResult GetAllProtocol(BoardProtocol data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data)},
            };
            ViewBag.Protocol = API.Post<BoardProtocol>("Protocol/GetAllProtocol", arg);
            return PartialView("_BoardProtocolsPartial");
        }

        [GeneralSecurity(Rol = "Protocol-GetDataProtocol")]
        public JsonResult GetDataProtocol(string protocolId)
        {
            Api API = new Api();
            var result = API.Get<ProtocolCustom>("Protocol/GetDataProtocol?protocolId=" + protocolId);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Protocol-GetMasterServiceId")]
        public JsonResult GetMasterServiceId(string TypeService)
        {
            Api API = new Api();
            var result = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroMasterServiceByGrupoId?serviceType=" + TypeService);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Protocol-GetGESO")]
        public JsonResult GetGESO(string organizationId)
        {
            var orgloc = organizationId.Split('|');
            Api API = new Api();
            var result = API.Get<List<Dropdownlist>>("SystemParameter/GetGESO?organizationId=" + orgloc[0] + "&locationId=" + orgloc[1]);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Protocol-GetMedicalExam")]
        public JsonResult GetMedicalExam(string componentName)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = 500000000;
            Api API = new Api();
            var result = API.Get<List<ExamsCustom>>("Component/GetMedicalExam?componentName=" + componentName);
            var json = new JsonResult
            {
                Data = result,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            json.MaxJsonLength = 500000000;

            return json;
        }

        [GeneralSecurity(Rol = "Protocol-SaveProtocols")]
        public JsonResult VerifyExistsProtocol(string protocolName)
        {

            Api API = new Api();

            var result = API.Get<MessageCustom>("Protocol/VerifyExistsProtocol?protocolName=" + protocolName);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Protocol-SaveProtocols")]
        public JsonResult SaveProtocols(ProtocolList data)
        {

            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data) },
                { "Int1", ViewBag.USER.SystemUserId.ToString() },
                { "Int2", ViewBag.USER.NodeId.ToString() },
            };

            var result = API.Post<MessageCustom>("Protocol/SaveProtocols", arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Protocol-GetProtocolComponents")]
        public JsonResult GetProtocolComponentByProtocolIdJSON(string protocolId)
        {

            Api API = new Api();
            var result = API.Get<BoardProtocolComponent>("Protocol/GetProtocolComponentByProtocolId?protocolId=" + protocolId);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Protocol-GetProtocolComponents")]
        public JsonResult DeletedProtocolComponent(string protocolComponentId)
        {
            Api API = new Api();
            var result = API.Get<MessageCustom>("Protocol/DeletedProtocolComponent?protocolComponentId=" + protocolComponentId + "&userId=" + ViewBag.User.SystemUserId.ToString());
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Protocol-NewSystemUserExternal")]
        public JsonResult NewSystemUserExternal(BoardSystemUserExternal data)
        {
            Api API = new Api();
            data.EntitySystemUser.i_SystemUserId = data.EntitySystemUser.i_SystemUserId == null ? -1 : data.EntitySystemUser.i_SystemUserId;
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data) },
                { "Int1", ViewBag.USER.SystemUserId.ToString() },
                { "Int2", ViewBag.USER.NodeId.ToString() },
            };

            var result = API.Post<MessageCustom>("SystemUser/NewSystemUserExternal", arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Protocol-GetSecuentialForServiceOrder")]
        public JsonResult GetSecuentialForServiceOrder()
        {
            Api API = new Api();

            var result = API.Get<int>("Protocol/GetSecuentialForServiceOrder?nodeId=" + ViewBag.USER.NodeId.ToString() + "&tableId=101" );
            string resp = result.ToString() + "-" + ViewBag.USER.NodeId.ToString();
            return new JsonResult { Data = resp, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Protocol-GetPriceProtocolComponentByProtocolId")]
        public JsonResult GetPriceProtocolComponentByProtocolId(string protocolId)
        {
            Api API = new Api();

            var result = API.Get<float>("Protocol/GetPriceProtocolComponentByProtocolId?protocolId=" + protocolId);
            string resp = result.ToString("N2");
            return new JsonResult { Data = resp, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Protocol-AddServiceOrder")]
        public JsonResult AddServiceOrder(BoardServiceOrder data)
        {
            Api API = new Api();

            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data) },
                { "Int1", ViewBag.USER.SystemUserId.ToString() },
                { "Int2", ViewBag.USER.NodeId.ToString() },
            };

            var result = API.Post<MessageCustom>("ServiceOrder/AddServiceOrder", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Protocol-GenerateServiceOrderReport")]
        public JsonResult GenerateServiceOrderReport(BoardServiceOrder data)
        {
            Api API = new Api();

            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data) },
                { "Int1", ViewBag.USER.SystemUserId.ToString() },
            };

            var result = API.Post<MessageCustom>("ServiceOrder/GenerateServiceOrderReport", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Protocol-GenerateServiceOrderReport")]
        public JsonResult GetServicesOrderDetail(string serviceOrderId)
        {
            Api API = new Api();

            var result = API.Get<List<ServiceOrderDetailCustom>>("ServiceOrder/GetServicesOrderDetail?serviceOrderId=" + serviceOrderId);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [GeneralSecurity(Rol = "Protocol-GetSystemUserById")]
        public JsonResult GetSystemUserById(string systemUserId)
        {
            Api API = new Api();

            var result = API.Get<SystemUserCustom>("SystemUser/GetSystemUserById?systemUserId=" + systemUserId);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Protocol-DeletedSystemUser")]
        public JsonResult DeletedSystemUser(string systemUserId)
        {
            Api API = new Api();

            var result = API.Get<MessageCustom>("SystemUser/DeletedSystemUser?systemUserId=" + systemUserId);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Protocol-SearchOrganizations")]
        public JsonResult SearchOrganizations(string value)
        {
            Api API = new Api();

            var result = API.Get<List<string>>("Organization/SearchOrganizations?value=" + value);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [GeneralSecurity(Rol = "Protocol-GetCaptchaBusquedaRuc")]
        public JsonResult GetCaptchaBusquedaRuc()
        {
            Api API = new Api();

            var result = API.Get<MessageCustom>("ConsultaRUC/GetCaptcha");

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Protocol-GetCaptchaBusquedaRuc")]
        public JsonResult GetDataContribuyente(string captcha, string value, string tipoBusqueda)
        {
            Api API = new Api();

            var result = API.Get<RootObjectSIS>("ConsultaRUC/GetData?captcha=" + captcha + "&value=" + value + "&tipoBusqueda=" + tipoBusqueda);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Protocol-GetCaptchaBusquedaRuc")]
        public JsonResult DeletedServiceOrderDetail(string serviceOrderDetail)
        {
            Api API = new Api();

            var result = API.Get<MessageCustom>("ServiceOrder/DeletedServiceOrderDetail?serviceOrderDetail=" + serviceOrderDetail + "&userId=" + ViewBag.USER.SystemUserId.ToString());

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Protocol-SendEmail")]
        public JsonResult SendEmail(EmailModel data)
        {
            Api API = new Api();

            EmailModel model = new EmailModel();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data) },

            };

            var result = API.Post<MessageCustom>("ServiceOrder/SendEmail", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        
    }
}