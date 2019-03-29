using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using BE.Service;
using DAL.Common;

namespace DAL.Calendar
{
    public class CalendarDal
    {
        private static DatabaseContext ctx = new DatabaseContext();

        public void AddCalendar(CalendarDto oCalendarDto, int nodeId, int systemUserId)
        {
            var calendarId = new Common.Utils().GetPrimaryKey(nodeId, 22, "CA");

            oCalendarDto.v_CalendarId = calendarId;

            oCalendarDto.i_IsDeleted = (int) Enumeratores.SiNo.No;
            oCalendarDto.d_InsertDate = DateTime.UtcNow;
            oCalendarDto.i_InsertUserId = systemUserId;
            
            ctx.Calendar.Add(oCalendarDto);
            ctx.SaveChanges();
        }
    }
}
