using BE.Common;
using BL.Plan;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.Plan
{
    public class PlanController : ApiController
    {
        [HttpPost]
        public IHttpActionResult SavePlan(MultiDataModel model)
        {
            PlanBE data = JsonConvert.DeserializeObject<PlanBE>(model.String1);
            var result = PlanBL.SavePlan(data);

            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetPlanByProtocolId(string protocolId)
        {
            var result = PlanBL.GetPlanByProtocolId(protocolId);

            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult DeletedPlan(int planId)
        {
            var result = PlanBL.DeletedPlan(planId);

            return Ok(result);
        }
    }
}
