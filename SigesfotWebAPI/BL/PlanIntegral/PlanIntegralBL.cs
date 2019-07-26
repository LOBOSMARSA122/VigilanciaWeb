using BE.Common;
using BE.Message;
using BE.PlanIntegral;
using DAL.PlanIntegral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BE.Common.Enumeratores;

namespace BL.PlanIntegral
{
    public class PlanIntegralBL
    {
        public List<PlanIntegralList> GetPlanIntegralAndFiltered(string pstrPersonId)
        {
            return new PlanIntegralDal().GetPlanIntegralAndFiltered(pstrPersonId);
        }

        public List<ProblemaList> GetProblemaPagedAndFiltered(string pstrPersonId)
        {
            return new PlanIntegralDal().GetProblemaPagedAndFiltered(pstrPersonId);
        }

        public MessageCustom AddPlanIntegral(PlanIntegralList data, int nodeId, int userId)
        {
            MessageCustom _MessageCustom = new MessageCustom();
            PlanIntegralBE _PlanIntegralBE = new PlanIntegralBE();
            _PlanIntegralBE.d_Fecha = data.d_Fecha;
            _PlanIntegralBE.i_TipoId = data.i_TipoId;
            _PlanIntegralBE.v_Descripcion = data.v_Descripcion;
            _PlanIntegralBE.v_Lugar = data.v_Lugar;
            _PlanIntegralBE.v_PersonId = data.v_PersonId;
            var reult = new PlanIntegralDal().AddPlanIntegral(_PlanIntegralBE, nodeId, userId);
            if (!reult)
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = (int)StatusHttp.BadRequest;
                _MessageCustom.Message = "Sucedió un error al grabar, por favor vuelva a intentar";
            }
            else
            {
                _MessageCustom.Error = false;
                _MessageCustom.Status = (int)StatusHttp.Ok;
                _MessageCustom.Message = "Se grabo correctamente";
            }
            return _MessageCustom;
        }

        public MessageCustom AddProblema(ProblemaList data, int nodeId, int userId)
        {
            MessageCustom _MessageCustom = new MessageCustom();
            ProblemaBE _ProblemaBE = new ProblemaBE();
            _ProblemaBE.d_Fecha = data.d_Fecha;
            _ProblemaBE.i_Tipo = data.i_Tipo;
            _ProblemaBE.v_Descripcion = data.v_Descripcion;
            _ProblemaBE.v_Observacion = data.v_Observacion;
            _ProblemaBE.i_EsControlado = data.i_EsControlado;
            _ProblemaBE.v_PersonId = data.v_PersonId;
            var reult = new PlanIntegralDal().AddProblema(_ProblemaBE, nodeId, userId);
            if (!reult)
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = (int)StatusHttp.BadRequest;
                _MessageCustom.Message = "Sucedió un error al grabar, por favor vuelva a intentar";
            }
            else
            {
                _MessageCustom.Error = false;
                _MessageCustom.Status = (int)StatusHttp.Ok;
                _MessageCustom.Message = "Se grabo correctamente";
            }
            return _MessageCustom;
        }
    }
}
