using BE.Calendar;
using BE.Common;
using BE.Component;
using BE.Message;
using BE.Service;
using BL.AdditionalExam;
using BL.Calendar;
using BL.Component;
using BL.Service;
using DAL.Component;
using DAL.Protocol;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;
using static BE.Common.Enumeratores;

namespace SigesoftWebAPI.Controllers.Calendar
{
    public class CalendarController : ApiController
    {
        [HttpPost]
        public IHttpActionResult GetDataCalendar(MultiDataModel data)
        {
            var dataFilter = JsonConvert.DeserializeObject<BoardCalendar>(data.String1);
            var result = new CalendarBL().GetDataCalendar(dataFilter);
            return Ok(result);

        }

        [HttpPost]
        public IHttpActionResult CreateCalendar(MultiDataModel data)
        {
            MessageCustom _MessageCustom = new MessageCustom();
            try
            {
                using (var ts = new TransactionScope())
                {

                    var dataService = JsonConvert.DeserializeObject<ServiceCustom>(data.String1);
                    string serviceId = new ServiceBl().CreateService(dataService, data.Int1, data.Int2);

                    if (serviceId == null) throw new Exception("Sucedió un error al generar la agenda, por ello no se guardó ningún cambio.");
                    else
                    _MessageCustom.Error = false;
                    _MessageCustom.Status = (int)StatusHttp.Ok;
                    _MessageCustom.Id = serviceId;
                    _MessageCustom.Message = "Se agendó correctamente.";
                    ts.Complete();
                    return Ok(_MessageCustom);                
                }
            }
            catch (Exception ex)
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = (int)StatusHttp.BadRequest;
                _MessageCustom.Message = ex.Message;
                return Ok(_MessageCustom);
            }
            
            
        }

        [HttpPost]
        public IHttpActionResult UpdateServiceForProtocol(MultiDataModel data)
        {
            MessageCustom _MessageCustom = new MessageCustom();
            try
            {
                using (var ts = new TransactionScope())
                {

                    var dataService = JsonConvert.DeserializeObject<ServiceCustom>(data.String1);
                    bool resultService = new ServiceBl().UpdateServiceForProtocol(dataService, data.Int1);
                    if(!resultService) throw new Exception("Sucedió un error al actualizar el servicio, por ello no se guardó ningún cambio.");

                    bool resultCalendar = new CalendarBL().UpdateCalendarForProtocol(dataService, data.Int1);
                    if (!resultCalendar) throw new Exception("Sucedió un error al actualizar la agenda, por ello no se guardó ningún cambio.");

                    else
                    _MessageCustom.Error = false;
                    _MessageCustom.Status = (int)StatusHttp.Ok;
                    _MessageCustom.Id = "";
                    _MessageCustom.Message = "Se actualizó correctamente.";
                    ts.Complete();
                    return Ok(_MessageCustom);
                }
            }
            catch (Exception ex)
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = (int)StatusHttp.BadRequest;
                _MessageCustom.Message = ex.Message;
                return Ok(_MessageCustom);
            }
        }

        [HttpPost]
        public IHttpActionResult RegistrarCarta(MultiDataModel data)
        {
            MessageCustom _MessageCustom = new MessageCustom();
            try
            {
                bool result = new ServiceBl().RegistrarCarta(data);

                if (!result) throw new Exception("Sucedió un error al agregar la carta, por favor vuelva a intentar.");
                else
                    _MessageCustom.Error = false;
                _MessageCustom.Status = (int)StatusHttp.Ok;
                _MessageCustom.Id = data.String1;
                _MessageCustom.Message = "Se agregó correctamente.";
                return Ok(_MessageCustom);
            }
            catch (Exception ex)
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = (int)StatusHttp.BadRequest;
                _MessageCustom.Message = ex.Message;
                return Ok(_MessageCustom);
            }


        }

        [HttpPost]
        public IHttpActionResult FusionarServicios(MultiDataModel data)
        {
            List<string> ServicesId = JsonConvert.DeserializeObject<List<string>>(data.String1);
            var result = new ServiceBl().FusionarServicios(ServicesId, data.Int1, data.Int2);
            return Ok(result);
        }
        [HttpGet]
        public IHttpActionResult GetListSaldosPaciente(string serviceId)
        {
            var result = new ServiceBl().GetListSaldosPaciente(serviceId);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult GetPacientInLineByComponentId(MultiDataModel data)
        {
            var ListComponent = new ComponentDal().GetAllComponents();
            List<string> Components = new List<string>();
            if (data.Int3 == -1)
            {
                Components.Add(data.String2);
            }
            else
            {
                Components = ListComponent.FindAll(p => p.Value4 == data.Int2)
                                                .Select(s => s.Value2)
                                                .OrderBy(p => p).ToList();
            }

            var dateTime = DateTime.Parse(data.String1);
            var result = new CalendarBL().GetPacientInLineByComponentId(dateTime, Components,  data.Int1);
            return Ok(result);
        }


        [HttpPost]
        public IHttpActionResult GetPacientInLineByComponentId_Atx(MultiDataModel data)
        {
            var ListComponent = new ComponentDal().GetAllComponents();
            List<string> Components = new List<string>();
            if (data.Int3 == -1)
            {
                Components.Add(data.String2);
            }
            else
            {
                Components = ListComponent.FindAll(p => p.Value4 == data.Int2)
                                                .Select(s => s.Value2)
                                                .OrderBy(p => p).ToList();
            }

            var dateTime = DateTime.Parse(data.String1);
            var result = new CalendarBL().GetPacientInLineByComponentId_ATX(dateTime, Components, data.Int1, data.Int3);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult IniciarCircuito(MultiDataModel data)
        {
            var result = new CalendarBL().CircuitStart(data.String1, data.Int1);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetExamenesAdicionales(string ServiceId)
        {
            var result = new AdditionalExamBL().GetAdditionalExamByServiceId(ServiceId);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult SaveAdditionalExamsForCalendar(MultiDataModel data)
        {
            List<AdditionalExamCreate> dataAdd = JsonConvert.DeserializeObject<List<AdditionalExamCreate>>(data.String1);

            var result = new CalendarBL().SaveAdditionalExamsForCalendar(dataAdd, data.Int1, data.Int2);
            return Ok(result);
        }
    }
}
