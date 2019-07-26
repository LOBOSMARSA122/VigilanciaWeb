using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BL.SendSMS;

namespace SigesoftWebAPI.Controllers.SendSMS
{
    public class SendSMSController : ApiController
    {
        [HttpGet]
        public IHttpActionResult SendSMS(string number)
        {
            var result = new SendSMSBL().SendSMS(number);
            return Ok(result);
        }
    }
}
