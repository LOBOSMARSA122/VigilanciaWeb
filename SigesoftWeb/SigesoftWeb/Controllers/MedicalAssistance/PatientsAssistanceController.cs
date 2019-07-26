using Newtonsoft.Json;
using SigesoftWeb.Controllers.Security;
using SigesoftWeb.Models;
using SigesoftWeb.Models.Common;
using SigesoftWeb.Models.Eso;
using SigesoftWeb.Models.MedicalAssistance;
using SigesoftWeb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using SigesoftWeb.Models.Vigilancia;
using SigesoftWeb.Models.Schedule;
using SigesoftWeb.Models.Component;
using SigesoftWeb.Models.Diagnostic;
using SigesoftWeb.Models.ReportManagerBe;
using SigesoftWeb.Models.ServiceComponent;
using SigesoftWeb.Models.Service;
using System.IO;
using SigesoftWeb.Models.Security;

using SigesoftWeb.Models.Antecedentes;
using SigesoftWeb.Models.Pacient;
using SigesoftWeb.Models.PlanIntegral;
using SigesoftWeb.Models.Message;
using SigesoftWeb.Models.Embarazo;
using SigesoftWeb.Models.AntecedentesGenerales;
using System.Net;
using static BE.Eso.RecipesCustom;

namespace SigesoftWeb.Controllers.MedicalAssistance
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class PatientsAssistanceController : Controller
    {
        [GeneralSecurity(Rol = "PatientsAssistance-BoardPatientsAssistance")]
        public ActionResult Index()
        {

            Api API = new Api();
            var organizationId = ViewBag.USER.SystemUserByOrganizationId;
            var doctorResponsibleId = ViewBag.USER.SystemUserId;
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "grupoId" , ((int)Enums.SystemParameter.NotificationType).ToString() },
            };

            ViewBag.NotificationType = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId", arg);

            return View();
        }
        [GeneralSecurity(Rol = "PatientsAssistance-BoardPatientsAssistance")]
        public JsonResult LoadComboVigilancia(string organizationId)
        {
            var user = ViewBag.USER.SystemUserId;

            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "organizationId", organizationId}
            };

            string url = "PlanVigilancia/ComboPlanesVigilancia";
            var result = API.Get<List<Dropdownlist>>(url, arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        [GeneralSecurity(Rol = "PatientsAssistance-BoardPatientsAssistance")]
        public ActionResult FilterPacient(BoardPatient data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "Patient",data.Patient},
                { "Workerstatus", data.Workerstatus.ToString()},
                { "SystemUserByOrganizationId", data.OrganizationId },
                { "PlanVigilanciaId", data.PlanVigilanciaId},
                { "OrganizationId", data.OrganizationId},
                { "SystemUserId", ViewBag.USER.SystemUserId == (int)Enums.UserId.Sa ? "-1" : ViewBag.USER.SystemUserId.ToString() },
                //{ "EmployerOrganizationId", data.EmployerOrganizationId },
                //{ "EmployerOrganizationId", "-1" },
                { "Index", data.Index.ToString()},
                { "Take", data.Take.ToString()}
            };

            ViewBag.Services = API.Post<BoardPatient>("PatientsAssistance/FilterWorkers", arg);
            ViewBag.PendingReview = API.Get<int?>("PatientsAssistance/GetPendingReview?systemUserId=" + ViewBag.USER.SystemUserId.ToString() + "&organizationId=" + data.OrganizationId);
            return PartialView("_BoardPatientsAssistancePartial");

        }

        [GeneralSecurity(Rol = "PatientsAssistance-MedicalConsultation")]
        public ActionResult MedicalConsultation(string id, string serviceId, string organizationId)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "patientId",id}
            };

            Dictionary<string, string> arg2 = new Dictionary<string, string>()
            {
                { "grupoId" , ((int)Enums.SystemParameter.AuditType).ToString() },
            };


            Dictionary<string, string> arg3 = new Dictionary<string, string>()
            {
                { "patientId", id},
                { "organizationId", organizationId},
            };

            Dictionary<string, string> arg4 = new Dictionary<string, string>()
            {
                { "grupoId" , ((int)Enums.SystemParameter.EstateEso).ToString() },
            };

            ViewBag.ServiceId = serviceId;
            ViewBag.PersonId = id;
            ViewBag.AntecedentesEmbarazo = API.Get<List<EmbarazoCustom>>("Embarazo/GetEmbarazo?personId=" + id);
            ViewBag.AtencedentesEso = API.Get<BoardEsoAntecedentes>("Antecedentes/ObtenerEsoAntecedentesPorGrupoId?PersonId=" + id);
            ViewBag.EstateEso = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId", arg4);
            ViewBag.EstadoCivil = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=101");
            ViewBag.GradoInstruccion = API.Get<List<Dropdownlist>>("DataHierarchy/GetDataHierarchyByGrupoId?grupoId=108");
            ViewBag.Pacient = API.Get<PacientCustom>("Pacient/FindPacientByDocNumberOrPersonId?value=" + id);
            ViewBag.DataCuidadosPreventivos = API.Get<List<EsoCuidadosPreventivosFechas>>("Antecedentes/ObtenerFechasCuidadosPreventivos?PersonId=" + id);
            ViewBag.Genero = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=100");
            ViewBag.DataGeneral = API.Get<BoardGenerales>("Antecedentes/GetDataGeneral?personId=" + id);
            ViewBag.Afiliacion = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=188");



            ViewBag.EsControlado = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=111");
            ViewBag.TipoPlan = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=281");
            ViewBag.GrupoSanguineo = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=154");
            ViewBag.FactorSanguineo = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId?grupoId=155");
            ViewBag.EstadoAuditoria = API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId", arg2);
            ViewBag.Indicators = API.Get<Indicators>("PatientsAssistance/IndicatorByPacient", arg);
            ViewBag.Antecedent = API.Get<List<PersonMedicalHistoryList>>("PatientsAssistance/GetAntecedentConsolidateForService", arg);
            ViewBag.Reviews = API.Get<List<ReviewEMO>>("PatientsAssistance/ReviewsEMOs", arg3);

            return View();
        }
        [GeneralSecurity(Rol = "MedicalConsultation-BoardRecipes")]
        public ActionResult GetRecipesByServiceId(string serviceId)
        {
            Api API = new Api();
            ViewBag.RecipesForService = API.Get<List<Recipes>>("Eso/GetRecipesByServiceId?serviceId=" + serviceId);
            return PartialView("_BoardRecipesPartial");
        }

        [GeneralSecurity(Rol = "PatientsAssistance-MedicalConsultation")]
        public JsonResult Indicators(string patientId)
        {

            Api API = new Api();

            string url = "PatientsAssistance/IndicatorByPacient?patientId=" + patientId;
            var result = API.Get<Indicators>(url);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        [GeneralSecurity(Rol = "PatientsAssistance-MedicalConsultation")]
        public JsonResult TopDiagnostic(string organizationId)
        {
            var user = ViewBag.USER.SystemUserId;

            Api API = new Api();
            string url = "PatientsAssistance/TopDiagnostic?systemUserId=" + user + "&organizationId=" + organizationId;
            var result = API.Get<List<TopDiagnostic>>(url);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [GeneralSecurity(Rol = "PatientsAssistance-BoardPatientsAssistance")]
        public JsonResult TopDiagnosticOcupational(string organizationId)
        {
            Api API = new Api();
            string url = "PatientsAssistance/TopDiagnosticOcupational?systemUserId=" + ViewBag.USER.SystemUserId.ToString() + "&organizationId=" + organizationId;
            var result = API.Get<List<TopDiagnostic>>(url);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        public JsonResult RevisedStatusEMO(string serviceId, bool status)
        {
            Api API = new Api();
            string url = "PatientsAssistance/RevisedStatusEMO";
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "serviceId", serviceId },
                { "status", status.ToString() },
            };
            return new JsonResult { Data = API.Get<bool>(url, arg), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult MonthlyControls()
        {
            Api API = new Api();
            string url = "PatientsAssistance/MonthlyControls";
            var result = API.Get<MonthlyControls>(url);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult GetAntecedent(string patientId, string organizationId)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "patientId", patientId},
            };
            Dictionary<string, string> arg2 = new Dictionary<string, string>()
            {
                { "patientId", patientId},
                { "organizationId", organizationId},
            };
            ViewBag.Antecedent = API.Get<List<PersonMedicalHistoryList>>("PatientsAssistance/GetAntecedentConsolidateForService", arg);
            ViewBag.Reviews = API.Get<List<ReviewEMO>>("PatientsAssistance/ReviewsEMOs", arg2);
            ViewBag.Indicators = API.Get<Indicators>("PatientsAssistance/IndicatorByPacient", arg);
            return PartialView("_ReviewEMOPartial");

        }

        public JsonResult GetNamePatient(string value)
        {
            Api API = new Api();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "value", value}
            };
            List<string> result = API.Get<List<string>>("PatientsAssistance/GetNamePatient", args);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult DownloadFile(string patientId)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "patientId", patientId },
            };

            byte[] ms = API.PostDownloadStream("PatientsAssistance/DownloadFile", arg);

            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=FileName.pdf");
            Response.BinaryWrite(ms);
            Response.End();

            return Json(Response);
        }

        [GeneralSecurity(Rol = "PatientsAssistance-BoardPatientsAssistance")]
        public JsonResult ScheduleMedicalConsultation(string personId)
        {
            Api API = new Api();
            string url = "Schedule/ScheduleMedicalConsultation";
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "personId", personId},
                { "nodeId", ViewBag.USER.NodeId.ToString()},
                { "systemUserId", ViewBag.USER.SystemUserId.ToString()},
            };

            return new JsonResult { Data = API.Get<string>(url, arg), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [GeneralSecurity(Rol = "PatientsAssistance-BoardPatientsAssistance")]
        public JsonResult GetServiceComponentsForBuildMenu(string serviceId)
        {
            Api API = new Api();
            string url = "Eso/GetServiceComponentsForBuildMenu";
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                {
                    "serviceId", serviceId
                }
            };
            List<ComponentList> result = API.Get<List<ComponentList>>(url, arg);
            for (int i = 0; i < result.Count; i++)
            {

                result[i].Fields.Sort((x, y) =>
                {
                    int resultado = x.v_Group.CompareTo(y.v_Group);
                    return resultado != 0 ? resultado : x.i_Order.CompareTo(y.i_Order);
                });

            }
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "PatientsAssistance-BoardPatientsAssistance")]
        public JsonResult NewMedicalConsultation(string personId)
        {
            Api API = new Api();

            string url = "Eso/NewMedicalConsultation";
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "personId", personId},
                { "nodeId", ViewBag.USER.NodeId.ToString()},
                { "systemUserId", ViewBag.USER.SystemUserId.ToString()}
            };
            List<ComponentList> result = API.Get<List<ComponentList>>(url, arg);
            for (int i = 0; i < result.Count; i++)
            {
                result[i].Fields.Sort((x, y) => string.Compare(x.v_Group, y.v_Group));

            }
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [GeneralSecurity(Rol = "PatientsAssistance-BoardPatientsAssistance")]
        public ActionResult TestSave(string categoryName)
        {
            Api API = new Api();
            string url = "Eso/TestSave";
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "categoryName",categoryName}
            };

            return new JsonResult { Data = API.Get<string>(url, arg), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetInfo(string serviceComponentId)
        {
            var API = new Api();
            var url = "Eso/GetInfo";

            var arg = new Dictionary<string, string>()
            {
                { "serviceComponentId",serviceComponentId}
            };

            return new JsonResult { Data = API.Get<List<ValorCampo>>(url, arg), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        [GeneralSecurity(Rol = "PatientsAssistance-BoardPatientsAssistance")]
        public JsonResult SaveMedicalConsultation(List<ServiceComponentFieldsList> oServicecomponentfields, string oServicecomponent,
            string serviceComponentId)
        {
            //int permiss; 
            //if (permission == "true")
            //{
            //    permiss = (int)Enumeratores.SaveEso.Allowed;
            //}
            //else
            //{
            //    permiss = (int)Enumeratores.SaveEso.Denied;
            //}
            ServiceComponent oservicecomponent = JsonConvert.DeserializeObject<ServiceComponent>(oServicecomponent);
            var NodeId = ViewBag.USER.NodeId;
            var SystemUserId = ViewBag.USER.SystemUserId;
            oservicecomponent.i_ApprovedUpdateUserId = SystemUserId;
            //var NodeId = 1;
            //var SystemUserId = 49;
            var API = new Api();
            var url = "Eso/SaveMedicalConsultation";

            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(oServicecomponentfields)},
                { "String2",JsonConvert.SerializeObject(oservicecomponent)},//AMC
                { "Int1",NodeId.ToString()},
                { "Int2",SystemUserId.ToString()},
                //{ "Int3",permiss.ToString()}
            };

            var response = API.Post<string>(url, arg);
            return Json(response);
        }

        [GeneralSecurity(Rol = "SendVigilancia-BoardPatientsAssistance")]
        public JsonResult SendVigilancia(Vigilancia oVigilancia)
        {
            var NodeId = ViewBag.USER.NodeId;
            var SystemUserId = ViewBag.USER.SystemUserId;

            //var NodeId = 1;
            //var SystemUserId = 49;
            var API = new Api();
            var url = "Vigilancia/SendVigilancia";

            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(oVigilancia)},
                { "Int1",NodeId.ToString()},
                { "Int2",SystemUserId.ToString()}
            };

            var response = API.Post<string>(url, arg);
            return Json(response);
        }

        [GeneralSecurity(Rol = "SendVigilancia-BoardPatientsAssistance")]
        public JsonResult MedicalConsultationSchedule(ScheduleCustom oSchedule)
        {
            var NodeId = ViewBag.USER.NodeId;
            var SystemUserId = ViewBag.USER.SystemUserId;

            oSchedule.TypeId = (int)Enumeratores.TypeSchedule.Agendado;
            var API = new Api();
            var url = "Schedule/MedicalConsultation";

            Dictionary<string, string> arg = new Dictionary<string, string>()
            {

                { "String1", JsonConvert.SerializeObject(oSchedule)},
                { "Int1",NodeId.ToString()},
                { "Int2",SystemUserId.ToString()}
            };

            var response = API.Post<string>(url, arg);
            return Json(response);
        }

        [GeneralSecurity(Rol = "PatientsAssistance-BoardPatientsAssistance")]
        public JsonResult ListOfAdditionalExams()
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = 500000000;
            Api API = new Api();
            string url = "Component/ListOfAdditionalExams";
            List<AdditionalExams> result = API.Get<List<AdditionalExams>>(url);
            for (int i = 0; i < result.Count; i++)
            {
                for (int z = 0; z < result[i].Components.Count; z++)
                {

                    result[i].Components[z].fields.Sort((x, y) =>
                    {
                        int resultado = x.Group.CompareTo(y.Group);
                        return resultado != 0 ? resultado : x.Order.CompareTo(y.Order);
                    });
                }


            }
            var json = new JsonResult
            {
                Data = result,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            json.MaxJsonLength = 500000000;

            return json;

        }

        [GeneralSecurity(Rol = "PatientsAssistance-BoardPatientsAssistance")]
        public async Task<JsonResult> ViewSchceduleVigilancia(string organizationId)
        {
            var doctorResponsibleId = ViewBag.USER.SystemUserId;
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "doctorResponsibleId", doctorResponsibleId.ToString() },
                { "organizationId", organizationId },
            };

            return await Task.Run(() =>
            {
                string url = "PatientsAssistance/ViewSchceduleVigilancia";

                return new JsonResult { Data = API.Get<List<VigilanciaServiceCustom>>(url, arg), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            });
        }

        [GeneralSecurity(Rol = "PatientsAssistance-BoardPatientsAssistance")]
        public async Task<JsonResult> DarDeBaja(string personId)
        {

            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "personId", personId },

            };

            return await Task.Run(() =>
            {
                string url = "Service/DarDeBaja";

                return new JsonResult { Data = API.Get<string>(url, arg), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            });
        }

        [GeneralSecurity(Rol = "PatientsAssistance-MedicalConsultation")]
        public ActionResult MedicalResult(string serviceId)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "serviceId",serviceId}
            };

            ViewBag.Results = API.Get<List<DiagnosticCustom>>("Diagnostic/GetDiagnosticByServiceId", arg);


            return PartialView("_MedicalResult");
        }

        [GeneralSecurity(Rol = "PatientsAssistance-BoardPatientsAssistance")]
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

        [GeneralSecurity(Rol = "PatientsAssistance-BoardPatientsAssistance")]
        public JsonResult SaveDiagnostic(List<DiagnosticCustom> diagnostics)
        {
            Api API = new Api();
            string url = "Diagnostic/SaveDiagnostic";

            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                {  "String1" , JsonConvert.SerializeObject(diagnostics) },
                {  "Int1" , ViewBag.USER.NodeId.ToString()},
                {  "Int2" , ViewBag.USER.SystemUserId.ToString() },
            };

            var response = API.Post<string>(url, arg);
            return new JsonResult { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [GeneralSecurity(Rol = "PatientsAssistance-MedicalConsultation")]
        public JsonResult ReportManagerByServiceId(string serviceId)
        {
            Api API = new Api();
            string url = "ReportManager/ReportManagerByServiceId";

            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                {  "serviceId" , serviceId },
            };

            var response = API.Get<List<ReportManagerBe>>(url, arg);
            return new JsonResult { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "PatientsAssistance-MedicalConsultation")]
        public JsonResult GetInfoServiceComponent(string serviceComponentId)
        {
            Api API = new Api();
            string url = "Eso/GetInfoServiceComponent";

            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                {  "serviceComponentId" , serviceComponentId },
            };

            var response = API.Get<ServiceComponentBe>(url, arg);

            return new JsonResult { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "PatientsAssistance-MedicalConsultation")]
        public JsonResult BuildReport(List<ComponentsServiceId> oComponentsServiceId)
        {
            Api API = new Api();
            string url = "ReportManager/BuildReport";

            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                {  "String1" , JsonConvert.SerializeObject(oComponentsServiceId) },
            };

            var response = API.Post<string>(url, arg);
            //DirectoryInfo source = new DirectoryInfo(http://localhost:1932/ESO/");
            //FileInfo[] fileToCopy = source.GetFiles(response);
            //var destinationFolder = "http://localhost:1045/ESO/";
            //foreach (FileInfo file in fileToCopy)
            //{

            //    file.CopyTo(destinationFolder + file.Name);

            //}

            return new JsonResult { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "PatientsAssistance-MedicalConsultation")]
        public JsonResult Searchmasterrecommendationrestricction(string name, int typeId)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "name" , name  },
                { "typeId" , typeId.ToString()  },
            };

            var result = API.Get<List<string>>("Diagnostic/Searchmasterrecommendationrestricction", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [GeneralSecurity(Rol = "PatientsAssistance-MedicalConsultation")]
        public JsonResult GetServiceIdNewMedicalConsultation(string personId)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "personId" , personId  },
                { "nodeId" , ViewBag.USER.NodeId.ToString()  },
                { "systemUserId" , ViewBag.USER.SystemUserId.ToString()  },
            };

            var result = API.Get<string>("Eso/GetServiceIdNewMedicalConsultation", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [GeneralSecurity(Rol = "PatientsAssistance-MedicalConsultation")]
        public JsonResult TimeLineByServiceId(string serviceId)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "serviceId" , serviceId  },
            };

            var result = API.Get<List<TimeLine>>("Eso/TimeLineByServiceId", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [GeneralSecurity(Rol = "PatientsAssistance-MedicalConsultation")]
        public JsonResult SaveServiceStatus(ServiceStatusBE oServiceStatusBE)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1" , JsonConvert.SerializeObject(oServiceStatusBE)  },
            };

            var result = API.Post<bool>("Eso/SaveServiceStatus", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [GeneralSecurity(Rol = "PatientsAssistance-MedicalConsultation")]
        public JsonResult GetWarehouses(string organizationId)
        {
            var organizations = ViewBag.User.Organizations;
            List<OrganizationWareHouse> result = new List<OrganizationWareHouse>();
            foreach (var item in organizations)
            {
                if (item.OrganizationId == organizationId)
                {
                    result = item.WareHouses;
                    break;
                }
            }

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "PatientsAssistance-Board")]
        public JsonResult GetOptionsContextMenu()
        {
            var result = ViewBag.User.Options;

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "MedicalConsultation-PrintRecipe")]
        public JsonResult PrintRecipes(BoardPrintRecipes data)
        {
            data.MovementTypeId = (int)Enumeratores.MovementType.Egreso;
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1" , JsonConvert.SerializeObject(data) },
                { "Int1" , ViewBag.USER.NodeId.ToString() },
                { "Int2" , ViewBag.USER.SystemUserId.ToString() },
            };

            var result = API.Post<string>("Eso/GeneratePrintRecipes", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "MedicalConsultation-AddEmbarazo")]
        public JsonResult AddEmbarazo(EmbarazoCustom data)
        {

            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1" , JsonConvert.SerializeObject(data) },
                { "Int1" , ViewBag.USER.NodeId.ToString() },
                { "Int2" , ViewBag.USER.SystemUserId.ToString() },
            };

            var result = API.Post<MessageCustom>("Embarazo/AddEmbarazo", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "MedicalConsultation-AddPlanIntegral")]
        public JsonResult AddPlanIntegral(PlanIntegralList data)
        {

            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1" , JsonConvert.SerializeObject(data) },
                { "Int1" , ViewBag.USER.NodeId.ToString() },
                { "Int2" , ViewBag.USER.SystemUserId.ToString() },
            };

            var result = API.Post<MessageCustom>("PlanIntegral/AddPlanIntegral", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "MedicalConsultation-AddProblema")]
        public JsonResult AddProblema(ProblemaList data)
        {

            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1" , JsonConvert.SerializeObject(data) },
                { "Int1" , ViewBag.USER.NodeId.ToString() },
                { "Int2" , ViewBag.USER.SystemUserId.ToString() },
            };

            var result = API.Post<MessageCustom>("PlanIntegral/AddProblema", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "MedicalConsultation-AddProblema")]
        public JsonResult GetPlanIntegral(string PersonId)
        {
            Api API = new Api();

            var result = API.Get<List<PlanIntegralList>>("PlanIntegral/GetPlanIntegralAndFiltered?personId=" + PersonId);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [GeneralSecurity(Rol = "MedicalConsultation-AddProblema")]
        public JsonResult GetProblemas(string PersonId)
        {
            Api API = new Api();

            var result = API.Get<List<ProblemaList>>("PlanIntegral/GetProblemaPagedAndFiltered?personId=" + PersonId);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "MedicalConsultation-AddProblema")]
        public JsonResult EditPlanIntegral(string PersonId)
        {
            Api API = new Api();

            var result = API.Get<List<PlanIntegralList>>("PlanIntegral/GetPlanIntegralAndFiltered?personId=" + PersonId);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [GeneralSecurity(Rol = "MedicalConsultation-EditProblemas")]
        public JsonResult EditProblemas(string PersonId)
        {
            Api API = new Api();

            var result = API.Get<List<ProblemaList>>("PlanIntegral/GetProblemaPagedAndFiltered?personId=" + PersonId);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "MedicalConsultation-SaveDataNinio")]
        public JsonResult SaveDataNinio(BoardGenerales data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data) },
                { "Int1", ViewBag.USER.SystemUserId.ToString() },
                { "Int2", ViewBag.USER.NodeId.ToString() },
            };
            var result = API.Post<MessageCustom>("Antecedentes/SaveDataNinio", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "MedicalConsultation-UpdateDataNinio")]
        public JsonResult UpdateDataNinio(BoardGenerales data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data) },
                { "Int1", ViewBag.USER.SystemUserId.ToString() },
                { "Int2", ViewBag.USER.NodeId.ToString() },
            };
            var result = API.Post<MessageCustom>("Antecedentes/UpdateDataNinio", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "MedicalConsultation-SaveDataAdolescente")]
        public JsonResult SaveDataAdolescente(BoardGenerales data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data) },
                { "Int1", ViewBag.USER.SystemUserId.ToString() },
                { "Int2", ViewBag.USER.NodeId.ToString() },
            };
            var result = API.Post<MessageCustom>("Antecedentes/SaveDataAdolescente", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "MedicalConsultation-UpdateDataAdolescente")]
        public JsonResult UpdateDataAdolescente(BoardGenerales data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data) },
                { "Int1", ViewBag.USER.SystemUserId.ToString() },
                { "Int2", ViewBag.USER.NodeId.ToString() },
            };
            var result = API.Post<MessageCustom>("Antecedentes/UpdateDataAdolescente", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "MedicalConsultation-SaveDataAdulto")]
        public JsonResult SaveDataAdulto(BoardGenerales data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data) },
                { "Int1", ViewBag.USER.SystemUserId.ToString() },
                { "Int2", ViewBag.USER.NodeId.ToString() },
            };
            var result = API.Post<MessageCustom>("Antecedentes/SaveDataAdulto", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "MedicalConsultation-UpdateDataAdulto")]
        public JsonResult UpdateDataAdulto(BoardGenerales data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data) },
                { "Int1", ViewBag.USER.SystemUserId.ToString() },
                { "Int2", ViewBag.USER.NodeId.ToString() },
            };
            var result = API.Post<MessageCustom>("Antecedentes/UpdateDataAdulto", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "MedicalConsultation-SaveDataAdultoMayor")]
        public JsonResult SaveDataAdultoMayor(BoardGenerales data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data) },
                { "Int1", ViewBag.USER.SystemUserId.ToString() },
                { "Int2", ViewBag.USER.NodeId.ToString() },
            };
            var result = API.Post<MessageCustom>("Antecedentes/SaveDataAdultoMayor", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "MedicalConsultation-UpdateDataAdultoMayor")]
        public JsonResult UpdateDataAdultoMayor(BoardGenerales data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data) },
                { "Int1", ViewBag.USER.SystemUserId.ToString() },
                { "Int2", ViewBag.USER.NodeId.ToString() },
            };
            var result = API.Post<MessageCustom>("Antecedentes/UpdateDataAdultoMayor", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "MedicalConsultation-UpdateDataAdultoMayor")]
        public JsonResult SaveCuidadosPreventivos(EsoCuidadosPreventivosFechas data, string personId)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(data) },
                { "String2", personId },
                { "Int1", ViewBag.USER.SystemUserId.ToString() },
                { "Int2", ViewBag.USER.NodeId.ToString() },
            };
            var result = API.Post<MessageCustom>("Antecedentes/SaveCuidadosPreventivos", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [GeneralSecurity(Rol = "MedicalConsultation-VerificarKey")]
        public JsonResult SendSMS()
        {
            MessageCustom msg = new MessageCustom();
            Api API = new Api();
            if (ViewBag.USER.TelephoneNumber != null)
            {
                if (ViewBag.USER.TelephoneNumber.Length == 9)
                {
                    var result = API.Get<string>("SendSMS/SendSMS?number=" + ViewBag.USER.TelephoneNumber);
                    ViewBag.USER.KeySMS = result;
                    msg.Error = false;
                    msg.Status = (int)HttpStatusCode.OK;

                }
                else
                {
                    msg.Error = true;
                    msg.Status = (int)HttpStatusCode.NoContent;
                    msg.Message = "Su número registrado debe ser un celular, por favor comuniquese con el administrador de sistemas";
                }
            }
            else
            {
                msg.Error = true;
                msg.Status = (int)HttpStatusCode.NoContent;
                msg.Message = "Debe tener registrado un número de celular, por favor comuniquese con el administrador de sistemas";

            }

            return new JsonResult { Data = msg, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [GeneralSecurity(Rol = "MedicalConsultation-VerificarKey")]
        public JsonResult VerificarKey(string keySMS)
        {

            bool result = false;
            if (ViewBag.USER.KeySMS == keySMS)
            {
                result = true;
            }
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "MedicalConsultation-SaveAndPrintRecipes")]
        public JsonResult SaveAndPrintRecipes(BoardPrintRecipes data)
        {
            data.MovementTypeId = (int)Enumeratores.MovementType.Egreso;
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1" , JsonConvert.SerializeObject(data) },
                { "Int1" , ViewBag.USER.NodeId.ToString() },
                { "Int2" , ViewBag.USER.SystemUserId.ToString() },
            };

            var result = API.Post<MessageCustom>("Eso/SaveAndPrintRecipes", arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "MedicalConsultation-CopyPdf")]
        public JsonResult CopyPdf()
        {
            Api API = new Api();
            var result = API.Get<MessageCustom>("PatientsAssistance/CopyPdf");
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}