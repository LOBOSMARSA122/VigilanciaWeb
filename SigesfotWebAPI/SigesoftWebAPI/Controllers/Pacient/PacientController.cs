using BE.Common;
using BE.Message;
using BE.Pacient;
using BL.Pacient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.Pacient
{
    public class PacientController : ApiController
    {
        PacientBL pacientBL = new PacientBL();

        [HttpPost]
        public IHttpActionResult CreateOrUpdatePacient(MultiDataModel data)
        {
            PacientCustom objPerson = JsonConvert.DeserializeObject<PacientCustom>(data.String1);

            MessageCustom result = pacientBL.CreateOrUpdatePacient(objPerson, data.Int1, data.Int2);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult FindPacientByDocNumberOrPersonId(string value)
        {
            var result = pacientBL.FindPacientByDocNumberOrPersonId(value);
            return Ok(result);
        }


    }
}
