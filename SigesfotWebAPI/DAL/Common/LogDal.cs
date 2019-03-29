using BE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BE.Common.Enumeratores;

namespace DAL.Common
{
    public class LogDal
    {
        private static DatabaseContext ctx = new DatabaseContext();
        
        public void SaveLog(string pintNodeId, string pintOrganizationId, string pintSystemUserId, LogEventType pEnuEventType, string pstrProcess, string pstrItem, Success pEnuSuccess, string pstrErrorMessage)
        {
            LogBE oLogBE = new LogBE();

            oLogBE.i_NodeLogId = int.Parse(pintNodeId);
            oLogBE.i_SystemUserId = pintSystemUserId == null ? (int?)null : int.Parse(pintSystemUserId);
            oLogBE.i_EventTypeId = (int)pEnuEventType;
            oLogBE.v_ProcessEntity = pstrProcess;
            oLogBE.v_ElementItem = pstrItem;
            oLogBE.i_Success = (int)pEnuSuccess;
            oLogBE.v_ErrorException = pstrErrorMessage;
            oLogBE.d_Date = DateTime.Now;

            int intNodeId = int.Parse(pintNodeId);
            oLogBE.v_LogId = new Utils().GetPrimaryKey(intNodeId, 7, "LV"); 
           
            ctx.Log.Add(oLogBE);
            ctx.SaveChanges();
        }
    }
}
