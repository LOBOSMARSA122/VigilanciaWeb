using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Common;

namespace DAL.Log
{
    public class LogDal
    {
        public static void SaveLog(string pintNodeId, string pintOrganizationId, string pintSystemUserId, Enumeratores.LogEventType pEnuEventType, string pstrProcess, string pstrItem, Enumeratores.Success pEnuSuccess, string pstrErrorMessage)
        {

            using (var dbContext = new DatabaseContext())
            {
                LogBE objEntity = new LogBE();

                objEntity.i_NodeLogId = int.Parse(pintNodeId);
                objEntity.i_SystemUserId = pintSystemUserId == null ? (int?)null : int.Parse(pintSystemUserId);
                objEntity.i_EventTypeId = (int)pEnuEventType;
                objEntity.v_ProcessEntity = pstrProcess;
                objEntity.v_ElementItem = pstrItem;
                objEntity.i_Success = (int)pEnuSuccess;
                objEntity.v_ErrorException = pstrErrorMessage;
                objEntity.d_Date = DateTime.Now;

                //Autogeneramos el Pk de la tabla
                int intNodeId = int.Parse(pintNodeId);
                objEntity.v_LogId = new Common.Utils().GetPrimaryKey(intNodeId, 7, "LV");

                dbContext.SaveChanges();
            }

            
        }

    }
}
