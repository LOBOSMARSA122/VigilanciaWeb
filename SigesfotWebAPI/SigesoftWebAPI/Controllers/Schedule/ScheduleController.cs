using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BE.Common;
using BE.Schedule;
using BL.Calendar;
using Newtonsoft.Json;

namespace SigesoftWebAPI.Controllers.Schedule
{
    public class ScheduleController : ApiController
    {
        private ScheduleBl oScheduleBl = new ScheduleBl();

        [HttpPost]
        public IHttpActionResult MedicalConsultation(MultiDataModel model)
        {
            if (string.IsNullOrWhiteSpace(model.String1))
                return BadRequest("Información Inválida");
            var data = JsonConvert.DeserializeObject<ScheduleCustom>(model.String1);

            var result = oScheduleBl.ScheduleMedicalConsultation(data, model.Int1, model.Int2);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult ScheduleOcupationalConsultation(string personId, int nodeId, int systemUserId)
        {
            var result = oScheduleBl.ScheduleOcupationalConsultation(personId, nodeId, systemUserId);
            return Ok(result);
        }

    }
}
