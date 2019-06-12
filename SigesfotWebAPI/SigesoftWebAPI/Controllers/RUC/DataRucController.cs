using BL.RUC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.RUC
{
    public class DataRucController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetDataContribuyente(string numDoc)
        {
            var result = new RucDataBL().GetDataContribuyente(numDoc);
            return Ok(result);
        }
    }
}
