using BE;
using BE.Especiality;
using BE.Message;
using DAL.Especiality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BL.Especiality
{
    public class EspecialityBL
    {
        public static BoardEspeciality GetAllEspeciality(BoardEspeciality data)
        {
            return EspecialityDal.GetAllEspeciality(data);
        }

        public static EspecialityCustom GetEspecialityById(string especialityId)
        {
            return EspecialityDal.GetEspecialityById(especialityId);
        }

        public static MessageCustom AddEspeciality(EspecialityBE data)
        {
            MessageCustom msg = new MessageCustom();

            bool result = EspecialityDal.AddEspeciality(data);
            if (!result)
            {
                msg.Error = true;
                msg.Message = "Sucedió un error al agregar la especialidad, por favor vuelva intentar";
                msg.Status = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                msg.Error = false;
                msg.Message = "Se agregó correctamente";
                msg.Status = (int)HttpStatusCode.Created;
            }
            return msg;
        }

        public static MessageCustom DeletedEspeciality(string especialityId)
        {
            MessageCustom msg = new MessageCustom();

            bool result = EspecialityDal.DeletedEspeciality(especialityId);
            if (!result)
            {
                msg.Error = true;
                msg.Message = "Sucedió un error al eliminar la especialidad, por favor vuelva intentar";
                msg.Status = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                msg.Error = false;
                msg.Message = "Se eliminó correctamente";
                msg.Status = (int)HttpStatusCode.OK;
            }
            return msg;
        }
    }
}
