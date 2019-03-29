using System.Collections.Generic;
using System.Web.Http;
using BE.Common;
using BE.Diagnostic;
using BL.Diagnostic;
using Newtonsoft.Json;

namespace SigesoftWebAPI.Controllers.Diagnostic
{
    public class DiagnosticController : ApiController
    {
        private readonly DiagnosticBl _oDiagnosticBl = new DiagnosticBl();

        [HttpGet]
        public IHttpActionResult GetDiagnosticByServiceId(string serviceId)
        {
            var result = _oDiagnosticBl.GetDiagnosticsByServiceId(serviceId);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult SaveDiagnostic(MultiDataModel model)
        {
            if (string.IsNullOrWhiteSpace(model.String1))
                return BadRequest("Información Inválida");

            var data = JsonConvert.DeserializeObject<List<DiagnosticCustom>>(model.String1);

            var result = _oDiagnosticBl.SaveDiagnostics(data, model.Int1, model.Int2);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult Searchmasterrecommendationrestricction(string name, int typeId)
         {
            var result = _oDiagnosticBl.Searchmasterrecommendationrestricction(name, typeId);
            return Ok(result);
        }

    }
}