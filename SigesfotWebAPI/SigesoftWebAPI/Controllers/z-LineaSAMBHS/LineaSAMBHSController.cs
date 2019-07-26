using BL.z_LineaSAMBHS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.z_LineaSAMBHS
{
    public class LineaSAMBHSController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetAllLineas()
        {
            var result = LineaSAMBHSBL.GetAllLineas();
            return Ok(result);
        }
    }
}
