using BE.Calendar;
using BE.Common;
using BE.Component;
using BE.Message;
using BE.Service;
using DAL.Calendar;
using DAL.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BE.Common.Enumeratores;

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

        public List<CalendarList> GetPacientInLineByComponentId(DateTime CurrentDate, List<string> components,int masterServiceId)
        {

            return _CalendarDAL.GetPacientInLineByComponentId(CurrentDate, components, masterServiceId);
        }

        public List<CalendarList> GetPacientInLineByComponentId_ATX(DateTime CurrentDate, List<string> components, int masterServiceId, int UserId)
        {

            return _CalendarDAL.GetPacientInLineByComponentId_ATX(CurrentDate, components, masterServiceId, UserId);
        }

        public MessageCustom CircuitStart(string CalendarId, int userId)
        {
            MessageCustom _MessageCustom = new MessageCustom();
            var result = new CalendarDal().CircuitStart(CalendarId, userId);
            if (result)
            {
                _MessageCustom.Error = false;
                _MessageCustom.Status = (int)StatusHttp.Ok;
                _MessageCustom.Message = "El circuito se inició correctamente";
            }
            else
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = (int)StatusHttp.BadRequest;
                _MessageCustom.Message = "Sucedió un error al iniciar el circuito, porfavor vuelva a intentar.";
            }

            return _MessageCustom;
        }

        public MessageCustom SaveAdditionalExamsForCalendar(List<AdditionalExamCreate> data, int userId, int nodeId)
        {
            MessageCustom _MessageCustom = new MessageCustom();
            var result = new CalendarDal().SaveAdditionalExamsForCalendar(data, userId, nodeId);
            if (!result)
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = 500;
                _MessageCustom.Message = "Sucedió un error al guardar los cambios.";
            }
            else
            {
                _MessageCustom.Error = false;
                _MessageCustom.Status = 200;
                _MessageCustom.Message = "Se guardaron los cambios.";
            }
            return _MessageCustom;
        }
    }
}
