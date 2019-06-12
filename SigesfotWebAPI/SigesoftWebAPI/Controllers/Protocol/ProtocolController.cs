using BL.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.Protocol
{
    public class ProtocolController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetDataProtocol(string protocolId)
        {
            var result = new ProtocolBL().GetDataProtocol(protocolId);
            return Ok(result);
        }
    }
}
