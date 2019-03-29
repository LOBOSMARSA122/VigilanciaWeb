using BL.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.Security
{
    public class AuthorizationController : ApiController
    {
        private AuthorizationBL oAuthorizationBL = new AuthorizationBL();

        [HttpGet]
        public IHttpActionResult ValidateSystemUser(int nodeId, string userName, string password)
        {
            var result = oAuthorizationBL.ValidateSystemUser(nodeId, userName, password);
            return Ok(result);
        }
    }
}
