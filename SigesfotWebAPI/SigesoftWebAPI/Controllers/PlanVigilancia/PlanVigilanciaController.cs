using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BE.Common;
using BE.Plan;
using BL.PlanVigilancia;
using Newtonsoft.Json;

namespace SigesoftWebAPI.Controllers.PlanVigilancia
{
    public class PlanVigilanciaController : ApiController
    {
        private readonly PlanVigilanciaBl _oPlanVigilanciaBl = new PlanVigilanciaBl();

        [HttpPost]
        public IHttpActionResult Filter(BoardPlanVigilancia data)
        {
            var result = _oPlanVigilanciaBl.Filter(data);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetId(string planVigianciaId)
        {
            var result = _oPlanVigilanciaBl.GetId(planVigianciaId);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult Save(MultiDataModel model)
        {
            if (string.IsNullOrWhiteSpace(model.String1))
                return BadRequest("Información Inválida");

            var data = JsonConvert.DeserializeObject<PlanVigilanciaCustom>(model.String1);

            var result = _oPlanVigilanciaBl.Save(data);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult Remove(string planVigilanciaId, int systemUserId)
        {
            var result = _oPlanVigilanciaBl.Remove(planVigilanciaId,systemUserId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult SearchDisease(string name)
        {
            var result = _oPlanVigilanciaBl.SearchDisease(name);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult ComboPlanesVigilancia(string organizationId)
        {
            var result = _oPlanVigilanciaBl.ComboPlanesVigilancia(organizationId);
            return Ok(result);
        }

    }
}
