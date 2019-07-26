using BE.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ProtocolSystemUser
{
    public class ProtocolSystemUserDal
    {

        public bool AddProtocolSystemUser(List<ProtocolSystemUserBE> ListProtocolSystemUserDto, int pintSystemUserId, int userId, int nodeId)
        {

            string newId;

            try
            {
                DatabaseContext dbContext = new DatabaseContext();

                foreach (var objEntity in ListProtocolSystemUserDto)
                {
                    // Autogeneramos el Pk de la tabla
                    newId = new Common.Utils().GetPrimaryKey(nodeId, 44, "PU");

                    // Grabar como nuevo

                    objEntity.v_ProtocolSystemUserId = newId;
                    objEntity.i_SystemUserId = pintSystemUserId;

                    objEntity.d_InsertDate = DateTime.Now;
                    objEntity.i_InsertUserId = userId;
                    objEntity.i_IsDeleted = 0;
                    dbContext.ProtocolSystemUser.Add(objEntity);
                    dbContext.SaveChanges();
                }

                

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public static bool DeletedProtocolSystemUser(int systemUserId, int userId)
        {
            try
            {
                DatabaseContext ctx = new DatabaseContext();
                var listProt = ctx.ProtocolSystemUser.Where(x => x.i_SystemUserId == systemUserId).ToList();
                foreach (var item in listProt)
                {
                    item.i_IsDeleted = 1;
                    item.i_UpdateUserId = userId;
                    item.d_UpdateDate = DateTime.Now;
                    ctx.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
