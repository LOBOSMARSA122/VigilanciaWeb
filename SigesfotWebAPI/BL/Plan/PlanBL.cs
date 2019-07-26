using BE.Common;
using BE.Message;
using BE.Plan;
using DAL.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BL.Plan
{
    public class PlanBL
    {
        public static MessageCustom SavePlan(PlanBE entityPlan)
        {
            MessageCustom msg = new MessageCustom();


            var existPlan = PlanDal.ExistPlan(entityPlan);

            if (!existPlan)
            {
                var result = PlanDal.SavePlan(entityPlan);
                if (!result)
                {
                    msg.Error = true;
                    msg.Status = (int)HttpStatusCode.BadRequest;
                    msg.Message = "Sucedió un error al grabar el PLAN por favor vuelva a intentar.";
                }
                else
                {
                    msg.Error = false;
                    msg.Status = (int)HttpStatusCode.Created;
                    msg.Message = "Se creó correctamente.";
                }

            }
            else
            {
                msg.Error = false;
                msg.Status = (int)HttpStatusCode.Conflict;
                msg.Message = "El PLAN ya existe, por favor elija otra configuración.";
            }



            return msg;
        }

        public static List<PlanCustom> GetPlanByProtocolId(string protocolId)
        {
            return PlanDal.GetPlanByProtocolId(protocolId);
        }

        public static MessageCustom DeletedPlan(int planId)
        {
            MessageCustom msg = new MessageCustom();
            bool deleted = PlanDal.DeletedPlan(planId);

            msg.Error = deleted ?  false : true;
            msg.Status = deleted ? (int)HttpStatusCode.OK : (int)HttpStatusCode.Conflict;
            msg.Message = deleted ?  "Se eliminó correctamente." : "Sucedió un error al eliminar el PLAN, por favor vuelva a intentar.";


            return msg;
        }
    }
}
