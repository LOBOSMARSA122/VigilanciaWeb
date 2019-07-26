using HtmlAgilityPack;
using Newtonsoft.Json;
using SigesoftWeb.Controllers.Security;
using SigesoftWeb.Models;
using SigesoftWeb.Models.Calendar;
using SigesoftWeb.Models.Clients;
using SigesoftWeb.Models.Common;
using SigesoftWeb.Models.Component;
using SigesoftWeb.Models.Message;
using SigesoftWeb.Models.Pacient;
using SigesoftWeb.Models.Product;
using SigesoftWeb.Models.RUC;
using SigesoftWeb.Models.Service;
using SigesoftWeb.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SigesoftWeb.Controllers.Calendar
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class AgendaController : Controller
    {
        public AgendaController()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            _myCookie = new CookieContainer();
        }
        #region Fields
        public enum Resul
        {
            Ok = 0,
            NoResul = 1,
            ErrorCapcha = 2,
            Error = 3,
        }
        private readonly CookieContainer _myCookie;
        #endregion

        [GeneralSecurity(Rol = "Agenda-Index")]
        public ActionResult Index()
        {
            Api API = new Api();

            ViewBag.TypeService = API.Get<List<Dropdownlist>>("SystemParameter/GetParameterTypeServiceByGrupoId?grupoId=" + ((int)Enums.SystemParameter.TypeService).ToString());

            ViewBag.Modalidad = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.Modalidad).ToString());
            ViewBag.LineStatus = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.LineStatus).ToString());
            ViewBag.Vip = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.Vip).ToString());
            ViewBag.CalendarStatus = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.CalendarStatus).ToString());

            ViewBag.EstadoCivil = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.EstadoCivil).ToString());
            ViewBag.Genero = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.Gender).ToString());
            ViewBag.TipoDocumento = API.Get<List<Dropdownlist>>("DataHierarchy/GetDataHierarchyByGrupoId?grupoId=" + ((int)Enums.DataHierarchy.DocType).ToString());
            ViewBag.NivelEstudios = API.Get<List<Dropdownlist>>("DataHierarchy/GetDataHierarchyByGrupoId?grupoId=" + ((int)Enums.DataHierarchy.NivelEstudio).ToString());
            ViewBag.GrupoSanguineo = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.GrupoSanguineo).ToString());
            ViewBag.Distrito = API.Get<List<Dropdownlist>>("DataHierarchy/GetDataHierarchyByGrupoId?grupoId=" + ((int)Enums.SystemParameter.DistProvDep).ToString());
            ViewBag.FactorSanguineo = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.FactorSanguineo).ToString());
            ViewBag.TipoSeguro = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.TipoSeguro).ToString());
            ViewBag.ResideLugar = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.ResideLugar).ToString());
            ViewBag.AltitudLabor = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.AltitudLabor).ToString());
            ViewBag.LugarLabor = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.LugarLabor).ToString());
            ViewBag.PuestoActuPostu = API.Get<List<Dropdownlist>>("SystemParameter/GetPuestos");
            ViewBag.EmpresaFacturacion = API.Get<List<Dropdownlist>>("SystemParameter/GetEmpresaFacturacion");
            ViewBag.Empresa = API.Get<List<Dropdownlist>>("SystemParameter/GetOrganizationAndLocation");
            ViewBag.TipoEso = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.TipoEso).ToString());
            ViewBag.Parentesco = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=" + ((int)Enums.SystemParameter.Parentesco).ToString());
            ViewBag.Users = API.Get<List<Dropdownlist>>("SystemParameter/GetUser");
            ViewBag.CentroCosto = API.Get<List<Dropdownlist>>("SystemParameter/GetCentroCosto");
            ViewBag.Titulares = API.Get<List<Dropdownlist>>("SystemParameter/GetTitulares");

            ViewBag.Document = API.Get<List<KeyValueDTO>>("Documento/GetDocumentsForCombo?UsadoCompras=0&UsadoVentas=1");

            ViewBag.Moneda = API.Get<List<Dropdownlist>>("DataHierarchy/GetDataHierarchySAMBHSByGrupoId?grupoId=" + ((int)Enums.DataHierarchy.Moneda).ToString());
            ViewBag.CondicionPago = API.Get<List<Dropdownlist>>("DataHierarchy/GetDataHierarchySAMBHSByGrupoId?grupoId=" + ((int)Enums.DataHierarchy.CondicionPago).ToString());
            ViewBag.TipoPago = API.Get<List<Dropdownlist>>("DataHierarchy/GetDataHierarchySAMBHSByGrupoId?grupoId=" + ((int)Enums.DataHierarchy.TipoPago).ToString());
            ViewBag.Vendedor = API.Get<List<Dropdownlist>>("DataHierarchy/GetVendedorSAMBHS");


            return View();


        }

        [GeneralSecurity(Rol = "Agenda-GetMasterService")]
        public JsonResult GetMasterServiceId(string TypeService)
        {
            Api API = new Api();
            var result = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroMasterServiceByGrupoId?serviceType=" + TypeService);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Agenda-BoardCalendar")]
        public ActionResult GetDataCalendar(BoardCalendar data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data)},
            };
            ViewBag.Calendar = API.Post<BoardCalendar>("Calendar/GetDataCalendar", arg);

            return PartialView("_BoardCalendarPartial");
        }

        [GeneralSecurity(Rol = "Agenda-GetProtocols")]
        public JsonResult GetProtocolsForCombo(string Service, string ServiceType)
        {
            Api API = new Api();
            var result = API.Get<List<Dropdownlist>>("SystemParameter/GetProtocolsForCombo?Service=" + Service + "&ServiceType=" + ServiceType);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Agenda-GetMasterService")]
        public JsonResult GetGESO(string organizationId, string locationId)
        {
            Api API = new Api();
            var result = API.Get<List<Dropdownlist>>("SystemParameter/GetGESO?organizationId=" + organizationId + "&locationId=" + locationId);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Agenda-GetProvincia")]
        public JsonResult GetProvincia(string name)
        {
            Api API = new Api();
            var result = API.Get<List<Dropdownlist>>("DataHierarchy/GetProvincia?name=" + name);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Agenda-GetDepartamento")]
        public JsonResult GetDepartamento(int id)
        {
            Api API = new Api();
            var result = API.Get<List<Dropdownlist>>("DataHierarchy/GetDepartamento?idProv=" + id);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Agenda-GetDepartamento")]
        public JsonResult AgendarPaciente(ServiceCustom data)
        {
            Api API = new Api();

            string nodo = ViewBag.USER == null ? "9" : ViewBag.USER.NodeId == null ? "9" : ViewBag.USER.NodeId.ToString();
            string userId = ViewBag.USER == null ? "11" : ViewBag.USER.SystemUserId == null ? "11" : ViewBag.USER.SystemUserId.ToString();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data) },
                { "Int2" , ViewBag.USER.SystemUserId.ToString()},
                { "Int1" , ViewBag.USER.NodeId.ToString()},
            };
            var result = API.Post<MessageCustom>("Calendar/CreateCalendar", arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Agenda-UpdateServiceForProtocol")]
        public JsonResult UpdateServiceForProtocol(ServiceCustom data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data) },
                { "Int1" , ViewBag.USER.SystemUserId.ToString()},
            };
            var result = API.Post<MessageCustom>("Calendar/UpdateServiceForProtocol", arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Agenda-RegistrarCarta")]
        public JsonResult RegistrarCarta(string serviceId, string solicitud)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", serviceId },
                { "String2", solicitud },
                { "Int1" , ViewBag.USER.SystemUserId.ToString()},
            };
            var result = API.Post<MessageCustom>("Calendar/RegistrarCarta", arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Agenda-FusionarServicios")]
        public JsonResult FusionarServicios(string ListserviceId)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", ListserviceId },
                { "Int1", ViewBag.USER.NodeId.ToString() },
                { "Int2" , ViewBag.USER.SystemUserId.ToString()},
            };
            var result = API.Post<MessageCustom>("Calendar/FusionarServicios", arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Agenda-BoardClientes")]
        public ActionResult GetClientes(BoardCliente data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data)},
            };

            ViewBag.Clients = API.Post<BoardCliente>("Clientes/GetClientes", arg);

            return PartialView("_BoardClientsPartial");
        }

        [GeneralSecurity(Rol = "Agenda-BoardProducts")]
        public ActionResult GetProductsSAMBHS(BoardProductsSAMBHS data)
        {
            data.RucEmpresa = ViewBag.USER.RucEmpresa;
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data)},
            };

            ViewBag.Pacients = API.Post<BoardProductsSAMBHS>("Product/GetProductsSAMBHS", arg);

            return PartialView("_BoardProducts");
        }


        [GeneralSecurity(Rol = "Agenda-CargarImagen")]
        public JsonResult CargarImagen()
        {
            try
            {

                Bitmap bm = new Bitmap(ReadCapcha());
                var result = "";
                using (MemoryStream ms = new MemoryStream())
                {
                    bm.Save(ms, ImageFormat.Jpeg);
                    result = Convert.ToBase64String(ms.ToArray());
                }
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                Bitmap bm = new Bitmap(ReadCapcha());
                var result = "";
                using (MemoryStream ms = new MemoryStream())
                {
                    bm.Save(ms, ImageFormat.Jpeg);
                    result = Convert.ToBase64String(ms.ToArray());
                }
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        //[GeneralSecurity(Rol = "Agenda-GetDataContribuyenteByRuc")]
        //public JsonResult GetDataContribuyenteByRuc(string numDoc, string textCaptcha)
        //{
        //    RootObject _RootObject = new RootObject();
        //    var tipoPersona = numDoc.Substring(0, 1) == "2" ? "1" : "0";
        //    var myUrl = string.Format("http://e-consultaruc.sunat.gob.pe/cl-ti-itmrconsruc/jcrS00Alias?accion=consPorRuc&nroRuc={0}&codigo={1}&tipdoc={2}", numDoc, textCaptcha, tipoPersona);

        //    var myWebRequest = (HttpWebRequest)WebRequest.Create(myUrl);
        //    myWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0";//esto creo que lo puse por gusto :/
        //    myWebRequest.CookieContainer = _myCookie;
        //    myWebRequest.Credentials = CredentialCache.DefaultCredentials;
        //    myWebRequest.Proxy = null;
        //    myWebRequest.ContentType = "text/xml;charset=\"utf-8\"";
        //    var myHttpWebResponse = (HttpWebResponse)myWebRequest.GetResponse();

        //    var myStream = myHttpWebResponse.GetResponseStream();
        //    if (myStream == null)
        //    {
        //        _RootObject.success = (int)Resul.Error;
        //        _RootObject.mensaje = "Error Desconocido, vuelva intentar.";
        //    }

        //    var myStreamReader = new StreamReader(myStream, Encoding.GetEncoding("ISO-8859-1"));
        //    var s = myStreamReader.ReadToEnd();

        //    HtmlDocument document = new HtmlDocument();
        //    document.LoadHtml(s);
        //    var tabla = document.DocumentNode.SelectSingleNode("//*[contains(@class,'form-table')]");

        //    if (tabla == null)
        //    {
        //        _RootObject.success = (int)Resul.ErrorCapcha;
        //        _RootObject.mensaje = "Ingrese el captcha correctamente.";
        //    }
        //    if (_RootObject.success != (int)Resul.ErrorCapcha && _RootObject.success != (int)Resul.Error)
        //    {
        //        var tablaSunat = tabla.InnerText;
        //        var resul = tablaSunat.Split(new[] { '\r', '\n', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

        //        var indexRs = resul.FindIndex(p => p.Contains("N&uacute;mero de RUC:")) + 1;
        //        var indexTipoC = resul.FindIndex(p => p.Contains("Tipo Contribuyente:")) + 1;
        //        var indexNombreC = resul.FindIndex(p => p.Contains("Nombre Comercial:")) + 1;
        //        var indexFechaInsc = resul.FindIndex(p => p.Contains("Fecha de Inscripci&oacute;n:")) + 1;
        //        var indexFechaInicioAc = resul.FindIndex(p => p.Contains("Fecha de Inicio de Actividades:")) + 1;
        //        var indexEstadoC = resul.FindIndex(p => p.Contains("Estado del Contribuyente:")) + 1;
        //        var indexCondicionC = resul.FindIndex(p => p.Contains("Condici&oacute;n del Contribuyente:")) + 3;
        //        var indexDireccionC = resul.FindIndex(p => p.Contains("Direcci&oacute;n del Domicilio Fiscal:")) + 1;


        //        var razonSocial = resul[indexRs];
        //        _RootObject.success = (int)Resul.Ok;
        //        _RootObject.result.razon_social = razonSocial.Remove(0, razonSocial.IndexOf('-') + 1);

        //        _RootObject.result.contribuyente_tipo = resul[indexTipoC].Trim();
        //        _RootObject.result.nombre_comercial = resul[indexNombreC].Trim();
        //        _RootObject.result.fecha_inscripcion = resul[indexFechaInsc].Trim();
        //        _RootObject.result.fecha_actividad = resul[indexFechaInicioAc].Trim();
        //        _RootObject.result.contribuyente_estado = resul[indexEstadoC].Trim();
        //        _RootObject.result.condicion = resul[indexCondicionC].Trim();
        //        _RootObject.result.domicilio_fiscal = resul[indexDireccionC].Trim();
        //    }


        //    return new JsonResult { Data = _RootObject, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}

        private Image ReadCapcha()
        {
            var myWebRequest = (HttpWebRequest)WebRequest.Create("http://e-consultaruc.sunat.gob.pe/cl-ti-itmrconsruc/captcha?accion=image");
            myWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0";//esto creo que lo puse por gusto :/
            myWebRequest.CookieContainer = _myCookie;
            myWebRequest.Credentials = CredentialCache.DefaultCredentials;
            myWebRequest.Proxy = null;
            myWebRequest.ContentType = "text/xml;charset=\"utf-8\"";
            myWebRequest.Accept = "text/xml";
            myWebRequest.Method = "POST";

            using (var myWebResponse = myWebRequest.GetResponse())
            {
                var myImgStream = myWebResponse.GetResponseStream();
                return myImgStream != null ? Image.FromStream(myImgStream) : null;
            }
        }

        [GeneralSecurity(Rol = "Agenda-GetListSaldosPaciente")]
        public JsonResult GetListSaldosPaciente(string serviceId)
        {
            Api API = new Api();

            var result = API.Get<List<SaldoPaciente>>("Calendar/GetListSaldosPaciente?serviceId=" + serviceId);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Agenda-IniciarCircuito")]
        public JsonResult IniciarCircuito(string CalendarId)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", CalendarId },
                { "Int1", ViewBag.USER.SystemUserId.ToString() },
            };
            var result = API.Post<MessageCustom>("Calendar/IniciarCircuito", arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Agenda-GetExamenesAdicionales")]
        public JsonResult GetExamenesAdicionales(string ServiceId)
        {
            Api API = new Api();

            var result = API.Get<List<Categoria>>("Calendar/GetExamenesAdicionales?ServiceId=" + ServiceId);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "Agenda-GetExamenesAdicionales")]
        public JsonResult SaveAdditionalExamsForCalendar(string data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", data },
                { "Int1", ViewBag.USER.SystemUserId.ToString() },
                { "Int2", ViewBag.USER.NodeId.ToString() },
            };
            var result = API.Post<MessageCustom>("Calendar/SaveAdditionalExamsForCalendar", arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [GeneralSecurity(Rol = "Agenda-GetAllComponentsByService")]
        public JsonResult GetAllComponentsByService(string serviceId)
        {
            Api API = new Api();
            var result = API.Get<List<Categoria>>("Service/GetAllComponentsByService?serviceId=" + serviceId);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}