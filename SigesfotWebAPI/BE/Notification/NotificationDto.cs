using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Notification
{
    [Table("notification")]
    public class NotificationDto
    {
        [Key]
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
}
