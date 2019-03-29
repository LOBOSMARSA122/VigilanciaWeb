using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SigesoftWeb.Controllers.Security;
using SigesoftWeb.Models;
using SigesoftWeb.Models.Notification;
using SigesoftWeb.Models.Worker;
using WebPush;

namespace SigesoftWeb.Controllers
{
    public class WorkerDataController : Controller
    {
        [GeneralSecurity(Rol = "WorkerPwa-Index")]
        public ActionResult Index()
        {
            
            ViewBag.PublicKey = "BKtlRblOBWPha7rSZ8pKA6NRsFz6PzKu1aQ0_nk4e5P_bANWknvjg0ViRmYVLTHDbeQurDv7YKt9J9luQm_EnYM";

            Api API = new Api();

            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "personId" , ViewBag.USER.PersonId }
            };
            
            ViewBag.UserData = API.Get<WorkerPwa>("WorkerPwa/WorkerInformationPwa", arg);
            ViewBag.IsSubscribe = API.Get<List<Notification>>("Subscribe/GetSubscriptions", arg);
            ViewBag.Photo = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(ViewBag.USER.PersonImage));
            return View();
        }

        [GeneralSecurity(Rol = "WorkerPwa-Subscribe")]
        public JsonResult Subscribe(string data)
        {
            Api API = new Api();
            var oVapidBe = new VapidBe();
            oVapidBe.Subs = data;
            oVapidBe.PersonId = ViewBag.USER.PersonId;
            string url = "Subscribe/Subscribe";
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(oVapidBe)}
            };
            var result = API.Post<string>(url,arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "WorkerPwa-Notification")]
        public JsonResult SaveNotification(Notification notification)
        {
            Api API = new Api();
            notification.i_InsertUserId = ViewBag.USER.SystemUserId;

            string url = "Notification/SaveNotification";
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(notification)},
                { "Int1", ViewBag.USER.NodeId.ToString()},
                { "Int2", ViewBag.USER.SystemUserId.ToString()},
            };
            var result = API.Post<string>(url, arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "WorkerPwa-Notification")]
        public JsonResult UnSubscribe()
        {
            Api API = new Api();
            VapidBe vapid = new VapidBe();
            vapid.PersonId = ViewBag.USER.PersonId;
            string url = "Subscribe/UnSubscribe";
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(vapid)},
            };
            var result = API.Post<string>(url, arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "WorkerPwa-SendNotification")]
        public ActionResult SendNotification(string personId, string notificationId)
        {
            Api API = new Api();
            string url = "Notification/SendNotification";

            Dictionary<string, string> arg = new Dictionary<string, string>()
                {
                    { "personId", personId},
                    { "notificationId", notificationId},
                };
            var notifications = API.Get<List<SendNotificationBE>>(url, arg);
            
            foreach (var item in notifications)
            {

                var objNotification = JsonConvert.DeserializeObject<KeyNotification>(item.Subs);

                var pushEndpoint = objNotification.endpoint;
                var p256dh = objNotification.Keys.p256dh;
                var auth = objNotification.Keys.auth;

                var oMessage = new Message();
                oMessage.title = item.Title;
                oMessage.message = item.Message;

                //var payload = "{'title':" +item.Title + "','message':" + item.Body + "}";
                var payload = JsonConvert.SerializeObject(oMessage);
                var options = new Dictionary<string, object>();
                options["vapidDetails"] = new VapidDetails("mailto:beto1826@hotmail.com", "BKtlRblOBWPha7rSZ8pKA6NRsFz6PzKu1aQ0_nk4e5P_bANWknvjg0ViRmYVLTHDbeQurDv7YKt9J9luQm_EnYM", "h__qPKs38j65-UgxnRnfcVrDMEzriFvSVqL8EkugvXw");
                options["gcmAPIKey"] = @"[your key here]";

                var webPushClient = new WebPushClient();

                Thread t = new Thread((a) => {
                    try
                    {
                        webPushClient.SendNotification((PushSubscription)a,
                            payload, options);
                    }
                    catch (WebPushException exception)
                    {
                        Console.WriteLine("Http STATUS code" + exception.StatusCode);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                });
                var s = new PushSubscription(pushEndpoint, p256dh, auth);
                t.Start(s);
            }

            return new JsonResult { Data = notifications, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        
        [GeneralSecurity(Rol = "WorkerPwa-SendNotification")]
        public void ReNotify(List<ReNotify> reNotify)
        {
            Api API = new Api();
            string url = "Notification/SendNotification";

            foreach (var notify in reNotify)
            {
                Dictionary<string, string> arg = new Dictionary<string, string>()
                {
                    { "personId", notify.PersonId},
                    { "notificationId", notify.NotifyId},
                };
                var notifications = API.Get<List<SendNotificationBE>>(url, arg);

                foreach (var item in notifications)
                {

                    var objNotification = JsonConvert.DeserializeObject<KeyNotification>(item.Subs);

                    var pushEndpoint = objNotification.endpoint;
                    var p256dh = objNotification.Keys.p256dh;
                    var auth = objNotification.Keys.auth;

                    var oMessage = new Message();
                    oMessage.title = item.Title;
                    oMessage.message = item.Message;

                    //var payload = "{'title':" +item.Title + "','message':" + item.Body + "}";
                    var payload = JsonConvert.SerializeObject(oMessage);
                    var options = new Dictionary<string, object>();
                    options["vapidDetails"] = new VapidDetails("mailto:beto1826@hotmail.com", "BKtlRblOBWPha7rSZ8pKA6NRsFz6PzKu1aQ0_nk4e5P_bANWknvjg0ViRmYVLTHDbeQurDv7YKt9J9luQm_EnYM", "h__qPKs38j65-UgxnRnfcVrDMEzriFvSVqL8EkugvXw");
                    options["gcmAPIKey"] = @"[your key here]";

                    var webPushClient = new WebPushClient();

                    Thread t = new Thread((a) => {
                        try
                        {
                            webPushClient.SendNotification((PushSubscription)a,
                                payload, options);
                        }
                        catch (WebPushException exception)
                        {
                            Console.WriteLine("Http STATUS code" + exception.StatusCode);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    });
                    var s = new PushSubscription(pushEndpoint, p256dh, auth);
                    t.Start(s);
                }
            }
            

            //return new JsonResult { Data = notifications, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "WorkerPwa-Notification")]
        public JsonResult UpdateWorker(WorkerPwa oWorkerPwa)
        {
            Api API = new Api();

            oWorkerPwa.PersonId = ViewBag.USER.PersonId;

            string url = "WorkerPwa/UpdateWorker";
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(oWorkerPwa)},
            };
            var result = API.Post<bool>(url, arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "WorkerPwa-Notification")]
        public JsonResult GetNotification(string notificationId)
        {
            Api API = new Api();

            string url = "Notification/GetNotification";
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "notificationId", notificationId},
            };
            var result = API.Get<NotificationsBE>(url, arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "WorkerPwa-Notification")]
        public JsonResult Notifications()
        {
            Api API = new Api();

            string url = "Notification/Notifications";
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "personId" , ViewBag.USER.PersonId }
            };
            var result = API.Get<List<NotificationsBE>>(url, arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [GeneralSecurity(Rol = "WorkerPwa-ReadNotification")]
        public JsonResult ReadNotification(string notificationId)
        {
            Api API = new Api();

            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "notificationId", notificationId},
            };
            var result = API.Get<bool>("Notification/ReadNotification", arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [GeneralSecurity(Rol = "WorkerPwa-ReadNotification")]
        public JsonResult SendMedicalAlert(NotificationAlert oNotificationAlert)
        {
            Api API = new Api();
            oNotificationAlert.nodeId = ViewBag.USER.NodeId;
            oNotificationAlert.systemUserId = ViewBag.USER.SystemUserId;
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(oNotificationAlert)},
            };
            var result = API.Post<string>("Notification/SendMedicalAlert", arg);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}