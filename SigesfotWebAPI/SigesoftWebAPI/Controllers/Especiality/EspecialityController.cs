using BE;
using BE.Common;
using BE.Especiality;
using BL.Especiality;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.Especiality
{
    public class EspecialityController : ApiController
    {
        [HttpPost]
        public IHttpActionResult GetAllEspeciality(MultiDataModel multi)
        {
            BoardEspeciality data = JsonConvert.DeserializeObject<BoardEspeciality>(multi.String1);

            var result = EspecialityBL.GetAllEspeciality(data);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetEspecialityById(string especialityId)
        {
            var result = EspecialityBL.GetEspecialityById(especialityId);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult AddEspeciality(MultiDataModel multi)
        {
            EspecialityBE dataEsp = JsonConvert.DeserializeObject<EspecialityBE>(multi.String1);
            var result = EspecialityBL.AddEspeciality(dataEsp);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult DeletedEspeciality(string especialityId)
        {
            var result = EspecialityBL.DeletedEspeciality(especialityId);
            return Ok(result);
        }
    }
}
