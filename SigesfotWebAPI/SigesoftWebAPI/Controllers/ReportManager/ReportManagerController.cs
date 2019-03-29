using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;  
using System.Web.Http;
using BE.Common;
using BE.ReportManager;
using BL.ReportManager;
using Newtonsoft.Json;

namespace SigesoftWebAPI.Controllers.ReportManager
{
    public class ReportManagerController : ApiController
    {
        ReportManagerBl _ReportManagerBl = new ReportManagerBl();

        [HttpGet]
        public IHttpActionResult ReportManagerByServiceId(string serviceId)
        {
            var result = _ReportManagerBl.ReportManagerByServiceId(serviceId);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult BuildReport(MultiDataModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.String1))
                    return BadRequest("Informaciónn Inválida");

                var data = JsonConvert.DeserializeObject<List<ComponentsServiceId>>(model.String1);

                var result = _ReportManagerBl.BuildReport(data);
                return Ok(result);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
