using BE.Antecedentes;
using BE.Common;
using BE.Ninio;
using BL.Antecedentes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers
{
    public class AntecedentesController : ApiController
    {
        [HttpGet]
        public IHttpActionResult ObtenerEsoAntecedentesPorGrupoId(string PersonId)
        {

            var result = new EsoAntecedentesBL().ObtenerEsoAntecedentesPorGrupoId(PersonId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult ObtenerFechasCuidadosPreventivos(string PersonId)
        {

            var result = new EsoAntecedentesBL().ObtenerFechasCuidadosPreventivos(PersonId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetServicePersonData(string serviceId)
        {

            var result = new EsoAntecedentesBL().GetServicePersonData(serviceId);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult SaveDataNinio(MultiDataModel data)
        {
            
            BoardGenerales dataBoard = JsonConvert.DeserializeObject<BoardGenerales>(data.String1);
            var result = new EsoAntecedentesBL().SaveDataNinio(dataBoard, data.Int1, data.Int2);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult UpdateDataNinio(MultiDataModel data)
        {
            BoardGenerales dataBoard = JsonConvert.DeserializeObject<BoardGenerales>(data.String1);
            var result = new EsoAntecedentesBL().UpdateDataNinio(dataBoard, data.Int1, data.Int2);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult SaveDataAdolescente(MultiDataModel data)
        {
            BoardGenerales dataBoard = JsonConvert.DeserializeObject<BoardGenerales>(data.String1);
            var result = new EsoAntecedentesBL().SaveDataAdolescente(dataBoard, data.Int1, data.Int2);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult UpdateDataAdolescente(MultiDataModel data)
        {
            BoardGenerales dataBoard = JsonConvert.DeserializeObject<BoardGenerales>(data.String1);
            var result = new EsoAntecedentesBL().UpdateDataAdolescente(dataBoard, data.Int1, data.Int2);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult SaveDataAdulto(MultiDataModel data)
        {
            BoardGenerales dataBoard = JsonConvert.DeserializeObject<BoardGenerales>(data.String1);
            var result = new EsoAntecedentesBL().SaveDataAdulto(dataBoard, data.Int1, data.Int2);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult UpdateDataAdulto(MultiDataModel data)
        {
            BoardGenerales dataBoard = JsonConvert.DeserializeObject<BoardGenerales>(data.String1);

            var result = new EsoAntecedentesBL().UpdateDataAdulto(dataBoard, data.Int1, data.Int2);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult SaveDataAdultoMayor(MultiDataModel data)
        {
            BoardGenerales dataBoard = JsonConvert.DeserializeObject<BoardGenerales>(data.String1);
            var result = new EsoAntecedentesBL().SaveDataAdultoMayor(dataBoard, data.Int1, data.Int2);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult UpdateDataAdultoMayor(MultiDataModel data)
        {
            BoardGenerales dataBoard = JsonConvert.DeserializeObject<BoardGenerales>(data.String1);
            var result = new EsoAntecedentesBL().UpdateDataAdultoMayor(dataBoard, data.Int1, data.Int2);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetDataGeneral(string personId)
        {

            var result = new EsoAntecedentesBL().GetDataGeneralEtario(personId);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult SaveCuidadosPreventivos(MultiDataModel data)
        {
            EsoCuidadosPreventivosFechas dataCudiados = JsonConvert.DeserializeObject<EsoCuidadosPreventivosFechas>(data.String1);

            var result = new EsoAntecedentesBL().SaveCuidadosPreventivos(dataCudiados, data.String2, data.Int1, data.Int2);
            return Ok(result);
        }
    }
}
