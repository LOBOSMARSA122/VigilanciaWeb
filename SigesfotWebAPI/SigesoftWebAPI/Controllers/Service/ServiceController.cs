using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BE.Common;
using BE.Service;
using BL.Service;
using Newtonsoft.Json;

namespace SigesoftWebAPI.Controllers.Service
{
    public class ServiceController : ApiController
    {
        private readonly ServiceBl _oServiceBl = new ServiceBl();

        [HttpGet]
        public IHttpActionResult DarDeBaja(string personId)
        {
            var result = _oServiceBl.DarDeBaja(personId);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult GetServices(MultiDataModel data)
        {
            BoardServiceCustomList dataService = JsonConvert.DeserializeObject<BoardServiceCustomList>(data.String1);
            var result = _oServiceBl.GetServicesPagedAndFiltered(dataService);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetAllComponentsByService(string serviceId)
        {
            var result = ServiceBl.GetAllComponentsByService(serviceId);
            return Ok(result);
        }
    }
}
