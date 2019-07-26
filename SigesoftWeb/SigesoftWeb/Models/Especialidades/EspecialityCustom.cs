using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Models.Especialidades
{
    public class Boards
    {
        public int TotalRecords { get; set; }
        public int Index { get; set; }
        public int Take { get; set; }
    }

    public class BoardEspeciality : Boards
    {
        public string Especiality { get; set; }
        public List<EspecialityCustom> List { get; set; }
    }

    public class EspecialityCustom
    {
        public string v_ProtocolId { get; set; }
        public string v_EspecialityId { get; set; }
        public string v_EspecialityName { get; set; }
        public byte[] b_EspecialityPicture { get; set; }
        public string v_EspecialityPicture { get; set; }
        public TimeSpan? t_TimeForAttention { get; set; }
        public decimal? r_Cost { get; set; }
        public string v_Description { get; set; }
        public int? i_IsDeleted { get; set; }
        public TimeSpan? t_StartTime { get; set; }
        public TimeSpan? t_EndTime { get; set; }
        public TimeSpan? t_StartTime2 { get; set; }
        public TimeSpan? t_EndTime2 { get; set; }
    }
}