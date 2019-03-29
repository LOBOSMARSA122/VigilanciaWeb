using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BE.Common;
using BE.Vigilancia;
using BL.Vigilancia;
using Newtonsoft.Json;

namespace SigesoftWebAPI.Controllers.Vigilancia
{
    public class VigilanciaController : ApiController
    {
        private readonly VigilanciaBl _oVigilanciaBl = new VigilanciaBl();
        [HttpPost]
        public IHttpActionResult SendVigilancia(MultiDataModel model)
        {
            if (string.IsNullOrWhiteSpace(model.String1))
                return BadRequest("Información Inválida");

            var data = JsonConvert.DeserializeObject<VigilanciaDto>(model.String1);

            var result = _oVigilanciaBl.SendVigilancia(data, model.Int1, model.Int2);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult FinishVigilancia(MultiDataModel model)
        {
            if (string.IsNullOrWhiteSpace(model.String1))
                return BadRequest("Información Inválida");

            var data = JsonConvert.DeserializeObject<VigilanciaDto>(model.String1);

            var result = _oVigilanciaBl.FinishVigilancia(data.v_VigilanciaId, model.Int1);
            return Ok(result);
        }


    }
}
