using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BL.Component;

namespace SigesoftWebAPI.Controllers.Component
{
    public class ComponentController : ApiController
    {
        private readonly ComponentBl _oComponentBl = new ComponentBl();

        [HttpGet]
        public IHttpActionResult ListOfAdditionalExams()
        {
            var result = _oComponentBl.ListOfAdditionalExams();
            return Ok(result);
        }

    }
}
