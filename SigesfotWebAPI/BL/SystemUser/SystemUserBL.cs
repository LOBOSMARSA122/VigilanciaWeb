using BE.Common;
using BE.Message;
using BE.Protocol;
using BE.Security;
using DAL.Pacient;
using DAL.Security;
using DAL.SystemUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BL.SystemUser
{
    public class SystemUserBL
    {
        public static MessageCustom AddSystemUserExternal(PersonDto pobjPerson, ProfessionalBE pobjProfessional, SystemUserDto pobjSystemUser, List<ProtocolSystemUserBE> ListProtocolSystemUser, int userId, int nodeId)
        {
            pobjSystemUser.v_Password = SecurityDal.Encrypt(pobjSystemUser.v_Password.Trim());
            return new PacientDal().AddSystemUserExternal(pobjPerson, pobjProfessional, pobjSystemUser, ListProtocolSystemUser, userId, nodeId);
        }
        public static SystemUserCustom GetSystemUserById(int systemuserId)
        {
            return SystemUserDal.GetSystemUserById(systemuserId);
        }

        public static MessageCustom DeletedSystemUser(int systemUserId)
        {
            MessageCustom msg = new MessageCustom();
            bool result = SystemUserDal.DeletedSystemUser(systemUserId);
            if (!result)
            {
                msg.Error = true;
                msg.Status = (int)HttpStatusCode.BadRequest;
                msg.Message = "Sucedió un error al eliminar el usuario, por favor vuelva a intentar.";
            }
            else
            {
                msg.Error = false;
                msg.Status = (int)HttpStatusCode.OK;
                msg.Message = "Se eliminó correctamente.";
            }

            return msg;
        }
    }
}
