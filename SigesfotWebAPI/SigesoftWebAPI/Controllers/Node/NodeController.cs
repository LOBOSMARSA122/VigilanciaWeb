using BE.Common;
using BL.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.Node
{
    public class NodeController : ApiController
    {
        NodeBl _NodeBl = new NodeBl();
        [HttpGet]
        public IHttpActionResult GetAllNodeForCombo(bool remoto, int nodeId)
        {
            var list = _NodeBl.GetAllNodeForCombo(remoto, nodeId);
            return Ok(list);
        }

    }
}
