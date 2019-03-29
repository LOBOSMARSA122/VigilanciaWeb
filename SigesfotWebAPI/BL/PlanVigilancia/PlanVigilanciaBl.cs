using System.Collections.Generic;
using BE.Common;
using BE.Plan;
using DAL.Plan;

namespace BL.PlanVigilancia
{
    public class PlanVigilanciaBl
    {
        public BoardPlanVigilancia Filter(BoardPlanVigilancia data)
        {
            var list = new PlanDal().Filter(out int totalRecords, data);

            data.TotalRecords = totalRecords;
            data.List = list;

            return data;
        }

        public PlanVigilanciaCustom GetId(string planVigianciaId)
        {
            return new PlanDal().GetId(planVigianciaId);
        }

        public bool Save(PlanVigilanciaCustom oPlanVigilanciaCustom)
        {
            return new PlanDal().Save(oPlanVigilanciaCustom);
        }

        public bool Remove(string planVigilanciaId, int systemUserId)
        {
            return new PlanDal().Remove(planVigilanciaId,systemUserId);
        }

        public List<string> SearchDisease(string name)
        {
            return new PlanDal().SearchDisease(name);
        }

        public List<KeyValueDTO> ComboPlanesVigilancia(string organizationId)
        {
            return new PlanDal().ComboPlanesVigilancia(organizationId);
        }
    }
}
