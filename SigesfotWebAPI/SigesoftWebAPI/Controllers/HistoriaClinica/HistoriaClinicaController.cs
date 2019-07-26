using DAL.HistoriaClinica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.HistoriaClinica
{
    public class HistoriaClinicaController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GenerateHistoriaClinica(string personId, string serviceId)
        {
            var result = new HistoriaClinicaBL().GenerateHistoriaClinica(personId, serviceId);
            return Ok(result);
        }
    }
}
