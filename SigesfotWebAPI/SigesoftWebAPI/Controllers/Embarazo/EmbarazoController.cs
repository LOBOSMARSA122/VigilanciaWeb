using BE.Common;
using BE.Embarazo;
using BE.Message;
using BL.EmbarazoBL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.Embarazo
{
    public class EmbarazoController : ApiController
    {
        [HttpPost]
        public IHttpActionResult AddEmbarazo(MultiDataModel data)
        {
            EmbarazoCustom obj = JsonConvert.DeserializeObject<EmbarazoCustom>(data.String1);
            MessageCustom result = new EmbarazoBL().AddEmbarazo(obj, data.Int1, data.Int2);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetEmbarazo(string personId)
        {
            var result = new EmbarazoBL().GetEmbarazo(personId);
            return Ok(result);
        }
    }
}
