using BE.MedicalAssistance;
using BL.MedicalAssistance;
using System.Collections.Generic;
using System.Web.Http;
using BE.Common;
using BE.Vigilancia;
using BL.Vigilancia;

namespace SigesoftWebAPI.Controllers.MedicalAssistance
{
    public class PatientsAssistanceController : ApiController
    {
        private readonly PatientsAssistanceBL _oPatientsAssistanceBl = new PatientsAssistanceBL();
        private readonly FilterWorkersBl _oFilterWorkersBl = new FilterWorkersBl();

        [HttpPost]
        public IHttpActionResult FilterWorkers(BoardPatient data)
        {
            var result = _oFilterWorkersBl.FilterWorkers(data);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetPendingReview(string systemUserId, string organizationId)
        {
            int? result = _oPatientsAssistanceBl.GetPendingReview(int.Parse(systemUserId), organizationId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetSchedule()
        {
            var result = _oPatientsAssistanceBl.GetSchedule();
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult TopDiagnostic(int systemUserId, string organizationId )
        {
            var result = _oPatientsAssistanceBl.TopDiagnostic(systemUserId, organizationId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult TopDiagnosticOcupational(string systemUserId, string organizationId)
        {
            var result = _oPatientsAssistanceBl.TopDiagnosticOcupational(int.Parse(systemUserId),organizationId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult IndicatorByPacient(string patientId)
        {
            var result = _oPatientsAssistanceBl.IndicatorByPacient(patientId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult MonthlyControls()
        {
            var result = _oPatientsAssistanceBl.MonthlyControls();
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult ReviewsEmOs(string patientId, string organizationId)
        {
            var result = _oPatientsAssistanceBl.ReviewsEMOs(patientId, organizationId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetAntecedentConsolidateForService(string patientId)
        {
            var result = _oPatientsAssistanceBl.GetAntecedentConsolidateForService(patientId);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult DownloadFile(Patients patientId)
        {
            var directorioEso = string.Format("{0}{1}\\", System.Web.Hosting.HostingEnvironment.MapPath("~/"), System.Configuration.ConfigurationManager.AppSettings["directorioESO"]);

            var response = _oPatientsAssistanceBl.DownloadFile(patientId.PatientId, directorioEso);
            return Ok(response);
        }

        [HttpGet]
        public IHttpActionResult RevisedStatusEmo(string serviceId, string status)
        {
            var statusBool = bool.Parse(status);

            var result = _oPatientsAssistanceBl.RevisedStatusEMO(serviceId, statusBool);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetNamePatient(string value)
        {
            var result = _oPatientsAssistanceBl.GetNamePatients(value);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetAllOrganizationEmployers()
        {
            List<KeyValueDTO> result = _oPatientsAssistanceBl.GetAllOrganizationEmployers();
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult ViewSchceduleVigilancia(int doctorResponsibleId, string organizationId)
        {
            List<VigilanciaServiceCustom> result = _oPatientsAssistanceBl.VigilanciaServiceDtos(doctorResponsibleId, organizationId);
            return Ok(result);
        }

    }
}
