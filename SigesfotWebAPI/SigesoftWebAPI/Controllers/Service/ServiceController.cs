using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BL.Service;

namespace SigesoftWebAPI.Controllers.Service
{
    public class ServiceController : ApiController
    {
        private readonly ServiceBl _oServiceBl = new ServiceBl();

        [HttpGet]
        public IHttpActionResult DarDeBaja(string personId)
        {
            var result = _oServiceBl.DarDeBaja(personId);
            return Ok(result);
        }

    }
}
