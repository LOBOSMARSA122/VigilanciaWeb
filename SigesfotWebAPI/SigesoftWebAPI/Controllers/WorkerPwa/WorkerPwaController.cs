using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BE.Common;
using BE.Eso;
using BE.Worker;
using BL.MedicalAssistance;
using BL.WorkerPwa;
using Newtonsoft.Json;

namespace SigesoftWebAPI.Controllers.WorkerPwa
{
    public class WorkerPwaController : ApiController
    {
        private readonly WorkerPwaBl _WorkerPwaBl = new WorkerPwaBl();
        [HttpGet]
        public IHttpActionResult WorkerInformationPwa(string personId)
        {
            var result = _WorkerPwaBl.WorkerInformationPwa(personId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult SendNotification(string personId)
        {
            var result = "OK";
            return Ok(result);
        }
        
        [HttpPost]
        public IHttpActionResult UpdateWorker(MultiDataModel model)
        {
            if (string.IsNullOrWhiteSpace(model.String1))
                return BadRequest("Información Inválida");
            var data = JsonConvert.DeserializeObject<BE.Worker.WorkerPwa>(model.String1);

            var result = _WorkerPwaBl.UpdateWorker(data);
            return Ok(result);
        }
    }
}
