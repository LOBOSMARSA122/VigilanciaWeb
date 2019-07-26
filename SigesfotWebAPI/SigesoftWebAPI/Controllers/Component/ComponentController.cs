using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BL.Component;

namespace SigesoftWebAPI.Controllers.Component
{
    public class ComponentController : ApiController
    {
        private readonly ComponentBl _oComponentBl = new ComponentBl();

        [HttpGet]
        public IHttpActionResult ListOfAdditionalExams()
        {
            var result = _oComponentBl.ListOfAdditionalExams();
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetAllComponent(int nodeId, int roleNodeId)
        {
            var result = _oComponentBl.GetAllComponent(nodeId, roleNodeId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetMedicalExam(string componentName)
        {
            var result = _oComponentBl.GetMedicalExam(componentName);
            return Ok(result);
        }
    }
}
