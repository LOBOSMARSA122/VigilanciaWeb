using BE.Common;
using BE.Message;
using BE.ServiceOrder;
using BL.Common;
using BL.Email;
using BL.ServiceOrder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.ServiceOrder
{
    public class ServiceOrderController : ApiController
    {
        [HttpPost]
        public IHttpActionResult AddServiceOrder(MultiDataModel multiData)
        {
            BoardServiceOrder data = JsonConvert.DeserializeObject<BoardServiceOrder>(multiData.String1);

            var result = ServiceOrderBL.AddServiceOrder(data, multiData.Int1, multiData.Int2);

            return Ok(result);

        }

        [HttpPost]
        public IHttpActionResult GenerateServiceOrderReport(MultiDataModel multiData)
        {
            BoardServiceOrder data = JsonConvert.DeserializeObject<BoardServiceOrder>(multiData.String1);

            var result = ServiceOrderBL.GenerateServiceOrderReport(data, multiData.Int1, data.FechaEmision);

            return Ok(result);

        }

        [HttpGet]
        public IHttpActionResult GetServicesOrderDetail(string serviceOrderId)
        {

            var result = ServiceOrderBL.GetServicesOrderDetail(serviceOrderId);
            return Ok(result);

        }

        [HttpGet]
        public IHttpActionResult DeletedServiceOrderDetail(string serviceOrderDetail, int userId)
        {

            var result = ServiceOrderBL.DeletedServiceOrderDetail(serviceOrderDetail, userId);
            return Ok(result);

        }

        [HttpPost]
        public IHttpActionResult SendEmail(MultiDataModel data)
        {
            EmailModel model = JsonConvert.DeserializeObject<EmailModel>(data.String1);
            var result = SendEmailBL.SendEmail(model);

            return Ok(result);
            
        }


    }
}
