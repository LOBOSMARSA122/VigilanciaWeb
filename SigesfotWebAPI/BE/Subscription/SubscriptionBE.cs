using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Subscription
{
    [Table("subscription")]
    public class SubscriptionDto 
    {
        [Key]
        public string v_SubscriptionId { get; set; }
        public string v_PersonId { get; set; }
        public string v_Subs { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
         
    }
}
