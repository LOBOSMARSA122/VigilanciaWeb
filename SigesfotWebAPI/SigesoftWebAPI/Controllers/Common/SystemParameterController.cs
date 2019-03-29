using BE.Common;
using BL.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.Common
{
    public class SystemParameterController : ApiController
    {
        private SystemParameterBL oSystemParameterBL = new SystemParameterBL();

        [HttpGet]
        public IHttpActionResult GetParametroByGrupoId(int grupoId)
        {
            List<Dropdownlist> result = oSystemParameterBL.GetParametroByGrupoId(grupoId);
            return Ok(result);
        }

    }
}
