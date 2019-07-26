using BE.Common;
using BE.Protocol;
using BL.Protocol;
using Newtonsoft.Json;
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

        [HttpPost]
        public IHttpActionResult GetAllProtocol(MultiDataModel multidata)
        {
            BoardProtocol data = JsonConvert.DeserializeObject<BoardProtocol>(multidata.String1);
            var result = ProtocolBL.GetAllProtocol(data);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetProtocolComponentByProtocolId(string protocolId)
        {
            var result = ProtocolBL.GetProtocolComponentByProtocolId(protocolId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult VerifyExistsProtocol(string protocolName)
        {
            var result = ProtocolBL.VerifyExistsProtocol(protocolName);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult SaveProtocols(MultiDataModel multi)
        {
            ProtocolList data = JsonConvert.DeserializeObject<ProtocolList>(multi.String1);
            var result = ProtocolBL.SaveProtocols(data, multi.Int1, multi.Int2);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult DeletedProtocolComponent(string protocolComponentId, int userId)
        {
            var result = ProtocolBL.DeletedProtocolComponent(protocolComponentId, userId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetPriceProtocolComponentByProtocolId(string protocolId)
        {
            var result = ProtocolBL.GetProtocolComponentByProtocolId(protocolId);
            float resp = 0;
            if (result.ListProtocolComponents != null)
            {
                if (result.ListProtocolComponents.Count > 0)
                {
                    resp = result.ListProtocolComponents.Sum(x => x.Price);
                }
            }    
            return Ok(resp);
        }

        [HttpGet]
        public IHttpActionResult GetSecuentialForServiceOrder(int nodeId, int tableId)
        {
            var result = ProtocolBL.GetSecuentialForOrderService(nodeId, tableId);
            return Ok(result);
        }
    }
}
