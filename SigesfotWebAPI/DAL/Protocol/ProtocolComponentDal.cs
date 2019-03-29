using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using BE.Protocol;
using DAL.Common;
using static BE.Common.Enumeratores;

namespace DAL.Protocol
{
    public class ProtocolComponentDal
    {
        private static DatabaseContext ctx = new DatabaseContext();

        public List<ProtocolComponentCustom> GetProtocolComponents(string protocolId)
        {
            var list = (from a in ctx.ProtocolComponent
                        join b in ctx.Component on a.v_ComponentId equals b.v_ComponentId
                        where a.v_ProtocolId == protocolId && a.i_IsDeleted == (int)SiNo.No
                        select new ProtocolComponentCustom
                        {
                            ProtocolComponentId = a.v_ProtocolComponentId,
                            ComponentTypeId = b.i_ComponentTypeId,
                            UIIsVisibleId = b.i_UIIsVisibleId,
                            UIIndex = b.i_UIIndex,
                            IdUnidadProductiva = b.v_IdUnidadProductiva,
                            Price = a.r_Price,
                            OperatorId = a.i_OperatorId,
                            Age = a.i_Age,
                            GenderId = a.i_GenderId,
                            GrupoEtarioId = a.i_GrupoEtarioId,
                            IsConditionalId =  a.i_IsConditionalId,
                            IsConditionalIMC = a.i_IsConditionalIMC,
                            IsAdditional = a.i_IsAdditional,
                            ComponentId = b.v_ComponentId
                        } ).ToList();
            
            return list;
        }
    }
}
