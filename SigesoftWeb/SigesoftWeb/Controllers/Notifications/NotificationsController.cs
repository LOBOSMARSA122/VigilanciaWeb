using Newtonsoft.Json;
using SigesoftWeb.Controllers.Security;
using SigesoftWeb.Models;
using SigesoftWeb.Models.Common;
using SigesoftWeb.Models.Notification;
using SigesoftWeb.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SigesoftWeb.Controllers.NotificationsController
{
    public class NotificationsController : Controller
    {
        [GeneralSecurity(Rol = "Notifications-Index")]
        public ActionResult Index()
        {
            Api API = new Api();

            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "grupoId" , ((int)Enums.SystemParameter.NotificationType).ToString() },
            };
            Dictionary<string, string> arg2 = new Dictionary<string, string>()
            {
                { "grupoId" , ((int)Enums.SystemParameter.StateNotification).ToString() },
            };
            ViewBag.NotificationType = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId", arg), Constants.All);
            ViewBag.EstateNotification = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId", arg2), Constants.All);
            return View();
        }

        [GeneralSecurity(Rol = "Notifications-BoardMailBodyPartial")]
        public ActionResult Filter(BoardNotification data)
        {
            Api API = new Api();

            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "TypeNotificationId" , data.TypeNotificationId.ToString()},
                { "OrganizationId" , data.OrganizationId},
                { "Worker" , data.Worker},
                { "StateNotificationId" , data.StateNotificationId.ToString()},
                { "NotificationDateStart" ,data.NotificationDateStart},
                { "NotificationDateEnd" , data.NotificationDateEnd},
                { "Title" , data.Title},

                { "Index", data.Index.ToString()},
                { "Take", data.Take.ToString()}
            };
            Dictionary<string, string> arg2 = new Dictionary<string, string>()
            {
                { "grupoId" , ((int)Enums.SystemParameter.NotificationType).ToString() },
            };

            ViewBag.NotificationType = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId", arg2), Constants.Select);
            ViewBag.Notifications = API.Post<BoardNotification>("Notification/FilterNotifications", arg);
            return PartialView("_BoardMailBodyPartial");
        }

        [GeneralSecurity(Rol = "Notifications-BoardMailBodyPartial")]
        public JsonResult ReNotify(string jsonArray)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1" , jsonArray},
            };
            var url = "Notification/ReNotify";
            var result = API.Post<string>(url, arg);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }

        [GeneralSecurity(Rol = "Notifications-SendFile")]
        public JsonResult SendFile()
        {
            Api API = new Api();
            var nuevo = Request.Form;
            Dictionary<string, byte[]> list = new Dictionary<string, byte[]>();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                using (var binaryReader = new BinaryReader(Request.Files[i].InputStream))
                {
                    list.Add(Request.Files[i].FileName, binaryReader.ReadBytes(Request.Files[i].ContentLength));
                }
            }


            Dictionary<string, string> args = new Dictionary<string, string>()
            {

                {"String2", JsonConvert.SerializeObject(list)}
            };

            //var response = API.Post<List<string>>("Notification/ScheduleNotification", args);

            return Json(list);
        }

        [GeneralSecurity(Rol = "Notifications-SendFile")]
        public JsonResult ScheduleNotification(string data, string file)
        {
            Api API = new Api();
            //Notification data = JsonConvert.DeserializeObject(String1);

            Dictionary<string, string> args = new Dictionary<string, string>()
            {

                {"String1", data},
                {"String2", file},
                {"Int1", ViewBag.USER.NodeId.ToString()},
                {"Int2", ViewBag.USER.SystemUserId.ToString()}
            };

            var response = API.Post<List<string>>("Notification/ScheduleNotification", args);

            return Json(response);
        }

    }
}
