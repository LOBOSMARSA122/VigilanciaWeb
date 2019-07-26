using BE.Common;
using BE.Component;
using BE.Message;
using BL.AdditionalExam;
using BL.Component;
using BL.Consultorio;
using DAL.Component;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.Consultorio
{
    public class ConsultorioController : ApiController
    {
        [HttpPost]
        public IHttpActionResult GetExamenes(MultiDataModel data)
        {
            BoardExamsCustom dataBoard = JsonConvert.DeserializeObject<BoardExamsCustom>(data.String1);

            var result = new ComponentBl().GetExamsForConsult(dataBoard);

            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult CallingPacient(MultiDataModel data)
        {
            BoardExamsCustom dataBoard = JsonConvert.DeserializeObject<BoardExamsCustom>(data.String1);

            var result = new ConsultorioBL().CallingPacient(dataBoard);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult EliminarExamen(MultiDataModel data)
        {
            BoardExamsCustom dataBoard = JsonConvert.DeserializeObject<BoardExamsCustom>(data.String1);

            var result = new ConsultorioBL().EliminarExamen(dataBoard, data.Int1);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult GetAdditionalExams(MultiDataModel data)
        {
            BoardExamsCustom dataBoard = JsonConvert.DeserializeObject<BoardExamsCustom>(data.String1);

            var result = new ConsultorioBL().GetAdditionalExams(dataBoard);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult BuscarCoincidencia(string serviceId, string componentId)
        {
            var result = new ConsultorioBL().BuscarCoincidencia(serviceId, componentId);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult SaveExamsAdditional(MultiDataModel data)
        {
            List<AdditionalExamCustom> listExams = JsonConvert.DeserializeObject<List<AdditionalExamCustom>>(data.String1);

            var result = new ConsultorioBL().SaveadditionalExams(listExams, data.Int1, data.Int2);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult LiberarPaciente(MultiDataModel data)
        {
            MessageCustom _MessageCustom = new MessageCustom();
            var result = new ConsultorioBL().LiberarPaciente(data.Int1, data.String1, data.String2, data.String3);
            if (result)
            {
                _MessageCustom.Error = false;
                _MessageCustom.Status = 200;
                _MessageCustom.Message = "Se liberó al paciente";
            }
            else
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = 500;
                _MessageCustom.Message = "Sucedió un error, vuelva a intentar";
            }
            return Ok(_MessageCustom);
        }
        [HttpGet]
        public IHttpActionResult GetAllComponent()
        {
            var result = new ComponentDal().GetAllComponents();
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult ViewAdditionalExams(string serviceId, int userId)
        {
            var result = new AdditionalExamBL().GetAdditionalExamForUpdateByServiceId(serviceId, userId);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult SaveUpdateAdditionalExam(MultiDataModel data)
        {
            List<string> ListaComponentes = JsonConvert.DeserializeObject<List<string>>(data.String2);
            List<string> ListaExamenAddicionalId = JsonConvert.DeserializeObject<List<string>>(data.String1);
            var result = new AdditionalExamBL().UpdateComponentAdditionalExam(ListaComponentes, ListaExamenAddicionalId, data.Int1, data.String3, data.String4);
            
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult SaveDeletedAdditionalExam(MultiDataModel data)
        {

            var result = new AdditionalExamBL().SaveDeletedAdditionalExam(data.String1, data.Int1, data.String2, data.String3);

            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult ReImprimirAdicionales(MultiDataModel data)
        {

            var result = new ConsultorioBL().ReImprimirAdicional(data.Int1, data.String1);

            return Ok(result);
        }
    }
}
