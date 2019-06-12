using BE.Calendar;
using BE.Service;
using DAL.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Calendar
{
    public class CalendarBL
    {
        CalendarDal _CalendarDAL = new CalendarDal();
        public BoardCalendar GetDataCalendar(BoardCalendar data)
        {
            return _CalendarDAL.GetAllCalendar(data);
        }

        public bool UpdateCalendarForProtocol(ServiceCustom data, int userId)
        {
            return _CalendarDAL.UpdateCalendarForProtocol(data, userId);
        }
    }
}
