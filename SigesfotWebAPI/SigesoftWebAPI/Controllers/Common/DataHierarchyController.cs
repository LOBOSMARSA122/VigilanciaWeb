using BE.Common;
using BL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.Common
{
    public class DataHierarchyController : ApiController
    {
        private  DataHierarchyBL oDataHierarchyBL = new DataHierarchyBL();

        [HttpGet]
        public IHttpActionResult GetDataHierarchyByGrupoId(int grupoId)
        {
            List<Dropdownlist> result = oDataHierarchyBL.GetDatahierarchyByGrupoId(grupoId);
            return Ok(result);
        }
    }
}
