using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Models.Notification
{
    public class Notification
    {
        public string v_NotificationId { get; set; }
        public string v_OrganizationId { get; set; }
        public DateTime? d_NotificationDate { get; set; }
        public string v_PersonId { get; set; }
        public string v_Title { get; set; }
        public string v_Body { get; set; }

        public int? i_IsRead { get; set; }
        public int? i_TypeNotificationId { get; set; }
        public DateTime? d_ScheduleDate { get; set; }
        public int? i_StateNotificationId { get; set; }
        public string v_Path { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
    }

    public class NotificationsBE
    {
        public string PersonId { get; set; }
        public string NotificationId { get; set; }

        public string Organization { get; set; }
        public int? TypeNotificationId { get; set; }
        public string TypeNotification { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime? NotificationDate { get; set; }
        public string NotificationDateString { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public string ScheduleDateString { get; set; }

        public int? SystemUserId { get; set; }
        public string SystemUser { get; set; }
        public int? IsRead { get; set; }
        public string Read { get; set; }
        public string Worker { get; set; }
        public string StateNotification { get; set; }
    }
}