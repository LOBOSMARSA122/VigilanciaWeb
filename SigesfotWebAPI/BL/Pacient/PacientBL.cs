using BE.Common;
using BE.Message;
using BE.Pacient;
using DAL.Pacient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BE.Common.Enumeratores;

namespace BL.Pacient
{
    public class PacientBL
    {
        PacientDal pacientDal = new PacientDal();
        public MessageCustom CreateOrUpdatePacient(PacientCustom data, int userId, int nodeId)
        {
            try
            {
                MessageCustom _MessageCustom = new MessageCustom();
                _MessageCustom.Error = true;
                _MessageCustom.Message = Constants.BAD_REQUEST;
                _MessageCustom.Status = (int)StatusHttp.BadRequest;

                if (data.ActionType == (int)ActionType.Create)
                {
                    PacientCustom objPerson = pacientDal.FindPacientByDocNumberOrPersonId(data.v_DocNumber);
                    string personId = "";
                    if (objPerson != null)//si ya existe el paciente se actualiza
                    {
                        personId = pacientDal.UpdatePacient(data, userId);
                        if (personId == null)
                        {
                            return _MessageCustom;
                        }
                        else
                        {
                            _MessageCustom.Error = false;
                            _MessageCustom.Id = personId;
                            _MessageCustom.Message = Constants.OK_RESULT;
                            _MessageCustom.Status = (int)StatusHttp.Ok;
                        }
                    }
                    else
                    {
                        personId = pacientDal.CreatePacient(data, userId, nodeId);

                        if (personId == null)
                        {
                            return _MessageCustom;
                        }
                        else
                        {
                            _MessageCustom.Error = false;
                            _MessageCustom.Id = personId;
                            _MessageCustom.Message = Constants.OK_RESULT;
                            _MessageCustom.Status = (int)StatusHttp.Ok;
                        }

                    }

                    

                }
                else if (data.ActionType == (int)ActionType.Edit)
                {
                    string personId = pacientDal.UpdatePacient(data, userId);
                    if (personId == null)
                    {
                        return _MessageCustom;
                    }
                    else
                    {
                        _MessageCustom.Error = false;
                        _MessageCustom.Id = personId;
                        _MessageCustom.Message = Constants.OK_RESULT;
                        _MessageCustom.Status = (int)StatusHttp.Ok;
                    }

                }
                else
                {

                }

                return _MessageCustom;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public PacientCustom FindPacientByDocNumberOrPersonId(string value)
        {
            return pacientDal.FindPacientByDocNumberOrPersonId(value);
        }

    }
}
