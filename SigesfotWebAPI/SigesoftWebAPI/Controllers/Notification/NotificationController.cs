using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BE.Common;
using BE.Notification;
using BL.Notification;
using Newtonsoft.Json;

namespace SigesoftWebAPI.Controllers
{
    public class NotificationController : ApiController
    {
        NotificationBl oNotificationBl = new NotificationBl();

        [HttpPost]
        public IHttpActionResult FilterNotifications(BoardNotification data)
        {
            var result = oNotificationBl.FilterNotifications(data);
            return Ok(result);
        }
        
        [HttpPost]
        public IHttpActionResult SaveNotification(MultiDataModel model)
        {
            if (string.IsNullOrWhiteSpace(model.String1))
                return BadRequest("Información Inválida");
            var data = JsonConvert.DeserializeObject<NotificationDto>(model.String1);

            var result =  oNotificationBl.SaveNotification(data, model.Int1, model.Int2);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult Notifications(string personId)
        {
            var result = oNotificationBl.Notifications(personId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetNotification(string notificationId)
        {
            var result = oNotificationBl.GetNotification(notificationId);
            return Ok(result);
        }
        
        [HttpGet]
        public IHttpActionResult SendNotification(string personId, string notificationId)
        {
            var result = oNotificationBl.SendNotification(personId, notificationId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult ReadNotification(string notificationId)
        {
            var result = oNotificationBl.ReadNotification(notificationId);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult SendMedicalAlert(MultiDataModel model)
        {
            if (string.IsNullOrWhiteSpace(model.String1))
                return BadRequest("Información Inválida");
            var data = JsonConvert.DeserializeObject<NotificationAlertBE>(model.String1);

            var result = oNotificationBl.SendMedicalAlert(data.personId, data.organizationId, data.planVigilancia, data.nodeId, data.systemUserId);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult ScheduleNotification(MultiDataModel model)
        {
            if (string.IsNullOrWhiteSpace(model.String1))
                return BadRequest("Información Inválida");

            var data = JsonConvert.DeserializeObject<NotificationDto>(model.String1);

            Dictionary<string, byte[]> documents = JsonConvert.DeserializeObject<Dictionary<string, byte[]>>(model.String2);
            string path = string.Format("{0}{1}\\", System.Web.Hosting.HostingEnvironment.MapPath("~/"), System.Configuration.ConfigurationManager.AppSettings["directorioCAMP"].ToString());

            var response = oNotificationBl.ScheduleNotification(data, documents, path, model.Int1, model.Int2);

            return Ok(response);
        }

        //[HttpPost]
        //public IHttpActionResult ReNotify(MultiDataModel model)
        //{
        //    if (string.IsNullOrWhiteSpace(model.String1))
        //        return BadRequest("Información Inválida");
        //    var data = JsonConvert.DeserializeObject<List<string>>(model.String1);

        //    var result = oNotificationBl.ReNotify(data);
        //    return Ok(result);
        //}

    }
}
