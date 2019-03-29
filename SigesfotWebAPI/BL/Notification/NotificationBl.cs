using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using BE.Notification;
using DAL.Notification;
using DAL.Subscription;

namespace BL.Notification
{
    public class NotificationBl
    {
        public BoardNotification FilterNotifications(BoardNotification data)
        {
            return new NotificationDal().FilterNotifications(data);
        }

        public string SaveNotification(NotificationDto oNotificationDto, int nodeId, int systemUserId)
        {
            if (VerifySubscription(oNotificationDto.v_PersonId))
            {
                if (oNotificationDto.v_Title == null) oNotificationDto.v_Title = Constants.TITLE_DEFAULT;

                return new NotificationDal().SaveNotification(oNotificationDto, nodeId, systemUserId);
            }
            return "Sin subscripcion";
        }

        public List<NotificationsBE> Notifications(string personId)
        {
            return new NotificationDal().Notifications(personId);
        }

        public NotificationsBE GetNotification(string notificationId)
        {
            return new NotificationDal().GetNotification(notificationId);
        }
        
        public List<SendNotificationBE> SendNotification(string personId, string notificationId)
        {
            return new NotificationDal().SendNotification(personId, notificationId);
        }

        public bool ReadNotification(string notificationId)
        {
            return new NotificationDal().ReadNotification(notificationId);
        }

        public string SendMedicalAlert(string personId, string organizationId, string planVigilancia, int nodeId, int systemUserId)
        {
            if (VerifySubscription(personId))
            {
                var oNotificationDto = new NotificationDto();
                oNotificationDto.v_PersonId = personId;
                oNotificationDto.v_OrganizationId = organizationId;
                oNotificationDto.i_TypeNotificationId = (int)Enumeratores.TypeNotification.AlertaMedica;
                oNotificationDto.v_Title = planVigilancia;
                oNotificationDto.v_Body = Constants.BODY_ALERT_MEDICAL + " " + planVigilancia;
                var notificationId =  new NotificationDal().SaveNotification(oNotificationDto, nodeId, systemUserId);

                new NotificationDal().SendNotification(personId, notificationId);
                return notificationId;
            }
            return null;
        }

        private bool VerifySubscription(string personId)
        {
           var subs =  new SubscriptionDal().GetSubscriptions(personId);
           return subs.Count != 0;
        }

        public List<string> ScheduleNotification(NotificationDto oNotificationDto, Dictionary<string, byte[]> documents, string path, int nodeId, int systemUserId)
        {
            SaveNotification(oNotificationDto, nodeId, systemUserId);

            List<string> return_data = new List<string>();
            foreach (var document in documents)
            {
                path += document.Key;

                File.WriteAllBytes(path, document.Value);

            }

            return return_data;
        }
            //public string ReNotify(List<string> notificationIds)
            //{
            //    foreach (var notificationId in notificationIds)
            //    {

            //    }

            //    return "OK";
            //}
        }
}
