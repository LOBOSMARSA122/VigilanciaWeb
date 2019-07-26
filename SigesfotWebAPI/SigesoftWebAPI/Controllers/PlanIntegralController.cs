using BE.Common;
using BE.PlanIntegral;
using BL.PlanIntegral;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers
{
    public class PlanIntegralController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetPlanIntegralAndFiltered(string personId)
        {
            var result = new PlanIntegralBL().GetPlanIntegralAndFiltered(personId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetProblemaPagedAndFiltered(string personId)
        {
            var result = new PlanIntegralBL().GetProblemaPagedAndFiltered(personId);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult AddPlanIntegral(MultiDataModel data)
        {
            PlanIntegralList dataPlan = JsonConvert.DeserializeObject<PlanIntegralList>(data.String1);

            var result = new PlanIntegralBL().AddPlanIntegral(dataPlan, data.Int1, data.Int2);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult AddProblema(MultiDataModel data)
        {
            ProblemaList dataProb = JsonConvert.DeserializeObject<ProblemaList>(data.String1);

            var result = new PlanIntegralBL().AddProblema(dataProb, data.Int1, data.Int2);
            return Ok(result);
        }

    }
}
