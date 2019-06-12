using BE.Common;
using BE.Z_SAMBHSCUSTOM.Clientes;
using BL.z_ClientesSAMBHS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.Clientes
{
    public class ClientesController : ApiController
    {
        [HttpPost]
        public IHttpActionResult GetClientes(MultiDataModel data)
        {
            BoardCliente dataClient = JsonConvert.DeserializeObject<BoardCliente>(data.String1);
            var result = new ClientesBL().GetClients(dataClient);
            return Ok(result);
        }
    }
}
