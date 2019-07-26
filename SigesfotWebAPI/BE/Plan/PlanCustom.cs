using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Plan
{
    public class PlanCustom
    {
        public int i_PlanId { get; set; }
        public string v_OrganizationSeguroId { get; set; }
        public string v_ProtocoloId { get; set; }
        public string v_IdUnidadProductiva { get; set; }
        public int? i_EsDeducible { get; set; }
        public int? i_EsCoaseguro { get; set; }
        public decimal? d_Importe { get; set; }
        public decimal? d_ImporteCo { get; set; }
        public string v_IdUnidadProductivaName { get; set; }
    }
}
