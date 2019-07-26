using BE.Common;
using BE.Embarazo;
using BE.Message;
using DAL.Embarazo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BE.Common.Enumeratores;

namespace BL.EmbarazoBL
{
    public class EmbarazoBL
    {
        public MessageCustom AddEmbarazo(EmbarazoCustom objEmbarazo, int nodeId, int userId)
        {
            MessageCustom _MessageCustom = new MessageCustom();
            EmbarazoBE _EmbarazoBE = new EmbarazoBE();
            _EmbarazoBE.v_Complicacion = objEmbarazo.Complicacion;
            _EmbarazoBE.v_PersonId = objEmbarazo.PersonId;
            _EmbarazoBE.v_Anio = objEmbarazo.Anio;
            _EmbarazoBE.v_Cpn = objEmbarazo.Cpn;
            _EmbarazoBE.v_Parto = objEmbarazo.Parto;
            _EmbarazoBE.v_PesoRn = objEmbarazo.PesoRn;
            _EmbarazoBE.v_Puerpio = objEmbarazo.Puerpio;

            bool result = new EmbarazoDal().AddEmbarazo(_EmbarazoBE, nodeId, userId);
            if (!result)
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = (int)StatusHttp.BadRequest;
                _MessageCustom.Message = "Sucedió un error al agregar el embarazo, por favor vuelva a intentar.";
            }
            else
            {
                _MessageCustom.Error = false;
                _MessageCustom.Status = (int)StatusHttp.Ok;
                _MessageCustom.Message = "Se agregó correctamente.";

            }

            return _MessageCustom;
        }

        public List<EmbarazoCustom> GetEmbarazo(string personId)
        {
            return new EmbarazoDal().GetEmbarazo(personId);
        }
    }
}
